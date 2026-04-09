using Godot;
using WhispersOfTheForest.Dialogue;
using WhispersOfTheForest.Story;

namespace WhispersOfTheForest.World;

/// <summary>
/// Holds explicit references to gameplay systems for a single location
/// and injects them into local scene objects.
/// </summary>
public partial class LocationContext : Node2D
{
	private DialogueController? _dialogueController;
	private StoryManager? _storyManager;
	private LocationHost? _locationHost;
	private bool _isInitialized;

	public DialogueController? DialogueController => _dialogueController;
	public StoryManager? StoryManager => _storyManager;
	public LocationHost? LocationHost => _locationHost;
	public bool IsInitialized => _isInitialized;

	public void Initialize(
		DialogueController dialogueController,
		StoryManager storyManager,
		LocationHost locationHost)
	{
		_dialogueController = dialogueController;
		_storyManager = storyManager;
		_locationHost = locationHost;
		_isInitialized = true;

		InjectContextRecursively(this);

		GD.Print($"[LocationContext] Initialized for location '{Name}'.");
	}

	public override void _Ready()
	{
		if (!_isInitialized)
		{
			GD.PushWarning(
				"[LocationContext] The location context is not initialized yet. " +
				"It must be initialized by LocationHost."
			);
		}
	}

	private void InjectContextRecursively(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is ILocationContextConsumer consumer)
			{
				consumer.SetLocationContext(this);
			}

			InjectContextRecursively(child);
		}
	}
}