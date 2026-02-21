using Godot;
using System;

namespace Test
{
	public partial class DialogueUI : Control
	{
		private DialogueController _controller;

		// -------- SPEAKERS --------
		private Control _leftSpeaker;
		private Label _nameLeft;
		private TextureRect _portraitLeft;

		private Control _rightSpeaker;
		private Label _nameRight;
		private TextureRect _portraitRight;

		// -------- TEXT / UI --------
		private RichTextLabel _textLabel;
		private HBoxContainer _choicesBar;
		private Button _continueButton;

		public override void _Ready()
		{
			_controller = GetTree().GetFirstNodeInGroup("dialogue_controller") as DialogueController;

			// ---- TOP ROW ----
			_leftSpeaker = GetNode<Control>("RootVBox/Panel/VBoxContainer/TopRow/LeftSpeaker");
			_nameLeft = GetNode<Label>("RootVBox/Panel/VBoxContainer/TopRow/LeftSpeaker/NameLeft");

			_rightSpeaker = GetNode<Control>("RootVBox/Panel/VBoxContainer/TopRow/RightSpeaker");
			_nameRight = GetNode<Label>("RootVBox/Panel/VBoxContainer/TopRow/RightSpeaker/NameRight");

			// ---- CENTER ROW ----
			_portraitLeft = GetNode<TextureRect>("RootVBox/Panel/VBoxContainer/CenterRow/PortraitLeft");
			_portraitRight = GetNode<TextureRect>("RootVBox/Panel/VBoxContainer/CenterRow/PortraitRight");
			_textLabel = GetNode<RichTextLabel>("RootVBox/Panel/VBoxContainer/CenterRow/TextLabel");

			// ---- BOTTOM / CHOICES ----
			_continueButton = GetNode<Button>(
				"RootVBox/Panel/VBoxContainer/BottomRow/ContinueButton"
			);
			_choicesBar = GetNode<HBoxContainer>("RootVBox/ChoicesBar");

			// ---- CONTINUE BUTTON ----
			_continueButton.Pressed += () =>
			{
				if (_controller != null && _controller.IsDialogueActive)
					_controller.Continue();
			};

			// ---- CONTROLLER EVENTS ----
			if (_controller != null)
			{
				_controller.DialogueStarted += OnDialogueStarted;
				_controller.DialogueUpdated += OnDialogueUpdated;
				_controller.ChoicesUpdated += OnChoicesUpdated;
				_controller.DialogueEnded += OnDialogueEnded;
			}

			Visible = false;
		}

		// ================= EVENTS =================

		private void OnDialogueStarted()
		{
			Visible = true;
			_textLabel.Text = "";
			ClearChoices();

			_continueButton.Visible = true;
		}

		private void OnDialogueUpdated(string speakerId, string text)
		{
			ApplySpeaker(speakerId);
			_textLabel.Text = text;
		}

		private void OnChoicesUpdated(string[] choices)
		{
			ClearChoices();

			// ---- NO CHOICES → CONTINUE ----
			if (choices == null || choices.Length == 0)
			{
				_continueButton.Visible = true;
				return;
			}

			// ---- HAS CHOICES ----
			_continueButton.Visible = false;

			for (int i = 0; i < choices.Length; i++)
			{
				int idx = i;

				var btn = new Button
				{
					Text = $"{idx + 1}) {choices[i]}"
				};

				btn.Pressed += () => _controller.Choose(idx);
				_choicesBar.AddChild(btn);
			}
		}

		private void OnDialogueEnded()
		{
			Visible = false;
			_textLabel.Text = "";
			ClearChoices();
		}

		// ================= HELPERS =================

		private void ClearChoices()
		{
			foreach (var child in _choicesBar.GetChildren())
				child.QueueFree();
		}

		private void ApplySpeaker(string speakerId)
		{
			bool isPlayer = string.Equals(
				speakerId,
				"player",
				StringComparison.OrdinalIgnoreCase
			);

			var def = CharacterDB.Instance?.Get(speakerId);
			string displayName = def?.DisplayName ?? speakerId;
			var portrait = def?.Portrait;

			if (isPlayer)
			{
				_rightSpeaker.Visible = true;
				_leftSpeaker.Visible = false;

				_nameRight.Text = displayName;
				_portraitRight.Texture = portrait;
			}
			else
			{
				_leftSpeaker.Visible = true;
				_rightSpeaker.Visible = false;

				_nameLeft.Text = displayName;
				_portraitLeft.Texture = portrait;
			}
		}

		// ================= INPUT =================

		public override void _UnhandledInput(InputEvent @event)
		{
			if (!Visible || _controller == null || !_controller.IsDialogueActive)
				return;

			// ENTER / SPACE → CONTINUE
			if (@event.IsActionPressed("ui_accept"))
			{
				_controller.Continue();
				GetViewport().SetInputAsHandled();
			}

			// 1..9 → CHOICE
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
}
