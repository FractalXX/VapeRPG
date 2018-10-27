using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class VitalSupplies : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Vital supplies";
            this.Description = "Damage to Defense increases life as well.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<DamageToDefense>();
        }

        public override void UpdateStats(VapePlayer modPlayer)
        {
            modPlayer.player.statLifeMax2 += (int)Math.Ceiling(modPlayer.player.statLifeMax * modPlayer.GetSkillLevel<DamageToDefense>() * 0.1f);
        }
    }
}
