﻿namespace Heroes.ReplayParser
{
    using System;
    using System.Collections.Generic;

    public class Replay
    {
        /// <summary> Gets a list of all chat messages which took place during the game. </summary>
        public List<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        /// <summary> Gets the speed the game was played at. </summary>
        public GameSpeed GameSpeed { get; set; }

        /// <summary> Gets the type of game this replay covers, whether it was a private or open match. </summary>
        public GameMode GameMode { get; set; }

        /// <summary> Gets the map the game was played on. </summary>
        public string Map { get; set; }

        /// <summary> Gets the size of the map the game was played on. </summary>
        public Point MapSize { get; set; }

        /// <summary> Gets the details of all players in the replay. </summary>
        public Player[] Players { get; set; }

        /// <summary> Gets the build number of the Heroes version used in creating the replay. </summary>
        public int ReplayBuild { get; set; }

        /// <summary> Gets the version number of the replay. </summary>
        public string ReplayVersion { get; set; }

        /// <summary> Gets the team size of the selected gametype. </summary>
        public string TeamSize { get; set; }

        /// <summary> Gets the Time at which the game took place. </summary>
        public DateTime Timestamp { get; set; }

        /// <summary> Gets the list of all clients connected to the game. </summary>
        public Player[] ClientList { get; set; } = new Player[0x10];

        /// <summary> Gets the game events. </summary>
        public List<GameEvent> GameEvents { get; set; } = new List<GameEvent>();

        /// <summary> Gets the tracker events. </summary>
        public List<TrackerEvent> TrackerEvents { get; set; }

        /// <summary> Gets a list of units. </summary>
        public List<Unit> Units { get; set; } = new List<Unit>();

        /// <summary> Gets the number of frames in this replay. </summary>
        public int Frames { get; set; }

        /// <summary> Gets the length of this replay as a timespan. </summary>
        public TimeSpan ReplayLength { get { return new TimeSpan(0, 0, (int)(Frames / 16.0)); } }

        /// <summary> Gets a single random value from replay.init.data; currently using as part of replay hash for deduplication. </summary>
        public uint RandomValue { get; set; }

        /// <summary> Team Levels ([Team][Level] = TimeSpan) </summary>
        public Dictionary<int, TimeSpan>[] TeamLevels { get; set; } = new Dictionary<int, TimeSpan>[2];

        /// <summary> Periodic XP Breakdown ([Team][PeriodicXPBreakdown]) </summary>
        public List<PeriodicXPBreakdown>[] TeamPeriodicXPBreakdown { get; set; } = new List<PeriodicXPBreakdown>[2];

        public bool IsGameEventsParsedSuccessfully { get; set; } = false;
        public bool IsStatisticsParsedSuccessfully { get; set; } = false;
    }

    public class PeriodicXPBreakdown
    {
        public int TeamLevel { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public int MinionXP { get; set; }
        public int CreepXP { get; set; }
        public int StructureXP { get; set; }
        public int HeroXP { get; set; }
        public int TrickleXP { get; set; }
    }

    public enum GameMode
    {
        Unknown = -9,
        Custom = -1,
        TryMe = 0,
        Practice = 1,
        Cooperative = 2,
        QuickMatch = 3,
        HeroLeague = 4,
        TeamLeague = 5
    }

    public enum GameSpeed
    {
        Unknown = 0,
        Slower = 1,
        Slow,
        Normal,
        Fast,
        Faster
    }
}
