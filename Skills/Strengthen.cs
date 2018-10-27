using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class Strengthen : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Strengthen";
            this.Description = "After a dodge or block, the next time you take damage, the amount is reduced by 15%/level.";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<Reflection>();
        }
    }
}
