using Godot;
using WhispersOfTheForest.Dialogue;
using WhispersOfTheForest.World;

namespace WhispersOfTheForest.Core;

/// <summary>
/// Controls the main player character.
/// Handles movement, interaction, and movement blocking during dialogue.
/// </summary>
public partial class Player : CharacterBody2D
{
	[Export] private float Speed { get; set; } = 100.0f;
	[Export] private string _cameraTopLeftMarkerName = "CameraTopLeft";
	[Export] private string _cameraBottomRightMarkerName = "CameraBottomRight";

	private InteractionArea? _interactionArea;
	private DialogueController? _dialogueSystem;
	private AnimatedSprite2D? _animatedSprite;
	private Camera2D? _camera;
	private LocationHost? _locationHost;

	private Vector2 _lastDirection = Vector2.Down;

	public override void _Ready()
	{
		_interactionArea = GetNodeOrNull<InteractionArea>("InteractionArea");
		if (_interactionArea is null)
		{
			GD.PushError("[Player] InteractionArea not found.");
		}

		_animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		if (_animatedSprite is null)
		{
			GD.PushError("[Player] AnimatedSprite2D not found.");
		}

		_camera = GetNodeOrNull<Camera2D>("Camera2D");
		if (_camera is null)
		{
			GD.PushWarning("[Player] Camera2D not found. Camera limits will not be applied.");
		}

		if (GetTree().GetFirstNodeInGroup("dialogue_controller") is DialogueController dialogueController)
		{
			_dialogueSystem = dialogueController;
		}
		else
		{
			GD.PushWarning("[Player] DialogueController not found in group 'dialogue_controller'.");
		}

		if (GetTree().GetFirstNodeInGroup("location_host") is LocationHost locationHost)
		{
			_locationHost = locationHost;
			_locationHost.LocationLoaded += OnLocationLoaded;
		}
		else
		{
			GD.PushWarning("[Player] LocationHost not found in group 'location_host'.");
		}
	}

	public override void _ExitTree()
	{
		if (_locationHost is not null)
		{
			_locationHost.LocationLoaded -= OnLocationLoaded;
		}
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (_dialogueSystem is not null && _dialogueSystem.IsDialogueActive)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			UpdateAnimation(Vector2.Zero);
			return;
		}

		Vector2 direction = Vector2.Zero;

		if (Input.IsActionPressed("move_up"))
			direction.Y -= 1;

		if (Input.IsActionPressed("move_down"))
			direction.Y += 1;

		if (Input.IsActionPressed("move_left"))
			direction.X -= 1;

		if (Input.IsActionPressed("move_right"))
			direction.X += 1;

		direction = direction.Normalized();

		if (direction != Vector2.Zero)
			_lastDirection = direction;

		Velocity = direction * Speed;

		MoveAndSlide();
		UpdateAnimation(direction);

		if (Input.IsActionJustPressed("interact"))
		{
			TryInteract();
		}
	}

	private void OnLocationLoaded(Node locationRoot)
	{
		ApplyCameraLimitsForLocation(locationRoot);

		if (locationRoot.FindChild("PlayerSpawn", recursive: true, owned: false) is Marker2D playerSpawn)
		{
			GlobalPosition = playerSpawn.GlobalPosition;
			GD.Print($"[Player] Moved to PlayerSpawn in location '{locationRoot.Name}'.");
		}
		else
		{
			GD.PushWarning($"[Player] PlayerSpawn was not found in location '{locationRoot.Name}'.");
		}
	}

	private void UpdateAnimation(Vector2 direction)
	{
		if (_animatedSprite is null)
			return;

		string animationName = direction == Vector2.Zero
			? "idle_" + GetDirectionSuffix(_lastDirection)
			: "walk_" + GetDirectionSuffix(direction);

		if (_animatedSprite.Animation != animationName)
		{
			_animatedSprite.Play(animationName);
		}
	}

	private string GetDirectionSuffix(Vector2 direction)
	{
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			return direction.X > 0 ? "right" : "left";
		}

		return direction.Y > 0 ? "down" : "up";
	}

	private void ApplyCameraLimitsForLocation(Node locationRoot)
	{
		if (_camera is null)
			return;

		if (locationRoot.FindChild(_cameraTopLeftMarkerName, recursive: true, owned: false) is not Marker2D topLeftMarker)
		{
			GD.PushWarning(
				$"[Player] Camera marker '{_cameraTopLeftMarkerName}' was not found in location '{locationRoot.Name}'."
			);
			return;
		}

		if (locationRoot.FindChild(_cameraBottomRightMarkerName, recursive: true, owned: false) is not Marker2D bottomRightMarker)
		{
			GD.PushWarning(
				$"[Player] Camera marker '{_cameraBottomRightMarkerName}' was not found in location '{locationRoot.Name}'."
			);
			return;
		}

		int left = Mathf.RoundToInt(Mathf.Min(topLeftMarker.GlobalPosition.X, bottomRightMarker.GlobalPosition.X));
		int top = Mathf.RoundToInt(Mathf.Min(topLeftMarker.GlobalPosition.Y, bottomRightMarker.GlobalPosition.Y));
		int right = Mathf.RoundToInt(Mathf.Max(topLeftMarker.GlobalPosition.X, bottomRightMarker.GlobalPosition.X));
		int bottom = Mathf.RoundToInt(Mathf.Max(topLeftMarker.GlobalPosition.Y, bottomRightMarker.GlobalPosition.Y));

		_camera.LimitLeft = left;
		_camera.LimitTop = top;
		_camera.LimitRight = right;
		_camera.LimitBottom = bottom;

		GD.Print(
			$"[Player] Camera limits applied for location '{locationRoot.Name}': " +
			$"Left={left}, Top={top}, Right={right}, Bottom={bottom}"
		);
	}

	/// <summary>
	/// Attempts to interact with a nearby interactable object.
	/// </summary>
	private void TryInteract()
	{
		if (_interactionArea is null)
		{
			GD.PushError("[Player] Cannot interact because InteractionArea is not assigned.");
			return;
		}

		IInteractable? interactable = _interactionArea.GetInteractable();
		if (interactable is not null)
		{
			interactable.Interact(this);
		}
	}
}