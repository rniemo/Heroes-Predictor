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

		static void parseReplay(Replay replay, int num){

			Console.WriteLine("Replay Build: " + replay.ReplayBuild);
			Console.WriteLine("Map: " + replay.Map);
			Player[] winningPlayers = new Player [5];
			Player[] losingPlayers = new Player [5];
			int x = 0, y = 0;
			foreach (var player in replay.Players.OrderByDescending(i => i.IsWinner)) {
				if (player.IsWinner) {
					winningPlayers[x++] = player;
				} else {
					losingPlayers[y++] = player;
				}
			}

			String dir = Directory.GetCurrentDirectory ();
			System.IO.StreamWriter writer = new System.IO.StreamWriter (dir + "/game" + num + ".txt");
			for(int time = 30; time < replay.ReplayLength.TotalSeconds; time += 10){
				double[] v1 = getVector (replay, winningPlayers, winningPlayers[0].Team, time);
				double[] v2 = getVector (replay, losingPlayers, losingPlayers[0].Team, time);
				v1 [6] = v1 [1] - v2 [1];
				v2 [6] = v2 [1] - v1 [1];
				v1 [7] = v1 [5] - v2 [5];
				v2 [7] = v2 [5] - v1 [5];
				writeVector (writer, v1, time, 1);
				writeVector (writer, v2, time, 0);
			}
			writer.Close ();
			Console.WriteLine ("Finished parsing replay");
		}

		static void writeVector(System.IO.StreamWriter writer, double[] vector, int time, int won){
			writer.Write (won + " ");
			writer.Write (time + " ");
			foreach (var v in vector) {
				writer.Write (v);
				writer.Write (" ");
			}
			writer.Write ("\n");

		}

		static double[] getVector(Replay replay, Player[] players, int team, int time){
			int mapWidth = replay.MapSize.X;
			// avg char level, team level, minion x position, num keeps, 
			double[] vector = new double[8];
			// calculat average character level
			vector[0] = players.Average (player => player.CharacterLevel);
			Console.WriteLine ("Avg character level: " + vector [0]);
			// calculate team level for this time
			foreach (var tl in replay.TeamLevels) {
				Console.WriteLine (tl);
				foreach (var val in tl.Values) {
					Console.WriteLine (val);
				}
			}
			Console.WriteLine(team);

			foreach (var teamLevels in replay.TeamLevels [team]) {
				if (time >= teamLevels.Value.TotalSeconds) {
					vector [1] = teamLevels.Key;
				} else {
					break;
				}
			}
			Console.WriteLine ("Avg team level: " + vector [1]);
			// calculate minion x position
			int totalx = 0;
			int numMinions = 0;
			int bornX = 0;
			foreach (var unit in replay.Units) {
				if (unit.Group == Unit.UnitGroup.Minions && unit.Team == team) {
					bornX = unit.PointBorn.X;
					foreach (var pos in unit.Positions) {
						if (Math.Abs(pos.TimeSpan.TotalSeconds - time) < 10) {
							totalx += pos.Point.X;
							numMinions++;
							break;
						}
					}
				}
			}
			double avgX = (double)totalx / numMinions;
			// if the minions are born on the high side of the map, "flip" the map
			if (bornX > mapWidth / 2) {
				avgX = mapWidth - avgX;
			}
			vector [2] = avgX / mapWidth;
			Console.WriteLine ("Avg Minion X position:" + vector [2]);
			// calculate hero x position
			totalx = 0;
			int numUnits = 0;
			Point centroid = new Point ();
			foreach (var player in players) {
				foreach (var unit in player.HeroUnits) {
					bornX = unit.PointBorn.X;
					// make sure this unit is alive
					if (time > unit.TimeSpanBorn.TotalSeconds && (!unit.TimeSpanDied.HasValue || time < unit.TimeSpanDied.Value.TotalSeconds)){
						// find the closest position
						foreach (var position in unit.Positions) {
							if (Math.Abs (position.TimeSpan.TotalSeconds - time) < 10) {
								totalx += position.Point.X;
								numUnits++;
								centroid.X += position.Point.X;
								centroid.Y += position.Point.Y;
								break;
							}
						}
					}
				}
			}
			avgX = (double)totalx / numUnits;
			if (bornX > mapWidth / 2) {
				avgX = mapWidth - avgX;
			}
			vector [3] = avgX / mapWidth;
			Console.WriteLine ("Avg Hero X position:" + vector [3]);
			// calculate avg dist to centroid
			double centroidX =  (double) centroid.X / numUnits;
			double centroidY =  (double) centroid.Y / numUnits;
			double sumDists = 0;
			foreach (var player in players) {
				foreach (var unit in player.HeroUnits) {
					if (time > unit.TimeSpanBorn.TotalSeconds && (!unit.TimeSpanDied.HasValue || time < unit.TimeSpanDied.Value.TotalSeconds)){
						// find the closest position
						foreach (var position in unit.Positions) {
							if (Math.Abs (position.TimeSpan.TotalSeconds - time) < 10) {
								sumDists += Math.Sqrt (Math.Pow (position.Point.X - centroidX, 2) + Math.Pow (position.Point.Y - centroidY, 2));
								break;
							}
						}
					}
				}
			}
			vector [4] = sumDists / numUnits;
			Console.WriteLine ("Avg hero distance to centroid: " + vector [4]);
			// calculate num keeps
			int numKeeps = 0;
			foreach (var unit in replay.Units) {
				if (unit.Group == Unit.UnitGroup.Structures && unit.Team == team && unit.Name.Equals("TownTownHallL3")) {
					// if the keep never died, or it died after the current time, then it's alive.
					if (unit.TimeSpanDied == null || unit.TimeSpanDied.Value.TotalSeconds > time) {
						numKeeps++;
					}
				}
			}
			vector [5] = numKeeps;
			Console.WriteLine ("Num keeps alive: " + vector [5]);
			return vector;
		}

		static void Main(string[] args)
		{
			var heroesAccountsFolder = Path.Combine("", @"/Users/Rniemo/Library/Application Support/Blizzard/Heroes of the Storm/Accounts/83034335/1-Hero-1-6078225/Replays");
			var randomReplayFileName = Directory.GetFiles(heroesAccountsFolder, "*.StormReplay", SearchOption.AllDirectories).OrderBy(i => Guid.NewGuid()).First();
			int num = 0;
			foreach (var filename in Directory.GetFiles(heroesAccountsFolder, "*.StormReplay", SearchOption.AllDirectories)) {
				Console.WriteLine ("Parsing file: " + filename);
				var tmpPath = Path.GetTempFileName();
				File.Copy(filename, tmpPath, overwrite: true);
				try
				{
					// Attempt to parse the replay
					// Ignore errors can be set to true if you want to attempt to parse currently unsupported replays, such as 'VS AI' or 'PTR Region' replays
					var replayParseResult = DataParser.ParseReplay(tmpPath, ignoreErrors: false, deleteFile: false);

					// If successful, the Replay object now has all currently available information
					if (replayParseResult.Item1 == DataParser.ReplayParseResult.Success)
					{
						var replay = replayParseResult.Item2;
						parseReplay(replay, num);
						num++;

					}
					else
						Console.WriteLine("Failed to Parse Replay: " + replayParseResult.Item1);

				}
				catch(Exception e){
					Console.WriteLine (e.StackTrace);
					Console.WriteLine ("exception");
				}
				finally
				{
					if (File.Exists (tmpPath)) {
						File.Delete (tmpPath);
					}
				}
			}
			//randomReplayFileName = "/Users/Rniemo/Library/Application Support/Blizzard/Heroes of the Storm/Accounts/83034335/1-Hero-1-6078225/Replays/Multiplayer/Blackheart's Bay (36).StormReplay";
			// Use temp directory for MpqLib directory permissions requirements

		}

	}
}
