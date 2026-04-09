using Godot;
using GodotInk;
using WhispersOfTheForest.Core;
using WhispersOfTheForest.Dialogue;

namespace WhispersOfTheForest.World;

/// <summary>
/// Represents a spirit NPC that can be revealed by story triggers.
/// </summary>
public partial class SpiritNpc : Area2D, IInteractable, ILocationContextConsumer, IRevealable
{
	[Export] private InkStory? _dialogueStory;
	[Export] private string _knot = "start";
	[Export] private Node2D? _visualRoot;
	[Export] private CollisionShape2D? _collisionShape;

	private LocationContext? _locationContext;

	public void SetLocationContext(LocationContext context)
	{
		_locationContext = context;
	}

	public override void _Ready()
	{
		if (_dialogueStory is null)
		{
			GD.PushError("[SpiritNpc] Dialogue story is not assigned.");
		}

		ApplyVisibleState(false);
	}

	public void Reveal()
	{
		ApplyVisibleState(true);
		GD.Print("[SpiritNpc] Spirit revealed.");
	}

	public void HideSpirit()
	{
		ApplyVisibleState(false);
	}

	public void Interact(Player player)
	{
		if (_locationContext?.DialogueController is not DialogueController dialogueController)
		{
			GD.PushError("[SpiritNpc] DialogueController is not available through LocationContext.");
			return;
		}

		if (_dialogueStory is null)
		{
			GD.PushError("[SpiritNpc] Dialogue story is not assigned.");
			return;
		}

		if (dialogueController.IsDialogueActive)
			return;

		dialogueController.StartDialogue(_dialogueStory, _knot);
	}

	private void ApplyVisibleState(bool isVisible)
	{
		Visible = isVisible;

		if (_visualRoot is not null)
		{
			_visualRoot.Visible = isVisible;
		}

		if (_collisionShape is not null)
		{
			_collisionShape.Disabled = !isVisible;
		}

		Monitoring = isVisible;
		Monitorable = isVisible;
	}
}