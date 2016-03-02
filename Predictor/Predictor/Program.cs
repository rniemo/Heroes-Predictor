using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Heroes.ReplayParser;
using Foole.Mpq;

namespace Predictor
{
	class Program
	{
		static void Main(string[] args)
		{
			var heroesAccountsFolder = Path.Combine("", @"/Users/Rniemo/Library/Application Support/Blizzard/Heroes of the Storm/Accounts/83034335/1-Hero-1-6078225/Replays");
			var randomReplayFileName = Directory.GetFiles(heroesAccountsFolder, "*.StormReplay", SearchOption.AllDirectories).OrderBy(i => Guid.NewGuid()).First();

			// Use temp directory for MpqLib directory permissions requirements
			var tmpPath = Path.GetTempFileName();
			File.Copy(randomReplayFileName, tmpPath, overwrite: true);

			try
			{
				// Attempt to parse the replay
				// Ignore errors can be set to true if you want to attempt to parse currently unsupported replays, such as 'VS AI' or 'PTR Region' replays
				var replayParseResult = DataParser.ParseReplay(tmpPath, ignoreErrors: false, deleteFile: false);

				// If successful, the Replay object now has all currently available information
				if (replayParseResult.Item1 == DataParser.ReplayParseResult.Success)
				{
					var replay = replayParseResult.Item2;

					Console.WriteLine("Replay Build: " + replay.ReplayBuild);
					Console.WriteLine("Map: " + replay.Map);
					foreach (var player in replay.Players.OrderByDescending(i => i.IsWinner))
						Console.WriteLine("Player: " + player.Name + ", Win: " + player.IsWinner + ", Hero: " + player.Character + ", Lvl: " + player.CharacterLevel + ", Talents: " + string.Join(",", player.Talents.Select(i => i.TalentID + ":" + i.TalentName)));

					Player me = null;
					foreach (var player in replay.Players)
						if (player.Name == "rniemo")
							me = player;

					foreach (var unit in me.HeroUnits){
						foreach (var pos in unit.Positions)
							Console.WriteLine(pos.Point.X + " " + pos.Point.Y + " " + pos.TimeSpan);
						Console.WriteLine("unit has died");
					}

					Console.WriteLine(me.ScoreResult.Deaths);


					Console.WriteLine("Press Any Key to Close");
				}
				else
					Console.WriteLine("Failed to Parse Replay: " + replayParseResult.Item1);

				Console.Read();
			}
			finally
			{
				if (File.Exists(tmpPath))
					File.Delete(tmpPath);
			}
		}

	}
}
