using Godot;
using GodotInk;
using System;
using System.Collections.Generic;
using System.Linq;
using WhispersOfTheForest.Story;

namespace WhispersOfTheForest.Dialogue;

/// <summary>
/// Controls the dialogue flow between Ink stories and the game UI.
/// Starts dialogues, advances story lines, handles choices,
/// and emits signals for dialogue updates.
/// </summary>
public partial class DialogueController : Node
{
	[Export] private StoryManager? _storyManager;

	public bool IsDialogueActive { get; private set; }

	private InkStory? _story;
	private readonly HashSet<InkStory> _boundStories = new();

	private StoryFlags? StoryFlags => _storyManager?.StoryFlags;
	private StoryVars? StoryVars => _storyManager?.StoryVars;

	[Signal] public delegate void DialogueStartedEventHandler();
	[Signal] public delegate void DialogueUpdatedEventHandler(string speakerId, string text);
	[Signal] public delegate void ChoicesUpdatedEventHandler(string[] choices);
	[Signal] public delegate void DialogueEndedEventHandler();

	public override void _EnterTree()
	{
		AddToGroup("dialogue_controller");
	}

	public override void _Ready()
	{
		if (_storyManager is null)
		{
			GD.PushError("[DialogueController] StoryManager is not assigned.");
		}
	}

	/// <summary>
	/// Starts a new dialogue from the given Ink story and knot.
	/// </summary>
	public void StartDialogue(InkStory? story, string knot = "start")
	{
		GD.Print($"[DialogueController] StartDialogue requested. Active={IsDialogueActive}, story={(story?.ResourcePath ?? "NULL")}, knot={knot}");

		if (IsDialogueActive)
			return;

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

	/// <summary>
	/// Selects one of the currently available dialogue choices.
	/// </summary>
	public void Choose(int index)
	{
		if (!IsDialogueActive || _story is null)
			return;

		if (index < 0 || index >= _story.CurrentChoices.Count)
			return;

		_story.ChooseChoiceIndex(index);
		Step();
	}

	/// <summary>
	/// Continues the current dialogue to the next line or choice.
	/// </summary>
	public void Continue()
	{
		if (!IsDialogueActive || _story is null)
			return;

		Step();
	}

	/// <summary>
	/// Advances the dialogue state and emits either dialogue text,
	/// available choices, or ends the dialogue if no content remains.
	/// </summary>
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

	/// <summary>
	/// Extracts the current speaker identifier from Ink tags.
	/// Returns "player" if no speaker tag is found.
	/// </summary>
	private static string ExtractSpeakerId(InkStory story)
	{
		foreach (string tag in story.CurrentTags)
		{
			string trimmedTag = tag.Trim();
			if (trimmedTag.StartsWith("speaker:", StringComparison.OrdinalIgnoreCase))
			{
				return trimmedTag.Substring("speaker:".Length).Trim();
			}
		}

		return "player";
	}

	/// <summary>
	/// Ends the active dialogue and clears the current story reference.
	/// </summary>
	public void EndDialogue()
	{
		GD.Print("[DialogueController] EndDialogue()");
		IsDialogueActive = false;
		_story = null;
		EmitSignal(SignalName.DialogueEnded);
	}

	/// <summary>
	/// Binds external Ink functions to the current story systems.
	/// </summary>
	// TODO: Refactor story bindings into a separate service.
	private void BindInkApi(InkStory story)
	{
		if (_boundStories.Contains(story))
			return;

		if (_storyManager is null)
		{
			GD.PushError("[DialogueController] Cannot bind Ink API because StoryManager is not assigned.");
			return;
		}

		if (StoryFlags is null || StoryVars is null)
		{
			GD.PushError("[DialogueController] Cannot bind Ink API because StoryFlags or StoryVars is not assigned.");
			return;
		}

		_boundStories.Add(story);
		GD.Print("[Ink] Binding external API");

		// Story flags
		story.BindExternalFunction("set_flag", (string flagName, bool value) =>
		{
			GD.Print($"[Ink] set_flag {flagName} = {value}");

			switch (flagName)
			{
				case "HelpedSpirit":
					StoryFlags.HelpedSpirit = value;
					break;
				case "IgnoredSpirit":
					StoryFlags.IgnoredSpirit = value;
					break;
				case "HarmedForest":
					StoryFlags.HarmedForest = value;
					break;
				case "TalkedToSpirit":
					StoryFlags.TalkedToSpirit = value;
					break;
				case "TalkedToWanderer":
					StoryFlags.TalkedToWanderer = value;
					break;
				case "TalkedToElder":
					StoryFlags.TalkedToElder = value;
					break;
				default:
					GD.PrintErr($"[Ink] Unknown flag: '{flagName}'");
					break;
			}
		});

		// Story variables
		story.BindExternalFunction("add_harmony", (int value) =>
		{
			StoryVars.Harmony += value;
			GD.Print($"[StoryVars] Harmony = {StoryVars.Harmony}");
		});

		story.BindExternalFunction("add_ruthlessness", (int value) =>
		{
			StoryVars.Ruthlessness += value;
			GD.Print($"[StoryVars] Ruthlessness = {StoryVars.Ruthlessness}");
		});

		// World state
		story.BindExternalFunction("recalc_world", () =>
		{
			_storyManager.RecalculateWorldState();
			GD.Print($"[StoryManager] WorldState = {_storyManager.CurrentWorldState}");
		});

		story.BindExternalFunction("get_world_state", () =>
		{
			string worldState = _storyManager.CurrentWorldState.ToString();
			GD.Print($"[Ink] get_world_state() -> {worldState}");
			return worldState;
		});
	}
}