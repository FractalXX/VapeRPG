using System;
using VapeRPG.UI.Elements;

namespace VapeRPG.UI
{
    static class TreeHelper
    {
        public static void AddSkillInfo(UISkillInfo skillInfo)
        {
            float x = 0;
            float y = 0;

            float offsetX = 0;

            string name = skillInfo.skill.Name;

            if (skillInfo.skill.Tree == SkillTree.Reaper)
            {
                offsetX = 50;
                if(name.Equals("Excitement", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 10;
                }

                else if(name.Equals("Bloodlust", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 0;
                    y = 94;
                }
                else if(name.Equals("Exploding Rage", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 0;
                    y = 198;
                }

                else if(name.Equals("Rage", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 74;
                    y = 94;
                }
                else if(name.Equals("Overkill", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 74;
                    y = 198;
                }
                else if(name.Equals("Fury", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 74;
                    y = 292;
                }

                else if (name.Equals("Static Field", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 198;
                }
                else if (name.Equals("High-Voltage Field", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 292;
                }

                else if(name.Equals("Mana Addict", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 94;
                }
                else if(name.Equals("Energizing Kills", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 198;
                }

                else if(name.Equals("Magic Sparks", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 296;
                    y = 198;
                }
                else if(name.Equals("Overkill Charge", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 296;
                    y = 292;
                }
                else if(name.Equals("Spectral Sparks", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 296;
                    y = 396;
                }

            }
            if (skillInfo.skill.Tree == SkillTree.Shredder)
            {
                offsetX = 50;
                if(name.Equals("One Above All", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 10;
                }

                else if(name.Equals("Bounce", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 0;
                    y = 94;
                }
                else if (name.Equals("Leftover Supply", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 0;
                    y = 198;
                }

                else if(name.Equals("Confusion", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 74;
                    y = 94;
                }
                else if(name.Equals("Confusion Field", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 74;
                    y = 198;
                }

                else if(name.Equals("High Five", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 94;
                }
                else if(name.Equals("Titan Grip", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 198;
                }
                else if(name.Equals("Hawk Eye", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 292;
                }

                else if(name.Equals("Close Combat Specialist", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 94;
                }
            }
            if(skillInfo.skill.Tree == SkillTree.Power)
            {
                offsetX = 50;

                if(name.Equals("Warmth", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 10;
                }

                else if (name.Equals("First Touch", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 94;
                }
                else if(name.Equals("Kickstart", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 198;
                }
                else if(name.Equals("Execution", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 222;
                    y = 292;
                }

                else if(name.Equals("Reflection", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 74;
                    y = 94;
                }
                else if(name.Equals("Strengthen", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 74;
                    y = 198;
                }

                else if(name.Equals("Damage to Defense", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 94;
                }
                else if(name.Equals("Vital Supplies", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 198;
                }
                else if(name.Equals("Hardened Skin", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 148;
                    y = 292;
                }

                else if(name.Equals("Longer Flight", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 0;
                    y = 94;
                }
                else if(name.Equals("Angel", StringComparison.CurrentCultureIgnoreCase))
                {
                    x = 0;
                    y = 198;
                }
            }

            skillInfo.Left.Set(x + offsetX, 0);
            skillInfo.Top.Set(y, 0);
        }
    }
}
