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

            string name = skillInfo.skill.name;

            if (skillInfo.skill.tree == SkillTree.Reaper)
            {
                offsetX = 50;
                if(name == "Excitement")
                {
                    x = 222;
                    y = 10;
                }

                else if(name == "Bloodlust")
                {
                    x = 0;
                    y = 94;
                }
                else if(name == "Exploding Rage")
                {
                    x = 0;
                    y = 198;
                }

                else if(name == "Rage")
                {
                    x = 74;
                    y = 94;
                }
                else if(name == "Overkill")
                {
                    x = 74;
                    y = 198;
                }
                else if(name == "Fury")
                {
                    x = 74;
                    y = 292;
                }

                else if (name == "Static Field")
                {
                    x = 148;
                    y = 198;
                }
                else if (name == "High-Voltage Field")
                {
                    x = 148;
                    y = 292;
                }

                else if(name == "Mana Addict")
                {
                    x = 222;
                    y = 94;
                }
                else if(name == "Energizing Kills")
                {
                    x = 222;
                    y = 198;
                }

                else if(name == "Magic Sparks")
                {
                    x = 296;
                    y = 198;
                }
                else if(name == "Overkill Charge")
                {
                    x = 296;
                    y = 292;
                }
                else if(name == "Spectral Sparks")
                {
                    x = 296;
                    y = 396;
                }

            }
            if (skillInfo.skill.tree == SkillTree.Shredder)
            {
                offsetX = 50;
                if(name == "One Above All")
                {
                    x = 222;
                    y = 10;
                }

                else if(name == "Bounce")
                {
                    x = 0;
                    y = 94;
                }
                else if (name == "Leftover Supply")
                {
                    x = 0;
                    y = 198;
                }

                else if(name == "Confusion")
                {
                    x = 74;
                    y = 94;
                }
                else if(name == "Confusion Field")
                {
                    x = 74;
                    y = 198;
                }

                else if(name == "High Five")
                {
                    x = 148;
                    y = 94;
                }
                else if(name == "Titan Grip")
                {
                    x = 148;
                    y = 198;
                }
                else if(name == "Hawk Eye")
                {
                    x = 148;
                    y = 292;
                }

                else if(name == "Close Combat Specialist")
                {
                    x = 222;
                    y = 94;
                }
            }
            if(skillInfo.skill.tree == SkillTree.Power)
            {
                offsetX = 50;

                if(name == "Warmth")
                {
                    x = 222;
                    y = 10;
                }

                else if(name == "Kickstart")
                {
                    x = 222;
                    y = 94;
                }
                else if(name == "Execution")
                {
                    x = 222;
                    y = 198;
                }
                else if(name == "First Touch")
                {
                    x = 222;
                    y = 292;
                }

                else if(name == "Reflection")
                {
                    x = 74;
                    y = 94;
                }
                else if(name == "Strengthen")
                {
                    x = 74;
                    y = 198;
                }

                else if(name == "Damage to Defense")
                {
                    x = 148;
                    y = 94;
                }
                else if(name == "Vital Supplies")
                {
                    x = 148;
                    y = 198;
                }
                else if(name == "Hardened Skin")
                {
                    x = 148;
                    y = 292;
                }

                else if(name == "Longer Flight")
                {
                    x = 0;
                    y = 94;
                }
                else if(name == "Angel")
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
