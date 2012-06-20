using System;
using System.Collections.Generic;
using System.Text;
using Wof.Model.Level.Common;

namespace Wof.Model.Level.LevelTiles.Watercraft
{
	public interface ISinkComponent
	{

		void StartSinking();
		void StopSinking();
		float DoSinking(float time, float timeUnit);
		float DoSubmerge(float time, float timeUnit);
		float DoEmerge(float time, float timeUnit);
		void StartEmerging();
		void StopEmerging();
		void StartSubmerging();
		void StopSubmerging();
		float Depth { get; }
		bool IsSinking { get; }
		bool IsSunkDown { get; }
		bool IsEmerged { get; }
		bool IsSubmerged { get; }
		float SubmergeTimeElapsed { get; }
		float SinkingTimeElapsed { get; }
		bool IsSubmerging { get; }
		bool IsEmerging { get; }

        LevelTile Tile { get; }
	}
}
