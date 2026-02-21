using Godot;
using GodotInk;
/// <summary>
/// Клас для управління головним героєм (ГГ).
/// Відповідає за рух персонажа в top-down ізометричному стилі.
/// </summary>
public partial class Player : CharacterBody2D
{
    // Швидкість руху персонажа (пікселів за секунду)
    [Export] public float Speed { get; set; } = 200.0f;
    
    // Посилання на зону взаємодії (для перевірки чи можемо взаємодіяти)
    private InteractionArea _interactionArea;
    
    // Посилання на систему діалогів (для відображення діалогів)
    private DialogueController _dialogueSystem;

    public override void _Ready()
    {
        // _Ready() викликається коли нода додається до дерева сцени
        // Тут ми ініціалізуємо посилання на інші компоненти
        
        // Шукаємо InteractionArea серед дочірніх нод
        _interactionArea = GetNode<InteractionArea>("InteractionArea");
        
        // Шукаємо DialogueSystem в дереві сцени (він буде на рівні сцени)
        _dialogueSystem = GetTree().GetFirstNodeInGroup("dialogue_controller") as DialogueController;
    }

    public override void _PhysicsProcess(double delta)
    {
        // _PhysicsProcess викликається кожен кадр для фізики
        // delta - це час що пройшов з попереднього кадру (зазвичай ~0.016 для 60 FPS)
        
        // Якщо діалог активний, не дозволяємо рух
        if (_dialogueSystem != null && _dialogueSystem.IsDialogueActive)
        {
            Velocity = Vector2.Zero;
            MoveAndSlide();
            return;
        }
        
        // Отримуємо напрямок руху з клавіатури
        // Input.IsActionPressed перевіряє чи натиснута клавіша
        Vector2 direction = Vector2.Zero;
        
        if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("move_up"))
            direction.Y -= 1;
                    
        if (Input.IsActionPressed("ui_down") || Input.IsActionPressed("move_down"))
            direction.Y += 1;
        
		if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("move_left"))
            direction.X -= 1;

		if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("move_right"))
            direction.X += 1;

		// Нормалізуємо вектор напрямку (щоб діагональний рух не був швидшим)
		direction = direction.Normalized();
        
        // Встановлюємо швидкість (velocity)
        Velocity = direction * Speed;
        
        // MoveAndSlide() рухає персонажа з урахуванням колізій
        MoveAndSlide();
        
        // Перевіряємо чи натиснута клавіша E для взаємодії
        if (Input.IsActionJustPressed("interact"))
        {
			//GD.Print("E");
			TryInteract();
        }
    }
    
    /// <summary>
    /// Спроба взаємодії з об'єктом поблизу
    /// </summary>
    private void TryInteract()
    {
        if (_interactionArea == null)
        {
            //GD.Print("null");
			return;
		}

		// Перевіряємо чи є об'єкт для взаємодії
		var interactable = _interactionArea.GetInteractable();
        if (interactable != null)
        {
			// Викликаємо метод взаємодії
			//GD.Print("interactable.Interact(this);");
			interactable.Interact(this);
        }
    }
}



