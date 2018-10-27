using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class Warmth : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Warmth";
            this.MaxLevel = 2;
            this.Description = "Increases your HP/MP regen by 10%/level.";
            this.Tree = SkillTree.Power;
        }

        public override void UpdateStats(VapePlayer modPlayer)
        {
            modPlayer.player.lifeRegen += (int)Math.Ceiling(modPlayer.player.lifeRegen * 0.1f * modPlayer.GetSkillLevel<Warmth>());
            modPlayer.player.manaRegen += (int)Math.Ceiling(modPlayer.player.manaRegen * 0.1f * modPlayer.GetSkillLevel<Warmth>());
        }
    }
}
