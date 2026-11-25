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
	[Export] public int Empathy { get; set; }

	/// <summary>
	/// Ruthlessness / pragmatism in difficult choices.
	/// </summary>
	[Export] public int Pragmatism { get; set; }

	/// <summary>
	/// Trust level of the child spirit toward the player.
	/// </summary>
	[Export] public int ChildTrust { get; set; }

	/// <summary>
	/// Resets all story variables for a new playthrough.
	/// </summary>
	public void Reset()
	{
        Empathy = 0;
        Pragmatism = 0;
		ChildTrust = 0;
	}
}