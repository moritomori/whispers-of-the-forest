using Godot;
using WhispersOfTheForest.Story;

namespace WhispersOfTheForest.World;

/// <summary>
/// Gate condition that checks a single story flag.
/// </summary>
[GlobalClass]
public partial class StoryFlagGateCondition : GateCondition
{
	[Export] private StoryFlagKey _flagKey;
	[Export] private bool _requiredValue = true;

	public override bool IsSatisfied(StoryManager storyManager)
	{
		if (storyManager.StoryFlags is not StoryFlags storyFlags)
		{
			GD.PushError("[StoryFlagGateCondition] _storyFlags is not assigned in StoryManager.");
			return false;
		}

		bool currentValue = _flagKey switch
		{
			StoryFlagKey.ForestIntroObservedLantern => storyFlags.ForestIntroObservedLantern,
			StoryFlagKey.ForestIntroObservedPath => storyFlags.ForestIntroObservedPath,
			StoryFlagKey.ForestIntroObservedTree => storyFlags.ForestIntroObservedTree,
			StoryFlagKey.ForestIntroGuideSpiritRevealed => storyFlags.ForestIntroGuideSpiritRevealed,
			StoryFlagKey.ForestIntroGuideSpiritSpoken => storyFlags.ForestIntroGuideSpiritSpoken,
			StoryFlagKey.VillageEntered => storyFlags.VillageEntered,
			StoryFlagKey.VillageBoardRead => storyFlags.VillageBoardRead,
			StoryFlagKey.MetChildSpirit => storyFlags.MetChildSpirit,
			StoryFlagKey.MetWoodcutter => storyFlags.MetWoodcutter,
			StoryFlagKey.SpokeToWoodcutterFirst => storyFlags.SpokeToWoodcutterFirst,
			_ => false
		};

		return currentValue == _requiredValue;
	}
}