using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.scripts.story
{
	/// <summary>
	/// Controls story flow and world state (FSM)
	/// Керує сюжетом і станом світу (FSM)
	/// </summary>
	public partial class StoryManager : Node
	{
		public static StoryManager Instance { get; private set; }

		/// <summary>
		/// Current global world state
		/// Поточний глобальний стан світу
		/// </summary>
		public WorldState CurrentWorldState { get; private set; } = WorldState.Neutral;

		public override void _Ready()
		{
			// Singleton pattern
			GD.Print($"[StoryManager] READY instance id={GetInstanceId()} path={GetPath()}");

			Instance = this;
		}

		/// <summary>
		/// Recalculate world state based on flags
		/// Перераховує стан світу на основі прапорців
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
		/// Reset story to initial state
		/// Скидання сюжету до початкового стану
		/// </summary>
		public void ResetStory()
		{
			StoryFlags.Reset();
			CurrentWorldState = WorldState.Neutral;
		}

		/// <summary>
		/// Mark story as finished
		/// Позначити сюжет як завершений
		/// </summary>
		public void ReachEnding()
		{
			CurrentWorldState = WorldState.EndingReached;
		}
	}
}
