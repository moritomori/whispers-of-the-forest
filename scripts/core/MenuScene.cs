using Godot;
using System;

/// <summary>
/// Сцена головного меню гри.
/// Містить кнопку Play для переходу до основної сцени.
/// </summary>
public partial class MenuScene : Control
{
    // Посилання на кнопку Play
    private Button _playButton;
    private Button _quitButton;

    // Шлях до головної сцени
    [Export] public string MainScenePath { get; set; } = "res://scenes/main/MainScene.tscn";

    public override void _Ready()
    {
        // Створюємо UI для меню
        //SetupUI();

        //шукаємо кнопки за іменами 
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
