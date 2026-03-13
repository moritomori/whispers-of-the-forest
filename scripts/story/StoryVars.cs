using Godot;

namespace WhispersOfTheForest.Story;

/// <summary>
/// Stores dynamic story variables for the current playthrough.
/// </summary>
[GlobalClass]
public partial class StoryVars : Resource
{
	/// <summary>
	/// Harmony with nature.
	/// </summary>
	[Export] public int Harmony { get; set; }

	/// <summary>
	/// Ruthlessness / pragmatism.
	/// </summary>
	[Export] public int Ruthlessness { get; set; }

	/// <summary>
	/// Resets all story variables for a new playthrough.
	/// </summary>
	public void Reset()
	{
		Harmony = 0;
		Ruthlessness = 0;
	}
}