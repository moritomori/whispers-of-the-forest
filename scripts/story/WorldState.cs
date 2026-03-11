namespace WhispersOfTheForest.Story;

/// <summary>
/// Represents the main global world state used by the story system.
/// </summary>
public enum WorldState
{
	Neutral,         // Default world state
	ForestAwakening, // Player helped the forest
	ForestAngered,   // Player harmed the forest
	Finale           // Final story state
}