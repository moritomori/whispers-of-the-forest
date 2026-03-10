using Godot;
using WhispersOfTheForest.Dialogue;

namespace WhispersOfTheForest.Core;

/// <summary>
/// Controls the main player character.
/// Handles movement, interaction, and movement blocking during dialogue.
/// </summary>
public partial class Player : CharacterBody2D
{
	[Export] private float Speed { get; set; } = 200.0f;

	private InteractionArea? _interactionArea;
	private DialogueController? _dialogueSystem;

	public override void _Ready()
	{
		_interactionArea = GetNodeOrNull<InteractionArea>("InteractionArea");
		if (_interactionArea is null)
		{
			GD.PushError("[Player] InteractionArea not found.");
		}

		if (GetTree() is SceneTree sceneTree
			&& sceneTree.GetFirstNodeInGroup("dialogue_controller") is DialogueController dialogueController)
		{
			_dialogueSystem = dialogueController;
		}
		else
		{
			GD.PushWarning("[Player] DialogueController not found in group 'dialogue_controller'.");
		}
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (_dialogueSystem is not null && _dialogueSystem.IsDialogueActive)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		Vector2 direction = Vector2.Zero;

		if (Input.IsActionPressed("move_up"))
			direction.Y -= 1;

		if (Input.IsActionPressed("move_down"))
			direction.Y += 1;

		if (Input.IsActionPressed("move_left"))
			direction.X -= 1;

		if (Input.IsActionPressed("move_right"))
			direction.X += 1;

		direction = direction.Normalized();
		Velocity = direction * Speed;

		MoveAndSlide();

		if (Input.IsActionJustPressed("interact"))
		{
			TryInteract();
		}
	}

	/// <summary>
	/// Attempts to interact with a nearby interactable object.
	/// </summary>
	private void TryInteract()
	{
		if (_interactionArea is null)
		{
			GD.PushError("[Player] Cannot interact because InteractionArea is not assigned.");
			return;
		}

		var interactable = _interactionArea.GetInteractable();
		if (interactable is not null)
		{
			interactable.Interact(this);
		}
	}
}