using Godot;
using WhispersOfTheForest.Story;

namespace WhispersOfTheForest.World;

/// <summary>
/// Base class for scene gate conditions.
/// </summary>
[GlobalClass]
public partial class GateCondition : Resource
{
	/// <summary>
	/// Returns true if the condition is satisfied.
	/// </summary>
	public virtual bool IsSatisfied(StoryManager storyManager)
	{
		return true;
	}
}