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
	/// Зберігає рішення гравця (прапорці)
	/// </summary>
	public static class StoryFlags
	{
		// --- NPC decisions ---
		public static bool HelpedSpirit = false;     // Player helped forest spirit
		public static bool IgnoredSpirit = false;    // Player ignored spirit
		public static bool HarmedForest = false;     // Player harmed forest

		/// <summary>
		/// Reset all flags (for new playthrough)
		/// Скидання прапорців (нове проходження)
		/// </summary>
		public static void Reset()
		{
			HelpedSpirit = false;
			IgnoredSpirit = false;
			HarmedForest = false;
		}
	}
}
