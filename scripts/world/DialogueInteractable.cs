using Godot;
using GodotInk;
using WhispersOfTheForest.Core;
using WhispersOfTheForest.Dialogue;

namespace WhispersOfTheForest.World;

/// <summary>
/// Plays a small Ink dialogue when the player interacts with this world object.
/// </summary>
public partial class DialogueInteractable : Area2D, IInteractable, ILocationContextConsumer
{
	[Export] private InkStory? _story;
	[Export] private string _knot = "start";

	private LocationContext? _locationContext;

	public void SetLocationContext(LocationContext context)
	{
		_locationContext = context;
	}

	public override void _Ready()
	{
		if (_story is null)
		{
			GD.PushError("[DialogueInteractable] InkStory is not assigned.");
		}
	}

	public void Interact(Player player)
	{
		if (_locationContext?.DialogueController is not DialogueController dialogueController)
		{
			GD.PushError("[DialogueInteractable] DialogueController is not available through LocationContext.");
			return;
		}

		if (_story is null)
		{
			GD.PushError("[DialogueInteractable] Cannot interact because InkStory is not assigned.");
			return;
		}

		if (dialogueController.IsDialogueActive)
			return;

		dialogueController.StartDialogue(_story, _knot);
	}
}