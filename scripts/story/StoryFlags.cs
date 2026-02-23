using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Test.scripts.story
{
	/// <summary>
	/// Stores player decisions (flags)
	/// </summary>
	public static class StoryFlags
	{
		// --- NPC decisions ---
		public static bool HelpedSpirit = false;
		public static bool IgnoredSpirit = false;
		public static bool HarmedForest = false;

		// --- Story progres
		public static bool TalkedToSpirit = false;
		public static bool TalkedToWanderer = false;
		public static bool TalkedToElder = false;

		/// <summary>
		/// Reset all flags (for new play)
		/// </summary>
		public static void Reset()
		{
			HelpedSpirit = false;
			IgnoredSpirit = false;
			HarmedForest = false;

			HelpedSpirit = false;
			IgnoredSpirit = false;
			HarmedForest = false;
		}
	}
}
