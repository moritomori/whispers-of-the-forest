using Godot;

namespace WhispersOfTheForest.Story;

/// <summary>
/// Stores story decision and progress flags for the current playthrough.
/// </summary>
[GlobalClass]
public partial class StoryFlags : Resource
{
	[Export] public bool HelpedSpirit { get; set; }
	[Export] public bool IgnoredSpirit { get; set; }
	[Export] public bool HarmedForest { get; set; }

	[Export] public bool TalkedToSpirit { get; set; }
	[Export] public bool TalkedToWanderer { get; set; }
	[Export] public bool TalkedToElder { get; set; }

	/// <summary>
	/// Resets all story flags for a new playthrough.
	/// </summary>
	public void Reset()
	{
		HelpedSpirit = false;
		IgnoredSpirit = false;
		HarmedForest = false;

		TalkedToSpirit = false;
		TalkedToWanderer = false;
		TalkedToElder = false;
	}
}