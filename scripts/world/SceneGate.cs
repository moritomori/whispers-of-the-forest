using Godot;
using GodotInk;
using WhispersOfTheForest.Core;
using WhispersOfTheForest.Dialogue;
using WhispersOfTheForest.Story;

namespace WhispersOfTheForest.World;

/// <summary>
/// Interactable location transition gate with an optional unlock condition.
/// </summary>
public partial class SceneGate : Area2D, IInteractable, ILocationContextConsumer
{
	[Export] private InkStory? _blockedStory;
	[Export] private string _blockedKnot = "start";
	[Export] private PackedScene? _targetLocation;
	[Export] private GateCondition? _openCondition;

	private LocationContext? _locationContext;

	public void SetLocationContext(LocationContext context)
	{
		_locationContext = context;
	}

	public override void _Ready()
	{
		if (_blockedStory is null)
		{
			GD.PushWarning("[SceneGate] Blocked story is not assigned.");
		}

		if (_targetLocation is null)
		{
			GD.PushWarning("[SceneGate] Target location is not assigned.");
		}
	}

	public void Interact(Player player)
	{
		if (_locationContext?.DialogueController is not DialogueController dialogueController)
		{
			GD.PushError("[SceneGate] DialogueController is not available through LocationContext.");
			return;
		}

		if (_locationContext.StoryManager is not StoryManager storyManager)
		{
			GD.PushError("[SceneGate] StoryManager is not available through LocationContext.");
			return;
		}

		if (dialogueController.IsDialogueActive)
			return;

		if (!IsGateOpen(storyManager))
		{
			PlayBlockedDialogue(dialogueController);
			return;
		}

		OpenGate();
	}

	private bool IsGateOpen(StoryManager storyManager)
	{
		if (_openCondition is null)
			return true;

		return _openCondition.IsSatisfied(storyManager);
	}

	private void PlayBlockedDialogue(DialogueController dialogueController)
	{
		if (_blockedStory is null)
		{
			GD.PushWarning("[SceneGate] Gate is blocked, but no blocked dialogue is assigned.");
			return;
		}

		dialogueController.StartDialogue(_blockedStory, _blockedKnot);
	}

	private void OpenGate()
	{
		if (_locationContext?.LocationHost is not LocationHost locationHost)
		{
			GD.PushError("[SceneGate] LocationHost is not available through LocationContext.");
			return;
		}

		if (_targetLocation is null)
		{
			GD.PushError("[SceneGate] Cannot open gate because TargetLocation is not assigned.");
			return;
		}

		locationHost.LoadLocation(_targetLocation);
	}
}