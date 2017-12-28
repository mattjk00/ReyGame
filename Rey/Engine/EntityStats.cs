using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class EntityStats
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int AttackLevel { get; set; }
        public int DefenceLevel { get; set; }
        public int AttackSpeed { get; set; }
        public int MagicSpeed { get; set; }
        public bool Aggressive { get; set; }

        /// <summary>
        /// Default constructor, set to base values
        /// </summary>
        public EntityStats()
        {
            this.HP = 10;
            this.MaxHP = 10;
            this.AttackLevel = 1;
            this.DefenceLevel = 1;
            this.AttackSpeed = 25;
            this.MagicSpeed = 35;
        }
    }
}
