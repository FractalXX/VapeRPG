using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class HardenedSkin : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Hardened skin";
            this.Description = "Melee hits deal 5%/10% less damage to you.";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<VitalSupplies>();
            this.AddPrerequisite<Strengthen>();
        }

        public override void ModifyHitByNPC(VapePlayer modPlayer, NPC npc, ref int damage, ref bool crit)
        {
            damage -= (int)Math.Ceiling(damage * 0.05f * modPlayer.GetSkillLevel<HardenedSkin>());
        }
    }
}
