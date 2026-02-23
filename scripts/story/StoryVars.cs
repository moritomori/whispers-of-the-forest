using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhispersOfTheForest.scripts.story
{
	/// <summary>
	/// Stores dynamic story variables
	/// </summary>
	public static class StoryVars
	{
		/// <summary>
		/// Harmony with nature
		/// </summary>
		public static int Harmony = 0;

		/// <summary>
		/// Ruthlessness / pragmatism
		/// </summary>
		public static int Ruthlessness = 0;

		public static void Reset()
		{
			Harmony = 0;
			Ruthlessness = 0;
		}
	}
}
