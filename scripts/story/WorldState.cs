using System;

namespace Test.scripts.story
{ 
	/// <summary>
	/// Global state of the world (FSM main state)
	/// Глобальний стан світу (основний стан FSM)
	/// </summary>
	public enum WorldState
	{
		Neutral,            // Default state / Початковий стан
		ForestAwakening,    // Player helped the forest / Гравець допоміг лісу
		ForestAngered,      // Player harmed the forest / Гравець нашкодив лісу
		Finale				// instead of EndingReached	
	}
}

