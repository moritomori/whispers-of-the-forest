using Godot;

namespace WhispersOfTheForest.Story;

/// <summary>
/// Controls story flow, world state, and ending evaluation.
/// </summary>
public partial class StoryManager : Node
{
	[Export] private StoryFlags? _storyFlags;
	[Export] private StoryVars? _storyVars;

	public StoryFlags? StoryFlags => _storyFlags;
	public StoryVars? StoryVars => _storyVars;

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
		if (_storyFlags is null || _storyVars is null)
		{
			GD.PushError("[StoryManager] _storyFlags or _storyVars is not assigned.");
			return;
		}

		GD.Print($"[StoryManager] Ready. InstanceId={GetInstanceId()} Path={GetPath()}");
	}

	/// <summary>
	/// Recalculates the current world state based on story flags.
	/// </summary>
	public void RecalculateWorldState()
	{
		if (_storyFlags is null)
		{
			GD.PushError("[StoryManager] Cannot recalculate world state because _storyFlags is not assigned.");
			return;
		}

		if (_storyFlags.HarmedForest)
		{
			CurrentWorldState = WorldState.ForestAngered;
		}
		else if (_storyFlags.HelpedSpirit)
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
		if (_storyVars is null)
		{
			GD.PushError("[StoryManager] Cannot evaluate ending because _storyVars is not assigned.");
			return EndingType.None;
		}

		if (_storyVars.Ruthlessness >= 2)
		{
			return EndingType.Bad;
		}

		if (_storyVars.Harmony >= 2)
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
		if (_storyFlags is null || _storyVars is null)
		{
			GD.PushError("[StoryManager] Cannot reset story because _storyFlags or _storyVars is not assigned.");
			return;
		}

		_storyFlags.Reset();
		_storyVars.Reset();

		CurrentWorldState = WorldState.Neutral;
		CurrentEndingType = EndingType.None;

		GD.Print("[StoryManager] Story reset.");
	}
}