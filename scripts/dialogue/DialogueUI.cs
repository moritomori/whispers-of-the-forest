using Godot;
using System;
using WhispersOfTheForest.Data;

namespace WhispersOfTheForest.Dialogue;

/// <summary>
/// Displays dialogue text, speaker information, and dialogue choices.
/// Subscribes to DialogueController events and updates the dialogue UI.
/// </summary>
public partial class DialogueUI : Control
{
	private DialogueController? _controller;

	// Speaker panels and their UI elements
	private Control? _leftSpeaker;
	private Label? _nameLeft;
	private TextureRect? _portraitLeft;

	private Control? _rightSpeaker;
	private Label? _nameRight;
	private TextureRect? _portraitRight;

	// Main dialogue UI elements
	private RichTextLabel? _textLabel;
	private HBoxContainer? _choicesBar;
	private Button? _continueButton;

	public override void _Ready()
	{
		// Find the dialogue controller in the scene group
		if (GetTree() is SceneTree sceneTree
			&& sceneTree.GetFirstNodeInGroup("dialogue_controller") is DialogueController dialogueController)
		{
			_controller = dialogueController;
		}
		else
		{
			GD.PushError("[DialogueUI] DialogueController not found in group 'dialogue_controller'.");
		}

		// Top row: speaker name containers
		_leftSpeaker = GetNodeOrNull<Control>("RootVBox/Panel/VBoxContainer/TopRow/LeftSpeaker");
		_nameLeft = GetNodeOrNull<Label>("RootVBox/Panel/VBoxContainer/TopRow/LeftSpeaker/NameLeft");

		_rightSpeaker = GetNodeOrNull<Control>("RootVBox/Panel/VBoxContainer/TopRow/RightSpeaker");
		_nameRight = GetNodeOrNull<Label>("RootVBox/Panel/VBoxContainer/TopRow/RightSpeaker/NameRight");

		// Center row: portraits and dialogue text
		_portraitLeft = GetNodeOrNull<TextureRect>("RootVBox/Panel/VBoxContainer/CenterRow/PortraitLeft");
		_portraitRight = GetNodeOrNull<TextureRect>("RootVBox/Panel/VBoxContainer/CenterRow/PortraitRight");
		_textLabel = GetNodeOrNull<RichTextLabel>("RootVBox/Panel/VBoxContainer/CenterRow/TextLabel");

		// Bottom row: continue button and choice container
		_continueButton = GetNodeOrNull<Button>("RootVBox/Panel/VBoxContainer/BottomRow/ContinueButton");
		_choicesBar = GetNodeOrNull<HBoxContainer>("RootVBox/ChoicesBar");

		// Stop initialization if any required UI node is missing
		if (_leftSpeaker is null || _nameLeft is null || _portraitLeft is null ||
			_rightSpeaker is null || _nameRight is null || _portraitRight is null ||
			_textLabel is null || _choicesBar is null || _continueButton is null)
		{
			GD.PushError("[DialogueUI] One or more UI nodes were not found.");
			return;
		}

		// Subscribe to the continue button
		_continueButton.Pressed += OnContinuePressed;

		// Subscribe to dialogue controller events
		if (_controller is not null)
		{
			_controller.DialogueStarted += OnDialogueStarted;
			_controller.DialogueUpdated += OnDialogueUpdated;
			_controller.ChoicesUpdated += OnChoicesUpdated;
			_controller.DialogueEnded += OnDialogueEnded;
		}

		// Dialogue UI is hidden until a dialogue starts
		Visible = false;
	}

	/// <summary>
	/// Unsubscribes from events before the node is deleted.
	/// </summary>
	public override void _Notification(int what)
	{
		base._Notification(what);

		if (what == NotificationPredelete)
		{
			if (_continueButton is not null)
			{
				_continueButton.Pressed -= OnContinuePressed;
			}

			if (_controller is not null)
			{
				_controller.DialogueStarted -= OnDialogueStarted;
				_controller.DialogueUpdated -= OnDialogueUpdated;
				_controller.ChoicesUpdated -= OnChoicesUpdated;
				_controller.DialogueEnded -= OnDialogueEnded;
			}
		}
	}

