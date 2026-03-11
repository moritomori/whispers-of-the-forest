using Godot;

namespace WhispersOfTheForest.Story;

/// <summary>
/// Controls story flow, world state, and ending evaluation.
/// </summary>
public partial class StoryManager : Node
{
	// TODO: Remove singleton access and inject StoryManager dependency where needed.
	public static StoryManager? Instance { get; private set; }

	/// <summary>
	/// Current global world state.
	/// </summary>
	public WorldState CurrentWorldState { get; private set; } = WorldState.Neutral;

	/// <summary>
	/// Current evaluated ending type.
	/// </summary>
	public EndingType CurrentEndingType { get; private set; } = EndingType.None;

	public override void _Ready()
	{
		if (Instance is not null && Instance != this)
		{
			GD.PushError("[StoryManager] Another instance already exists.");
			return;
		}

		Instance = this;
		GD.Print($"[StoryManager] Ready. InstanceId={GetInstanceId()} Path={GetPath()}");
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	/// <summary>
	/// Recalculates the current world state based on story flags.
	/// </summary>
	public void RecalculateWorldState()
	{
		if (StoryFlags.HarmedForest)
		{
			CurrentWorldState = WorldState.ForestAngered;
		}
		else if (StoryFlags.HelpedSpirit)
		{
			CurrentWorldState = WorldState.ForestAwakening;
		}
		else
		{
			CurrentWorldState = WorldState.Neutral;
		}

		GD.Print($"[StoryManager] WorldState = {CurrentWorldState}");
	}

	/// <summary>
	/// Evaluates the ending type based on story variables.
	/// </summary>
	public EndingType EvaluateEndingType()
	{
		if (StoryVars.Ruthlessness >= 2)
		{
			return EndingType.Bad;
		}

		if (StoryVars.Harmony >= 2)
		{
			return EndingType.Good;
		}

		return EndingType.Neutral;
	}

	/// <summary>
	/// Enters the final story state and stores the resulting ending type.
	/// </summary>
	public void EnterFinale()
	{
		CurrentEndingType = EvaluateEndingType();
		CurrentWorldState = WorldState.Finale;

		GD.Print($"[StoryManager] Finale reached. EndingType = {CurrentEndingType}");
	}

	/// <summary>
	/// Resets story flags, variables, and cached story state.
	/// </summary>
	public void ResetStory()
	{
		StoryFlags.Reset();
		StoryVars.Reset();

		CurrentWorldState = WorldState.Neutral;
		CurrentEndingType = EndingType.None;

		GD.Print("[StoryManager] Story reset.");
	}
}