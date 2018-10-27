using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class LeftoverSupply : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Leftover supply";
            this.Description = "Magic weapons have a chance to not consume mana. (4%/level)";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<Bounce>();
        }

        public override void UseItem(VapePlayer modPlayer, Item item)
        {
            if (item.magic && VapeRPG.rand.Next(0, 101) <= modPlayer.GetSkillLevel<LeftoverSupply>() * 2f)
            {
                modPlayer.player.statMana += item.mana;
            }
        }
    }
}
