using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhispersOfTheForest.scripts.story;

namespace Test.scripts.story
{
	/// <summary>
	/// Controls story flow and world state (FSM)
	/// </summary>
	public partial class StoryManager : Node
	{
		public static StoryManager Instance { get; private set; }

		/// <summary>
		/// Current global world state
		/// </summary>
		public WorldState CurrentWorldState { get; private set; } = WorldState.Neutral;

		/// <summary>
		/// Resulting ending type
		/// </summary>
		public EndingType CurrentEndingType { get; private set; } = EndingType.None;
		public override void _Ready()
		{
			GD.Print($"[StoryManager] READY instance id={GetInstanceId()} path={GetPath()}");
			Instance = this;
		}

		/// <summary>
		/// Recalculate world state based on flags
		/// </summary>
		public void RecalculateWorldState()
		{
			if (StoryFlags.HarmedForest)
				CurrentWorldState = WorldState.ForestAngered;
			else if (StoryFlags.HelpedSpirit)
				CurrentWorldState = WorldState.ForestAwakening;
			else
				CurrentWorldState = WorldState.Neutral;

			GD.Print($"[StoryManager] WorldState = {CurrentWorldState}");
		}

		/// <summary>
		/// Evaluate ending based on story flags
		/// </summary>
		public EndingType EvaluateEndingType()
		{
			// Logic:
			// HarmedForest has the highest priority (Bad)
			// HelpedSpirit without harm -> Good
			// Otherwise -> Neutral

			if (StoryVars.Ruthlessness >= 2)
				return EndingType.Bad;

			if (StoryVars.Harmony >= 2)
				return EndingType.Good;

			return EndingType.Neutral;
		}
		/// <summary>
		/// Enter final state of the story
		/// </summary>
		public void EnterFinale()
		{
			CurrentEndingType = EvaluateEndingType();
			CurrentWorldState = WorldState.Finale;

			GD.Print($"[StoryManager] Finale reached. EndingType = {CurrentEndingType}");
		}

		/// <summary>
		/// Reset story to initial state
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
}
