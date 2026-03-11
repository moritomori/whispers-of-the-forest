namespace WhispersOfTheForest.Story;

/// <summary>
/// Stores story decision flags.
/// TODO: Replace static state with an instance-based story state container.
/// </summary>
public static class StoryFlags
{
	public static bool HelpedSpirit { get; set; }
	public static bool IgnoredSpirit { get; set; }
	public static bool HarmedForest { get; set; }

	public static bool TalkedToSpirit { get; set; }
	public static bool TalkedToWanderer { get; set; }
	public static bool TalkedToElder { get; set; }

	/// <summary>
	/// Resets all story flags for a new playthrough.
	/// </summary>
	public static void Reset()
	{
		HelpedSpirit = false;
		IgnoredSpirit = false;
		HarmedForest = false;

		TalkedToSpirit = false;
		TalkedToWanderer = false;
		TalkedToElder = false;
	}
}