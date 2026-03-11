namespace WhispersOfTheForest.Story;

/// <summary>
/// Stores dynamic story variables.
/// TODO: Replace static state with an instance-based story state container.
/// </summary>
public static class StoryVars
{
	/// <summary>
	/// Harmony with nature.
	/// </summary>
	public static int Harmony { get; set; }

	/// <summary>
	/// Ruthlessness / pragmatism.
	/// </summary>
	public static int Ruthlessness { get; set; }

	/// <summary>
	/// Resets all story variables for a new playthrough.
	/// </summary>
	public static void Reset()
	{
		Harmony = 0;
		Ruthlessness = 0;
	}
}