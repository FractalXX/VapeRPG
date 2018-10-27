using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class HighVoltageField : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "High-Voltage field";
            this.Description = "Static Field applies slow debuff on hit and grants the night vision buff while it lasts. Upon taking fatal damage, your life gets restored to 50. This effect can only occur every 5 minutes. Duration also increases to 20 seconds.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<StaticField>();
            this.AddPrerequisite<EnergizingKills>();
        }
    }
}
