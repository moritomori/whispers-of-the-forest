using Godot;
using WhispersOfTheForest.Dialogue;
using WhispersOfTheForest.Story;

namespace WhispersOfTheForest.World;

/// <summary>
/// Loads and swaps gameplay locations inside a persistent main scene.
/// </summary>
public partial class LocationHost : Node
{
	[Export] private Node? _locationRoot;
	[Export] private DialogueController? _dialogueController;
	[Export] private StoryManager? _storyManager;
	[Export] private PackedScene? _startingLocation;

	private Node? _currentLocationInstance;

	[Signal]
	public delegate void LocationLoadedEventHandler(Node locationRoot);
	public override void _EnterTree()
	{
		AddToGroup("location_host");
	}

	public override void _Ready()
	{
		if (_locationRoot is null)
		{
			GD.PushError("[LocationHost] LocationRoot is not assigned.");
			return;
		}

		if (_dialogueController is null)
		{
			GD.PushError("[LocationHost] DialogueController is not assigned.");
			return;
		}

		if (_storyManager is null)
		{
			GD.PushError("[LocationHost] StoryManager is not assigned.");
			return;
		}

		if (_startingLocation is not null)
		{
			LoadLocation(_startingLocation);
		}
	}

	public void LoadLocation(PackedScene locationScene)
	{
		if (_locationRoot is null)
		{
			GD.PushError("[LocationHost] Cannot load location because LocationRoot is not assigned.");
			return;
		}

		if (_dialogueController is null || _storyManager is null)
		{
			GD.PushError("[LocationHost] Cannot load location because required systems are not assigned.");
			return;
		}

		UnloadCurrentLocation();

		Node locationInstance = locationScene.Instantiate();
		_locationRoot.AddChild(locationInstance);
		_currentLocationInstance = locationInstance;

		if (locationInstance is not LocationContext locationContext)
		{
			GD.PushError(
				$"[LocationHost] Loaded scene '{locationScene.ResourcePath}' does not have LocationContext on the root node."
			);
			return;
		}

		locationContext.Initialize(_dialogueController, _storyManager, this);

		GD.Print($"[LocationHost] Loaded location '{locationScene.ResourcePath}'.");

		EmitSignal(SignalName.LocationLoaded, locationInstance);
	}

	private void UnloadCurrentLocation()
	{
		if (_currentLocationInstance is null)
			return;

		_currentLocationInstance.QueueFree();
		_currentLocationInstance = null;
	}
}