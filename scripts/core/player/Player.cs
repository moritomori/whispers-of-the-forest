using Godot;
using GodotInk;
/// <summary>
/// Class for controlling the main character (MC).
/// Responsible for character movement in a top-down isometric style.
/// </summary>
public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 200.0f;
    private InteractionArea _interactionArea;
    private DialogueController _dialogueSystem;

    public override void _Ready()
    {
		// _Ready() is called when a node is added to the scene tree.
		// Here we initialize references to other components.

		// Search for InteractionArea among child nodes
		_interactionArea = GetNode<InteractionArea>("InteractionArea");

		// Search for DialogueSystem in the scene tree (it will be at the scene level)
		_dialogueSystem = GetTree().GetFirstNodeInGroup("dialogue_controller") as DialogueController;
    }

    public override void _PhysicsProcess(double delta)
    {
		// _PhysicsProcess is called every frame for physics
		// delta is the time elapsed since the previous frame (usually ~0.016 for 60 FPS)

		// If the dialog is active, do not allow movement
		if (_dialogueSystem != null && _dialogueSystem.IsDialogueActive)
        {
            Velocity = Vector2.Zero;
            MoveAndSlide();
            return;
        }

		// Get the direction of movement from the keyboard
		// Input.IsActionPressed checks whether a key is pressed
		Vector2 direction = Vector2.Zero;
        
        if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("move_up"))
            direction.Y -= 1;
                    
        if (Input.IsActionPressed("ui_down") || Input.IsActionPressed("move_down"))
            direction.Y += 1;
        
		if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("move_left"))
            direction.X -= 1;

		if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("move_right"))
            direction.X += 1;

		// So that diagonal movement is not faster)
		direction = direction.Normalized();

		Velocity = direction * Speed;

		// Moves the character taking collisions into account
		MoveAndSlide();

		// Check if the E key is pressed for interaction
		if (Input.IsActionJustPressed("interact"))
        {
			//GD.Print("E");
			TryInteract();
        }
    }
    
    /// <summary>
    /// Спроба взаємодії з об'єктом поблизу
    /// </summary>
    private void TryInteract()
    {
        if (_interactionArea == null)
        {
            //GD.Print("null");
			return;
		}

		// Перевіряємо чи є об'єкт для взаємодії
		var interactable = _interactionArea.GetInteractable();
        if (interactable != null)
        {
			// Викликаємо метод взаємодії
			//GD.Print("interactable.Interact(this);");
			interactable.Interact(this);
        }
    }
}



