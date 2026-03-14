namespace WhispersOfTheForest.Core;

/// <summary>
/// Represents an object that can interact with the player.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Called when the player interacts with the object.
    /// </summary>
    void Interact(Player player);
}