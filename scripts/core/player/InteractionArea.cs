using Godot;

namespace WhispersOfTheForest.Core;

/// <summary>
/// Detects nearby interactable objects inside the player's interaction zone.
/// Supports both physics bodies and areas.
/// </summary>
public partial class InteractionArea : Area2D
{
	private IInteractable? _currentInteractable;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;

		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;
	}

	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
		BodyExited -= OnBodyExited;

		AreaEntered -= OnAreaEntered;
		AreaExited -= OnAreaExited;
	}

	/// <summary>
	/// Called when a physics body enters the interaction area.
	/// </summary>
	private void OnBodyEntered(Node2D body)
	{
		TrySetCurrentInteractable(body);
	}

	/// <summary>
	/// Called when a physics body exits the interaction area.
	/// </summary>
	private void OnBodyExited(Node2D body)
	{
		TryClearCurrentInteractable(body);
	}

	/// <summary>
	/// Called when an area enters the interaction area.
	/// </summary>
	private void OnAreaEntered(Area2D area)
	{
		TrySetCurrentInteractable(area);
	}

	/// <summary>
	/// Called when an area exits the interaction area.
	/// </summary>
	private void OnAreaExited(Area2D area)
	{
		TryClearCurrentInteractable(area);
	}

	/// <summary>
	/// Returns the current object available for interaction.
	/// </summary>
	public IInteractable? GetInteractable()
	{
		return _currentInteractable;
	}

	private void TrySetCurrentInteractable(Node node)
	{
		IInteractable? interactable = ResolveInteractable(node);
		if (interactable is null)
			return;

		_currentInteractable = interactable;
		GD.Print("[InteractionArea] You can interact. Press E.");
	}

	private void TryClearCurrentInteractable(Node node)
	{
		IInteractable? interactable = ResolveInteractable(node);
		if (interactable is null)
			return;

		if (!ReferenceEquals(interactable, _currentInteractable))
			return;

		_currentInteractable = null;
		GD.Print("[InteractionArea] The object is too far away for interaction.");
	}

	private static IInteractable? ResolveInteractable(Node node)
	{
		if (node is IInteractable selfInteractable)
			return selfInteractable;

		if (node.GetParent() is IInteractable parentInteractable)
			return parentInteractable;

		return null;
	}
}