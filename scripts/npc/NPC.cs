using Godot;
using GodotInk;
using WhispersOfTheForest.Core;
using WhispersOfTheForest.Dialogue;

namespace WhispersOfTheForest.Npc;

/// <summary>
/// Class for NPCs (non-player characters).
/// Can engage in dialogue with the player.
/// </summary>
public partial class NPC : CharacterBody2D, IInteractable
{
    [Export] private InkStory? _dialogueStory;
    
    private DialogueController? _dialogueController;    

    public override void _Ready()
    {
		if (GetTree() is SceneTree sceneTree
			&& sceneTree.GetFirstNodeInGroup("dialogue_controller") is DialogueController dialogueController)
		{
			_dialogueController = dialogueController;
		}
		else
		{
			GD.PushWarning("[NPC] DialogueController not found in group 'dialogue_controller'.");
		}
	}

	/// <summary>
	/// Called when the player interacts with this NPC.
	/// </summary>
	public void Interact(Player player)
	{
		if (_dialogueController is null)
		{
			GD.PushError("[NPC] Cannot start dialogue because DialogueController is not assigned.");
			return;
		}

		if (_dialogueController is null)
		{
			GD.PushError("[NPC] Cannot start dialogue because dialogue story is not assigned.");
			return;
		}

		_dialogueController.StartDialogue(_dialogueStory);
	}
}



