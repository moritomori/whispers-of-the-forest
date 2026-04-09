using Godot;
using GodotInk;
using WhispersOfTheForest.Core;
using WhispersOfTheForest.Dialogue;

namespace WhispersOfTheForest.World;

/// <summary>
/// Plays an Ink dialogue and optionally reveals a target node after the dialogue ends.
/// </summary>
public partial class DialogueTriggerInteractable : Area2D, IInteractable, ILocationContextConsumer
{
	[Export] private InkStory? _story;
	[Export] private string _knot = "start";
	[Export] private Node2D? _nodeToRevealAfterDialogue;

	private LocationContext? _locationContext;
	private DialogueController? _dialogueController;
	private bool _waitingForDialogueEnd;

	public void SetLocationContext(LocationContext context)
	{
		_locationContext = context;
		_dialogueController = context.DialogueController;
	}

	public override void _Ready()
	{
		if (_story is null)
		{
			GD.PushError("[DialogueTriggerInteractable] InkStory is not assigned.");
		}
	}

	public override void _ExitTree()
	{
		if (_dialogueController is not null)
		{
			_dialogueController.DialogueEnded -= OnDialogueEnded;
		}
	}

	public void Interact(Player player)
	{
		if (_locationContext?.DialogueController is not DialogueController dialogueController)
		{
			GD.PushError("[DialogueTriggerInteractable] DialogueController is not available through LocationContext.");
			return;
		}

		if (_story is null)
		{
			GD.PushError("[DialogueTriggerInteractable] Cannot interact because InkStory is not assigned.");
			return;
		}

		if (dialogueController.IsDialogueActive)
			return;

		dialogueController.DialogueEnded -= OnDialogueEnded;
		dialogueController.DialogueEnded += OnDialogueEnded;

		_waitingForDialogueEnd = true;
		dialogueController.StartDialogue(_story, _knot);
	}

	private void OnDialogueEnded()
	{
		if (!_waitingForDialogueEnd)
			return;

		_waitingForDialogueEnd = false;

		if (_dialogueController is not null)
		{
			_dialogueController.DialogueEnded -= OnDialogueEnded;
		}

		GD.Print("[DialogueTriggerInteractable] Dialogue ended. Trying to reveal target.");
		RevealTargetNode();
	}

	private void RevealTargetNode()
	{
		if (_nodeToRevealAfterDialogue is null)
			return;

		if (_nodeToRevealAfterDialogue is IRevealable revealable)
		{
			revealable.Reveal();
			GD.Print($"[DialogueTriggerInteractable] Revealed target via IRevealable: '{_nodeToRevealAfterDialogue.Name}'.");
			return;
		}

		_nodeToRevealAfterDialogue.Visible = true;
		GD.Print($"[DialogueTriggerInteractable] Revealed node by visibility only: '{_nodeToRevealAfterDialogue.Name}'.");
	}
}