using Godot;
using System;

/// <summary>
/// The game's main menu screen.
/// Contains a Play button to go to the main scene.
/// </summary>
public partial class MenuScene : Control
{
    private Button _playButton;
    private Button _quitButton;

	// The way to the main stage
	[Export] public string MainScenePath { get; set; } = "res://scenes/main/MainScene.tscn";

    public override void _Ready()
    {
		// Create UI for menu
		//SetupUI();

		//Searching for buttons by name 
		_playButton = GetNode<Button>("PlayButton");
        _quitButton = GetNode<Button>("QuitButton");

		_playButton.Pressed += OnPlayButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    private void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToFile(MainScenePath);
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}

//using Godot;
//using System;

//public partial class MenuScene : Control
//{
//	private Button _playButton;
//	private Button _quitButton;

//	public override void _Ready()
//	{
//		// Отримуємо кнопки за їхніми іменами
//		_playButton = GetNode<Button>("PlayButton");
//		_quitButton = GetNode<Button>("QuitButton");

//		// Підключаємо методи до сигналів кнопок
//		_playButton.Pressed += OnPlayButtonPressed;
//		_quitButton.Pressed += OnQuitButtonPressed;
//	}

//	private void OnPlayButtonPressed()
//	{
//		GD.Print("Play button pressed!");
//		// Завантажуємо нову сцену для гри
//		// GetTree().ChangeScene("res://GameScene.tscn");
//	}

//	private void OnQuitButtonPressed()
//	{
//		GD.Print("Quit button pressed!");
//		// Завершуємо гру
//		GetTree().Quit();
//	}
//}
