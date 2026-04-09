namespace WhispersOfTheForest.World;

/// <summary>
/// Receives a location context from the root location node.
/// </summary>
public interface ILocationContextConsumer
{
	void SetLocationContext(LocationContext context);
}