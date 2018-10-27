using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class LongerFlight : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Longer flight";
            this.Description = "Your flight time is increased by 30%/level.";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<Warmth>();
        }

        public override void UpdateStats(VapePlayer modPlayer)
        {
            modPlayer.player.wingTimeMax += (int)Math.Ceiling(modPlayer.player.wingTimeMax * 0.3f * modPlayer.GetSkillLevel<LongerFlight>());
        }
    }
}
