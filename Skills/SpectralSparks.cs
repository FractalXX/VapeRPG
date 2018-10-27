using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class SpectralSparks : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Spectral sparks";
            this.Description = "Magic sparks also reduce the defense of enemies by 15%.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<OverkillCharge>();
            this.AddPrerequisite<EnergizingKills>();
        }
    }
}
