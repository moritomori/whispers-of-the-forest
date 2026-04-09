using Godot;

namespace WhispersOfTheForest.Story;

/// <summary>
/// Stores story decision and progress flags for the current playthrough.
/// </summary>
[GlobalClass]
public partial class StoryFlags : Resource
{
	// Forest intro
	[Export] public bool ForestIntroObservedLantern { get; set; }
	[Export] public bool ForestIntroObservedPath { get; set; }
	[Export] public bool ForestIntroObservedTree { get; set; }
	[Export] public bool ForestIntroGuideSpiritRevealed { get; set; }
	[Export] public bool ForestIntroGuideSpiritSpoken { get; set; }

	// Village hub
	[Export] public bool VillageEntered { get; set; }
	[Export] public bool VillageBoardRead { get; set; }
	[Export] public bool MetChildSpirit { get; set; }
	[Export] public bool MetWoodcutter { get; set; }
	[Export] public bool SpokeToWoodcutterFirst { get; set; }

	// Route progress
	[Export] public bool VisitedLake { get; set; }
	[Export] public bool SawPastReflection { get; set; }
	[Export] public bool VisitedWoodcutterCamp { get; set; }

	// Legacy / broad moral decisions
	[Export] public bool HelpedSpirit { get; set; }
	[Export] public bool IgnoredSpirit { get; set; }
	[Export] public bool HarmedForest { get; set; }

	/// <summary>
	/// Resets all story flags for a new playthrough.
	/// </summary>
	public void Reset()
	{
		ForestIntroObservedLantern = false;
		ForestIntroObservedPath = false;
		ForestIntroObservedTree = false;
		ForestIntroGuideSpiritRevealed = false;
		ForestIntroGuideSpiritSpoken = false;

		VillageEntered = false;
		VillageBoardRead = false;
		MetChildSpirit = false;
		MetWoodcutter = false;
		SpokeToWoodcutterFirst = false;

		VisitedLake = false;
		SawPastReflection = false;
		VisitedWoodcutterCamp = false;

		HelpedSpirit = false;
		IgnoredSpirit = false;
		HarmedForest = false;
	}
}