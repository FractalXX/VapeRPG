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

            if (skillInfo.skill.type == SkillType.OnKill)
            {
                offsetX = 50;
                if(name == "Excitement")
                {
                    y = 10;
                    x = 104;
                }

                else if(name == "Bloodlust")
                {
                    x = 104;
                    y = 94;
                }
                else if(name == "Exploding Rage")
                {
                    x = 104;
                    y = 198;
                }
                else if(name == "Static Field")
                {
                    x = 104;
                    y = 292;
                }
                else if(name == "High-Voltage Field")
                {
                    x = 104;
                    y = 396;
                }

                else if(name == "Rage")
                {
                    x = 0;
                    y = 198;
                }
                else if(name == "Overkill")
                {
                    x = 0;
                    y = 292;
                }
                else if(name == "Fury")
                {
                    x = 0;
                    y = 396;
                }

                else if(name == "Mana Addict")
                {
                    x = 208;
                    y = 94;
                }
                else if(name == "Energizing Kills")
                {
                    x = 208;
                    y = 198;
                }

                else if(name == "Magic Sparks")
                {
                    x = 312;
                    y = 198;
                }
                else if(name == "Overkill Charge")
                {
                    x = 312;
                    y = 292;
                }
                else if(name == "Spectral Sparks")
                {
                    x = 312;
                    y = 396;
                }

            }
            if (skillInfo.skill.type == SkillType.GeneralWeapon)
            {
                offsetX = 50;
                if(name == "X-Ray Hits")
                {
                    x = 104;
                    y = 10;
                }
                else if(name == "Leftover Supply")
                {
                    x = 0;
                    y = 94;
                }
                else if(name == "Bounce")
                {
                    x = 0;
                    y = 198;
                }
                
                else if(name == "Confusion")
                {
                    x = 104;
                    y = 94;
                }
                else if(name == "Confusion Field")
                {
                    x = 104;
                    y = 198;
                }

                else if(name == "High Five")
                {
                    x = 208;
                    y = 94;
                }
                else if(name == "Titan Grip")
                {
                    x = 208;
                    y = 198;
                }
                else if(name == "Hawk Eye")
                {
                    x = 208;
                    y = 292;
                }

                else if(name == "Ammo Hoarding")
                {
                    x = 312;
                    y = 198;
                }
                else if(name == "Close Combat Specialist")
                {
                    x = 312;
                    y = 292;
                }
            }
            if(skillInfo.skill.type == SkillType.General)
            {
                offsetX = 50;
                if(name == "Warmth")
                {
                    x = 104;
                    y = 10;
                }

                else if(name == "Kickstart")
                {
                    x = 0;
                    y = 94;
                }
                else if(name == "Execution")
                {
                    x = 0;
                    y = 198;
                }
                else if(name == "First Touch")
                {
                    x = 0;
                    y = 292;
                }

                else if(name == "Aggro")
                {
                    x = 104;
                    y = 94;
                }

                else if(name == "Reflection")
                {
                    x = 104;
                    y = 198;
                }
                else if(name == "Strengthen")
                {
                    x = 104;
                    y = 292;
                }

                else if(name == "Damage to Defense")
                {
                    x = 208;
                    y = 198;
                }
                else if(name == "Vital Supplies")
                {
                    x = 208;
                    y = 292;
                }
                else if(name == "Hardened Skin")
                {
                    x = 208;
                    y = 396;
                }

                else if(name == "Longer Flight")
                {
                    x = 312;
                    y = 94;
                }
                else if(name == "Angel")
                {
                    x = 312;
                    y = 198;
                }
            }

            skillInfo.Left.Set(x + offsetX, 0);
            skillInfo.Top.Set(y, 0);
        }
    }
}
