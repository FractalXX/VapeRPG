using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class Angel : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Angel";
            this.Description = "You deal increased damage (5%/level) while using your wing. Doesn't apply on jumps.";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<LongerFlight>();
        }

        public override void UpdateStats(VapePlayer modPlayer)
        {
            float amount = modPlayer.GetSkillLevel<Angel>() * 0.05f;
            modPlayer.player.meleeDamage += amount;
            modPlayer.player.rangedDamage += amount;
            modPlayer.player.magicDamage += amount;
            modPlayer.player.minionDamage += amount;
            modPlayer.player.thrownDamage += amount;
        }
    }
}
