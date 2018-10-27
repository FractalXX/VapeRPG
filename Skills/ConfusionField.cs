using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class ConfusionField : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Confusion field";
            this.Description = "Confusion applies to multiple enemies in a small radius.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<Confusion>();
        }
    }
}
