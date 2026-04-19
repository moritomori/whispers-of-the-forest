using Godot;
using WhispersOfTheForest.Core;

namespace WhispersOfTheForest.World;

/// <summary>
/// Changes the current scene when the player enters the transition zone.
/// </summary>
public partial class SceneTransitionZone : Area2D
{
	[Export] private string _targetScenePath = "res://scenes/locations/VillageHub.tscn";
	[Export] private bool _useDeferredChange = true;

	private bool _isTransitioning;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (_isTransitioning)
			return;

		if (body is not Player)
			return;

		if (string.IsNullOrWhiteSpace(_targetScenePath))
		{
			GD.PushError("[SceneTransitionZone] Target scene path is empty.");
			return;
		}

		_isTransitioning = true;
		GD.Print($"[SceneTransitionZone] Transitioning to '{_targetScenePath}'.");

		if (_useDeferredChange)
		{
			CallDeferred(nameof(ChangeSceneDeferred));
			return;
		}

		ChangeSceneNow();
	}

	private void ChangeSceneDeferred()
	{
		ChangeSceneNow();
	}

	private void ChangeSceneNow()
	{
		SceneTree? sceneTree = GetTree();
		if (sceneTree is null)
		{
			GD.PushError("[SceneTransitionZone] SceneTree is not available.");
			_isTransitioning = false;
			return;
		}

		Error error = sceneTree.ChangeSceneToFile(_targetScenePath);
		if (error != Error.Ok)
		{
			GD.PushError(
				$"[SceneTransitionZone] Failed to change scene to '{_targetScenePath}'. Error: {error}"
			);
			_isTransitioning = false;
		}
	}
}