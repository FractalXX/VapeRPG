using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class Fury : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Fury";
            this.Description = "The Rage buff also increases your melee speed by the same amount.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<Overkill>();
        }
    }
}
