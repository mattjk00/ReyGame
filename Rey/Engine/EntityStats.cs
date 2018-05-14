using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class StatsBoost
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int AttackLevel { get; set; }
        public int DefenceLevel { get; set; }
        public int AttackSpeed { get; set; }
        public int MagicSpeed { get; set; }
        public bool Aggressive { get; set; }
    }

    public class EntityStats
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int AttackLevel { get; set; }
        public int DefenceLevel { get; set; }
        public int AttackSpeed { get; set; }
        public int MagicSpeed { get; set; }
        public bool Aggressive { get; set; }

        public StatsBoost Boosts { get; set; }

        // full stats, including boosts
        public EntityStats FullStats {
            get
            {
                if (Boosts != null)
                    return SumStatsC(this, Boosts);
                else
                    return this;
            }
        }

        /// <summary>
        /// Default constructor, set to base values
        /// </summary>
        public EntityStats()
        {
            this.HP = 10;
            this.MaxHP = 10;
            this.AttackLevel = 5;
            this.DefenceLevel = 1;
            this.AttackSpeed = 25;
            this.MagicSpeed = 65;
        }

        public EntityStats(int hp, int maxhp, int attacklvl, int deflvl, int attackspd, int magicspd)
        {
            this.HP = hp;
            this.MaxHP = maxhp;
            this.AttackLevel = attacklvl;
            this.DefenceLevel = deflvl;
            this.AttackSpeed = attackspd;
            this.MagicSpeed = magicspd;
        }

        public static EntityStats SumStats(EntityStats e1, EntityStats e2)
        {
            EntityStats newStats = new EntityStats();
            newStats.AttackLevel = e1.AttackLevel + e2.AttackLevel;
            newStats.AttackSpeed = e1.AttackSpeed + e2.AttackSpeed;
            newStats.DefenceLevel = e1.DefenceLevel + e2.DefenceLevel;
            newStats.MaxHP = e1.MaxHP + e2.MaxHP;
            newStats.MagicSpeed = e1.MagicSpeed + e2.MagicSpeed;
            return newStats;
        }
        public static StatsBoost SumStatsB(EntityStats e1, StatsBoost e2)
        {
            StatsBoost newStats = new StatsBoost();
            newStats.AttackLevel = e1.AttackLevel + e2.AttackLevel;
            newStats.AttackSpeed = e1.AttackSpeed + e2.AttackSpeed;
            newStats.DefenceLevel = e1.DefenceLevel + e2.DefenceLevel;
            newStats.MaxHP = e1.MaxHP + e2.MaxHP;
            newStats.MagicSpeed = e1.MagicSpeed + e2.MagicSpeed;
            return newStats;
        }
        public static EntityStats SumStatsC(EntityStats e1, StatsBoost e2)
        {
            EntityStats newStats = new EntityStats();
            newStats.AttackLevel = e1.AttackLevel + e2.AttackLevel;
            newStats.AttackSpeed = e1.AttackSpeed + e2.AttackSpeed;
            newStats.DefenceLevel = e1.DefenceLevel + e2.DefenceLevel;
            newStats.MaxHP = e1.MaxHP + e2.MaxHP;
            newStats.MagicSpeed = e1.MagicSpeed + e2.MagicSpeed;
            return newStats;
        }
    }
}
