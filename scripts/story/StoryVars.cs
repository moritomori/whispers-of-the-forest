using Godot;

namespace WhispersOfTheForest.Story;

/// <summary>
/// Stores dynamic story variables for the current playthrough.
/// </summary>
[GlobalClass]
public partial class StoryVars : Resource
{
	/// <summary>
	/// Harmony with nature, spirits, and empathy-driven choices.
	/// </summary>
	[Export] public int Harmony { get; set; }

	/// <summary>
	/// Ruthlessness / pragmatism in difficult choices.
	/// </summary>
	[Export] public int Ruthlessness { get; set; }

	/// <summary>
	/// Trust level of the child spirit toward the player.
	/// </summary>
	[Export] public int ChildTrust { get; set; }

	/// <summary>
	/// Resets all story variables for a new playthrough.
	/// </summary>
	public void Reset()
	{
		Harmony = 0;
		Ruthlessness = 0;
		ChildTrust = 0;
	}
}