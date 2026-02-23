using Godot;
using GodotInk;
using System.Diagnostics.Tracing;
using Test.scripts.story;

/// <summary>
/// Class for NPCs (non-player characters).
/// Can engage in dialogue with the player.
/// </summary>
public partial class NPC : CharacterBody2D, IInteractable
{
    // Ім'я NPC (для відображення в діалогах)
    [Export] public InkStory Dialogue;
    //[Export] public string StartKnot = "start";
    // Посилання на систему діалогів
    private DialogueController _dialogue;    

    public override async void _Ready()
    {

        _dialogue = GetTree().GetFirstNodeInGroup("dialogue_controller") as DialogueController;
    }

	/// <summary>
	/// Implementation of the IInteractable interface
	/// Called when the player presses E near an NPC
	/// </summary>
	public void Interact(Player player)
    {
		GD.Print($"[DEBUG] Before Wanderer dialogue: {StoryManager.Instance.CurrentWorldState}");
		_dialogue?.StartDialogue(Dialogue);
    }
}



