using Godot;
using GodotInk;
using Ink.Parsed;
using System;
using System.Collections.Generic;
using System.Linq;
using WhispersOfTheForest.Story;

namespace WhispersOfTheForest.Dialogue;
public partial class DialogueController : Node
{
	public bool IsDialogueActive { get; private set; }
	private InkStory? _story;
	private readonly HashSet<InkStory> _boundStories = new();

	[Signal] public delegate void DialogueStartedEventHandler();
	[Signal] public delegate void DialogueUpdatedEventHandler(string speakerId, string text);
	[Signal] public delegate void ChoicesUpdatedEventHandler(string[] choices);
	[Signal] public delegate void DialogueEndedEventHandler();

	public override void _EnterTree()
	{
		AddToGroup("dialogue_controller");
	}

	public void StartDialogue(InkStory? story, string knot = "start")
	{
		GD.Print($"[DialogueController] StartDialogue requested. Active={IsDialogueActive}, story={(story?.ResourcePath ?? "NULL")}, knot={knot}");

		if (IsDialogueActive) return;

		if (story is null)
		{
			GD.PushError("[DialogueController] Cannot start dialogue because story is null.");
			return;
		}
		_story = story;
		_story.ResetState();
		_story.ChoosePathString(knot);

		BindInkApi(_story);

		IsDialogueActive = true;
		EmitSignal(SignalName.DialogueStarted);

		Step();
	}

	public void Choose(int index)
	{
		if (!IsDialogueActive || _story is null) return;
		if (index < 0 || index >= _story.CurrentChoices.Count) return;

		_story.ChooseChoiceIndex(index);
		Step();
	}

	public void Continue()
	{
		if (!IsDialogueActive || _story is null) return;
		Step();
	}

	private void Step()
	{
		if (_story is null)
		{
			EndDialogue();
			return;
		}

		if (_story.CurrentChoices.Count > 0)
		{
			EmitSignal(SignalName.ChoicesUpdated, _story.CurrentChoices.Select(c => c.Text).ToArray());
			return;
		}

		string? textToShow = null;
		string speakerId = "player";

		while (_story.GetCanContinue())
		{
			string raw = _story.Continue() ?? string.Empty;
			string trimmed = raw.Trim();

			speakerId = ExtractSpeakerId(_story);

			if (!string.IsNullOrEmpty(trimmed))
			{
				textToShow = trimmed;
				break;
			}
		}

		if (!string.IsNullOrEmpty(textToShow))
		{
			EmitSignal(SignalName.DialogueUpdated, speakerId, textToShow);
		}

		string[] choices = _story.CurrentChoices.Select(c => c.Text).ToArray();
		EmitSignal(SignalName.ChoicesUpdated, choices);

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