	/// <summary>
	/// Continues the active dialogue when the continue button is pressed.
	/// </summary>
	private void OnContinuePressed()
	{
		if (_controller is not null && _controller.IsDialogueActive)
		{
			_controller.Continue();
		}
	}

	/// <summary>
	/// Shows the dialogue UI and prepares it for a new dialogue.
	/// </summary>
	private void OnDialogueStarted()
	{
		Visible = true;
		_textLabel!.Text = string.Empty;
		ClearChoices();

		_continueButton!.Visible = true;
	}

	/// <summary>
	/// Updates the speaker UI and dialogue text.
	/// </summary>
	private void OnDialogueUpdated(string speakerId, string text)
	{
		ApplySpeaker(speakerId);
		_textLabel!.Text = text;
	}

	/// <summary>
	/// Updates the available dialogue choices.
	/// If there are no choices, the continue button is shown instead.
	/// </summary>
	private void OnChoicesUpdated(string[] choices)
	{
		ClearChoices();

		if (choices.Length == 0)
		{
			_continueButton!.Visible = true;
			return;
		}

		_continueButton!.Visible = false;

		for (int i = 0; i < choices.Length; i++)
		{
			int idx = i;

			var btn = new Button
			{
				Text = $"{idx + 1}) {choices[i]}"
			};

			// TODO: Replace lambda with a safer non-capturing approach if this UI is refactored further.
			btn.Pressed += () => _controller?.Choose(idx);

			_choicesBar!.AddChild(btn);
		}
	}

	/// <summary>
	/// Hides the dialogue UI and clears its content.
	/// </summary>
	private void OnDialogueEnded()
	{
		Visible = false;
		_textLabel!.Text = string.Empty;
		ClearChoices();
	}

	/// <summary>
	/// Removes all currently displayed choice buttons.
	/// </summary>
	private void ClearChoices()
	{
		foreach (Node child in _choicesBar!.GetChildren())
		{
			child.QueueFree();
		}
	}

	/// <summary>
	/// Applies the speaker name and portrait to the correct side of the dialogue UI.
	/// </summary>
	private void ApplySpeaker(string speakerId)
	{
		bool isPlayer = string.Equals(
			speakerId,
			"player",
			StringComparison.OrdinalIgnoreCase
		);

		// TODO: Replace CharacterDB.Instance with an injected or exported dependency.
		var def = CharacterDB.Instance?.Get(speakerId);
		string displayName = def?.DisplayName ?? speakerId;
		Texture2D? portrait = def?.Portrait;

		if (isPlayer)
		{
			_rightSpeaker!.Visible = true;
			_leftSpeaker!.Visible = false;

			_nameRight!.Text = displayName;
			_portraitRight!.Texture = portrait;
		}
		else
		{
			_leftSpeaker!.Visible = true;
			_rightSpeaker!.Visible = false;

			_nameLeft!.Text = displayName;
			_portraitLeft!.Texture = portrait;
		}
	}

	/// <summary>
	/// Handles keyboard input for continuing dialogue and selecting choices.
	/// </summary>
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!Visible || _controller is null || !_controller.IsDialogueActive)
		{
			return;
		}

		// Enter / Space -> continue dialogue
		if (@event.IsActionPressed("ui_accept"))
		{
			_controller.Continue();
			GetViewport().SetInputAsHandled();
		}

		// Keys 1..9 -> select dialogue choice
		if (@event is InputEventKey key && key.Pressed && !key.Echo)
		{
			int digit = KeyToDigit(key.Keycode);
			if (digit >= 1 && digit <= 9)
			{
				_controller.Choose(digit - 1);
				GetViewport().SetInputAsHandled();
			}
		}
	}

	/// <summary>
	/// Converts numeric key input to a dialogue choice index.
	/// </summary>
	private static int KeyToDigit(Key key) => key switch
	{
		Key.Key1 => 1,
		Key.Key2 => 2,
		Key.Key3 => 3,
		Key.Key4 => 4,
		Key.Key5 => 5,
		Key.Key6 => 6,
		Key.Key7 => 7,
		Key.Key8 => 8,
		Key.Key9 => 9,
		_ => -1
	};
}