using Godot;

/// <summary>
/// Main menu controller.
/// Handles Play and Quit buttons.
/// </summary>
public partial class MenuScene : Control
{
	private Button? _playButton;
	private Button? _quitButton;

	// Path to the main game scene
	[Export] public string MainScenePath { get; set; } = "res://scenes/main/MainScene.tscn";

	public override void _Ready()
	{
		_playButton = GetNodeOrNull<Button>("PlayButton");
		_quitButton = GetNodeOrNull<Button>("QuitButton");

		if (_playButton == null || _quitButton == null)
		{
			GD.PushError("[MenuScene] PlayButton or QuitButton not found.");
			return;
		}

		_playButton.Pressed += OnPlayButtonPressed;
		_quitButton.Pressed += OnQuitButtonPressed;
	}

	private void OnPlayButtonPressed()
	{
		if (string.IsNullOrWhiteSpace(MainScenePath))
		{
			GD.PushError("[MenuScene] MainScenePath is empty.");
			return;
		}

		Error error = GetTree().ChangeSceneToFile(MainScenePath);
		if (error != Error.Ok)
		{
			GD.PushError($"[MenuScene] Failed to change scene to '{MainScenePath}'. Error: {error}");
		}
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}