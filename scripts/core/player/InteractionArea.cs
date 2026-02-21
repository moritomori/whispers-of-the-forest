using Godot;

/// <summary>
/// Зона взаємодії - визначає чи може гравець взаємодіяти з об'єктом.
/// Використовує Area2D для виявлення об'єктів поблизу.
/// </summary>
public partial class InteractionArea : Area2D
{
    // Посилання на об'єкт з яким можна взаємодіяти
    private IInteractable _currentInteractable;
    
    public override void _Ready()
    {
        // Підписуємося на сигнали Area2D
        // Коли об'єкт входить в зону
        BodyEntered += OnBodyEntered;
        // Коли об'єкт виходить з зони
        BodyExited += OnBodyExited;
    }
    
    /// <summary>
    /// Викликається коли об'єкт входить в зону взаємодії
    /// </summary>
    private void OnBodyEntered(Node2D body)
    {
        // Перевіряємо чи це об'єкт з яким можна взаємодіяти
        // Шукаємо IInteractable в самому об'єкті або в його батьківському вузлі
        IInteractable interactable = body as IInteractable;
        if (interactable == null && body.GetParent() is IInteractable parentInteractable)
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
    /// Викликається коли об'єкт виходить з зони взаємодії
    /// </summary>
    private void OnBodyExited(Node2D body)
    {
        // Перевіряємо чи це той самий об'єкт що вийшов
        IInteractable interactable = body as IInteractable;
        if (interactable == null && body.GetParent() is IInteractable parentInteractable)
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
    /// Повертає поточний об'єкт для взаємодії
    /// </summary>
    public IInteractable GetInteractable()
    {
        return _currentInteractable;
    }
}

/// <summary>
/// Інтерфейс для всіх об'єктів з якими можна взаємодіяти
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Викликається коли гравець взаємодіє з об'єктом
    /// </summary>
    void Interact(Player player);
}

