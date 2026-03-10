using Godot;

namespace WhispersOfTheForest.Core;

/// <summary>
/// Detects nearby interactable objects inside the player's interaction zone.
/// </summary>
public partial class InteractionArea : Area2D
{
    private IInteractable? _currentInteractable;
    
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

	public override void _Notification(int what)
	{
		base._Notification(what);

		if (what == NotificationPredelete)
		{
			BodyEntered -= OnBodyEntered;
			BodyExited -= OnBodyExited;
		}
	}

	/// <summary>
	/// Called when an object enters the interaction area
	/// </summary>
	private void OnBodyEntered(Node2D body)
    {
		IInteractable? interactable = null;

		if (body is IInteractable selfInteractable)
		{
			interactable = selfInteractable;
		}
		else if (body.GetParent() is IInteractable parentInteractable)
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
		IInteractable? interactable = null;

		if (body is IInteractable selfInteractable)
		{
			interactable = selfInteractable;
		}
		else if (body.GetParent() is IInteractable parentInteractable)
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
	public IInteractable? GetInteractable()
    {
        return _currentInteractable;
    }
}