using Godot;
public partial class InteractionArea : Area2D
{
    private IInteractable _currentInteractable;
    
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

	/// <summary>
	/// Called when an object enters the interaction zone
	/// </summary>
	private void OnBodyEntered(Node2D body)
    {
		// Check if this is an object that can be interacted with.
		// Search for IInteractable in the object itself or in its parent node.
		IInteractable interactable = body as IInteractable;
        if (interactable == null && body.GetParent() is IInteractable parentInteractable)
        {
            interactable = parentInteractable;
        }
        
        if (interactable != null)
        {
            _currentInteractable = interactable;
            GD.Print("[InteractionArea] You can interact! Press E");
        }
    }

	/// <summary>
	/// Called when an object leaves the interaction area
	/// </summary>
	private void OnBodyExited(Node2D body)
    {
		// Check if this is the same object that was returned
		IInteractable interactable = body as IInteractable;
        if (interactable == null && body.GetParent() is IInteractable parentInteractable)
        {
            interactable = parentInteractable;
        }
        
        if (interactable == _currentInteractable)
        {
            _currentInteractable = null;
            GD.Print("[InteractionArea] The object is too far away for interaction");
        }
    }

	/// <summary>
	/// Returns the current object for interaction
	/// </summary>
	public IInteractable GetInteractable()
    {
        return _currentInteractable;
    }
}

/// <summary>
/// Interface for all objects with which you can interact
/// </summary>
public interface IInteractable
{
	/// <summary>
	/// Called when the player interacts with an object
	/// </summary>
	void Interact(Player player);
}

