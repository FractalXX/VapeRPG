using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Reflection : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Reflection";
            this.Description = "You reflect 10%/20% of melee damage done by your enemy.";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<Warmth>();
        }

        public override void ModifyHitByNPC(VapePlayer modPlayer, NPC npc, ref int damage, ref bool crit)
        {
            npc.StrikeNPC((int)Math.Ceiling(damage * 0.05f * modPlayer.GetSkillLevel<Reflection>()), 30, npc.direction);
        }
    }
}
