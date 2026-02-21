using Godot;
using GodotInk;
using System.Diagnostics.Tracing;
using Test.scripts.story;

/// <summary>
/// Клас для NPC (неігрових персонажів).
/// Може вести діалоги з гравцем.
/// </summary>
public partial class NPC : CharacterBody2D, IInteractable
{
    // Ім'я NPC (для відображення в діалогах)
    [Export] public InkStory Dialogue;
    //[Export] public string StartKnot = "start";
    // Посилання на систему діалогів
    private DialogueController _dialogue;    
    // Діалог який буде показано при взаємодії
    // Примітка: Godot не може експортувати звичайні C# класи, тому створюємо діалог в коді
    //private DialogueData _dialogue;

    public override async void _Ready()
    {

        _dialogue = GetTree().GetFirstNodeInGroup("dialogue_controller") as DialogueController;
    }
    
    /// <summary>
    /// Реалізація інтерфейсу IInteractable
    /// Викликається коли гравець натискає E поблизу NPC
    /// </summary>
    public void Interact(Player player)
    {
		GD.Print($"[DEBUG] Before Wanderer dialogue: {StoryManager.Instance.CurrentWorldState}");
		_dialogue?.StartDialogue(Dialogue);
    }
}



