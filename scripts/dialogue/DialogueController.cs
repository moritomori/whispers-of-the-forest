using Godot;
using GodotInk;
using Ink.Parsed;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.scripts.story;
using WhispersOfTheForest.scripts.story;

public partial class DialogueController : Node
{
	public bool IsDialogueActive { get; private set; }
	private InkStory _story;
	private readonly HashSet<InkStory> _boundStories = new();
	private bool _inkApiBound;

	[Signal] public delegate void DialogueStartedEventHandler();
	[Signal] public delegate void DialogueUpdatedEventHandler(string speakerId, string text);
	[Signal] public delegate void ChoicesUpdatedEventHandler(string[] choices);
	[Signal] public delegate void DialogueEndedEventHandler();

	public override void _EnterTree()
	{
		AddToGroup("dialogue_controller");
	}

	public void StartDialogue(InkStory story, string knot = "start")
	{
		GD.Print($"[DialogueController] StartDialogue requested. Active={IsDialogueActive}, story={(story?.ResourcePath ?? "NULL")}, knot={knot}");

		if (IsDialogueActive) return;

		_story = story ?? throw new ArgumentNullException(nameof(story));
		_story.ResetState();
		_story.ChoosePathString(knot);

		BindInkApi(_story); //Ink + flags

		IsDialogueActive = true;
		EmitSignal(SignalName.DialogueStarted);

		Step();
	}

	public void Choose(int index)
	{
		if (!IsDialogueActive || _story == null) return;
		if (index < 0 || index >= _story.CurrentChoices.Count) return;

		_story.ChooseChoiceIndex(index);
		Step();
	}

	public void Continue()
	{
		if (!IsDialogueActive || _story == null) return;
		Step();
	}

	private void Step()
	{
		if (_story == null)
		{
			EndDialogue();
			return;
		}

		// If there are choices, display the choices and exit (wait for selection).
		if (_story.CurrentChoices.Count > 0)
		{
			EmitSignal(SignalName.ChoicesUpdated, _story.CurrentChoices.Select(c => c.Text).ToArray());
			return;
		}

		// Let's continue, skipping empty lines
		string textToShow = null;
		string speakerId = "player";

		while (_story.GetCanContinue())
		{
			var raw = _story.Continue() ?? "";
			var trimmed = raw.Trim();

			speakerId = ExtractSpeakerId(_story);

			if (!string.IsNullOrEmpty(trimmed))
			{
				textToShow = trimmed;
				break;
			}
		}

		// If there is text, display it
		if (!string.IsNullOrEmpty(textToShow))
		{
			EmitSignal(SignalName.DialogueUpdated, speakerId, textToShow);
		}

		// After scrolling, refresh choices again
		var choices = _story.CurrentChoices.Select(c => c.Text).ToArray();
		EmitSignal(SignalName.ChoicesUpdated, choices);

		// If there is no text, no choices, and no continue, we finish.
		if (!_story.GetCanContinue() && _story.CurrentChoices.Count == 0 && string.IsNullOrEmpty(textToShow))
		{
			EndDialogue();
		}
	}

	private static string ExtractSpeakerId(InkStory story)
	{
		foreach (var tag in story.CurrentTags)
		{
			var t = tag.Trim();
			if (t.StartsWith("speaker:", StringComparison.OrdinalIgnoreCase))
				return t.Substring("speaker:".Length).Trim();
		}
		return "player";
	}

	public void EndDialogue()
	{
		GD.Print("[DialogueController] EndDialogue()");
		IsDialogueActive = false;
		_story = null;
		EmitSignal(SignalName.DialogueEnded);
	}
	private void BindInkApi(InkStory story)
	{
		if (story == null) return;
		if (_boundStories.Contains(story)) return;

		_boundStories.Add(story);
		GD.Print("[Ink] Binding external API");


		// FLAGS (bool)
		story.BindExternalFunction("set_flag", (string flagName, bool value) =>
		{
			GD.Print($"[Ink] set_flag {flagName} = {value}");

			switch (flagName)
			{
				// decisions
				case "HelpedSpirit":
					StoryFlags.HelpedSpirit = value; break;
				case "IgnoredSpirit":
					StoryFlags.IgnoredSpirit = value; break;
				case "HarmedForest":
					StoryFlags.HarmedForest = value; break;

				// progress
				case "TalkedToSpirit":
					StoryFlags.TalkedToSpirit = value; break;
				case "TalkedToWanderer":
					StoryFlags.TalkedToWanderer = value; break;
				case "TalkedToElder":
					StoryFlags.TalkedToElder = value; break;

				default:
					GD.PrintErr($"[Ink] Unknown flag: '{flagName}'");
					break;
			}
		});

		// VARS (int)
		story.BindExternalFunction("add_harmony", (int v) =>
		{
			StoryVars.Harmony += v;
			GD.Print($"[StoryVars] Harmony = {StoryVars.Harmony}");
		});

		story.BindExternalFunction("add_ruthlessness", (int v) =>
		{
			StoryVars.Ruthlessness += v;
			GD.Print($"[StoryVars] Ruthlessness = {StoryVars.Ruthlessness}");
		});

		// WORLD STATE
		story.BindExternalFunction("recalc_world", () =>
		{
			StoryManager.Instance.RecalculateWorldState();
			GD.Print($"[StoryManager] WorldState = {StoryManager.Instance.CurrentWorldState}");
		});

		story.BindExternalFunction("get_world_state", () =>
		{
			var ws = StoryManager.Instance.CurrentWorldState.ToString();
			GD.Print($"[Ink] get_world_state() -> {ws}");
			return ws;
		});
	}



}
