﻿using System;
using System.Collections.Generic;

namespace Heroes.ReplayParser
{
    public class Player
    {
        /// <summary>
        /// Gets or sets the Battle.NET region of a player.
        /// </summary>
        public int BattleNetRegionId { get; set; }

        /// <summary>
        /// Gets or sets the Battle.NET Sub-ID of a player, describing the URI to find the player profile. 
        /// </summary>
        public int BattleNetSubId { get; set; }

        /// <summary>
        /// Gets or sets the Battle.NET ID of a player, describing the URI to find the player profile.
        /// </summary>
        public int BattleNetId { get; set; }

        /// <summary>
        /// Gets or sets the player's color.
        /// </summary>
        public int[] Color { get; set; } = new int[0];

        /// <summary>
        /// Gets or sets the difficulty of a computer player.
        /// </summary>
        public Difficulty Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the player's handicap.
        /// </summary>
        public int Handicap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player won the game.
        /// </summary>
        public bool IsWinner { get; set; }

        /// <summary>
        /// Gets or sets the player's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the BattleTag (Numbers only)
        /// </summary>
        public int BattleTag { get; set; }

        /// <summary>
        /// Gets or sets the type of player, whether he is human or computer.
        /// </summary>
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// Gets or sets the player's team number.
        /// </summary>
        public int Team { get; set; }

        /// <summary>
        /// Gets or sets whether the player was auto select or not
        /// </summary>
        public bool IsAutoSelect { get; set; }

        /// <summary>
        /// Gets or sets the player's character.
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// Gets or sets the player's skin / skin tint.
        /// </summary>
        public string SkinAndSkinTint { get; set; } = null;

        /// <summary>
        /// Gets or sets the player's mount / mount tint.
        /// </summary>
        public string MountAndMountTint { get; set; } = null;

        /// <summary>
        /// Gets or sets the player's character level.
        /// </summary>
        public int CharacterLevel { get; set; }

        /// <summary>
        /// Gets or sets if the player has been given the silenced penalty
        /// </summary>
        public bool IsSilenced { get; set; } = false;

        /// <summary>
        /// Gets or sets the player's selected Hero talents
        /// </summary>
        public Talent[] Talents { get; set; } = new Talent[0];

        /// <summary>
        /// Gets or sets the player's in game Hero units.
        /// </summary>
        public List<Unit> HeroUnits { get; set; } = new List<Unit>();

        /// <summary>
        /// Gets or sets the player's standard end game score result statistics
        /// </summary>
        public ScoreResult ScoreResult { get; set; } = new ScoreResult();

        /// <summary>
        /// Gets or sets the player's miscellaneous end game score result statistics
        /// </summary>
        public Dictionary<string, int> MiscellaneousScoreResultEventDictionary { get; set; } = new Dictionary<string, int>();
    }

    public class ScoreResult
    {
        public int Takedowns { get; set; } = 0;
        public int SoloKills { get; set; } = 0;
        public int Assists { get; set; } = 0;
        public int Deaths { get; set; } = 0;

        public int HeroDamage { get; set; } = 0;
        public int SiegeDamage { get; set; } = 0;
        public int StructureDamage { get; set; } = 0;
        public int MinionDamage { get; set; } = 0;
        public int CreepDamage { get; set; } = 0;
        public int SummonDamage { get; set; } = 0;

        public TimeSpan? TimeCCdEnemyHeroes { get; set; } = null;

        public int? Healing { get; set; } = null;
        public int SelfHealing { get; set; } = 0;

        public int? DamageTaken { get; set; } = null;

        public int ExperienceContribution { get; set; } = 0;
        public int TownKills { get; set; } = 0;

        public TimeSpan TimeSpentDead { get; set; } = TimeSpan.Zero;

        public int MercCampCaptures { get; set; } = 0;
        public int WatchTowerCaptures { get; set; } = 0;

        public int MetaExperience { get; set; } = 0; // Exp added to the player's Account and Hero level after the match
    }

    public class Talent
    {
        public int TalentID { get; set; }
        public string TalentName { get; set; } = null;
        public TimeSpan TimeSpanSelected { get; set; }
    }

    public enum PlayerType
    {
        Human,
        Computer,
        Spectator
    }

    public enum Difficulty
    {
        Unknown,
        Beginner,
        Recruit,
        Adept,
        Veteran,
        Elite
    }
}