using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameInput;

using VapeRPG.Projectiles;
using VapeRPG.Buffs;

namespace VapeRPG
{
    static class SkillController
    {
        private static Random rnd = new Random();

        public static void UpdateStatBonuses(VapePlayer modPlayer)
        {
            if (modPlayer.HasSkill("Warmth"))
            {
                modPlayer.player.lifeRegen += (int)Math.Ceiling(modPlayer.player.lifeRegen * 0.1f * modPlayer.SkillLevels["Warmth"]);
                modPlayer.player.manaRegen += (int)Math.Ceiling(modPlayer.player.manaRegen * 0.1f * modPlayer.SkillLevels["Warmth"]);
            }
            if (modPlayer.HasSkill("Damage to Defense"))
            {
                float amount = modPlayer.SkillLevels["Damage to Defense"] * 0.1f;
                modPlayer.player.meleeDamage -= amount;
                modPlayer.player.magicDamage -= amount;
                modPlayer.player.rangedDamage -= amount;
                modPlayer.player.minionDamage -= amount;
                modPlayer.player.thrownDamage -= amount;
                modPlayer.player.statDefense += (int)Math.Ceiling(modPlayer.player.statDefense * amount);
                if (modPlayer.HasSkill("Vital Supplies"))
                {
                    modPlayer.player.statLifeMax2 += (int)Math.Ceiling(modPlayer.player.statLifeMax * amount);
                }
            }
            if (modPlayer.HasSkill("Longer Flight"))
            {
                modPlayer.player.wingTimeMax += (int)Math.Ceiling(modPlayer.player.wingTimeMax * 0.3f * modPlayer.SkillLevels["Longer Flight"]);
            }
            if (modPlayer.HasSkill("Angel") && modPlayer.player.wingTime < modPlayer.player.wingTimeMax)
            {
                float amount = modPlayer.SkillLevels["Angel"] * 0.05f;
                modPlayer.player.meleeDamage += amount;
                modPlayer.player.rangedDamage += amount;
                modPlayer.player.magicDamage += amount;
                modPlayer.player.minionDamage += amount;
                modPlayer.player.thrownDamage += amount;
            }
        }

        public static void ModifyHitByNPC(VapePlayer modPlayer, NPC npc, ref int damage, ref bool crit)
        {
            if (modPlayer.HasSkill("Reflection"))
            {
                npc.StrikeNPC((int)Math.Ceiling(damage * 0.05f * modPlayer.SkillLevels["Reflection"]), 30, npc.direction);
            }
            if (modPlayer.HasSkill("Hardened Skin"))
            {
                damage -= (int)Math.Ceiling(damage * 0.05f * modPlayer.SkillLevels["Hardened Skin"]);
            }
        }

        public static void ModifyHitByProjectile(VapePlayer modPlayer, Projectile proj, ref int damage, ref bool crit)
        {

        }

        public static void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (!crit)
            {
                if (modPlayer.HasSkill("Kickstart") && target.life >= target.lifeMax * 0.7f && rnd.Next(0, 101) <= modPlayer.SkillLevels["Kickstart"] * 5)
                {
                    crit = true;
                }
                if(modPlayer.HasSkill("Hawk Eye"))
                {
                    float distance = Vector2.Distance(modPlayer.player.position, target.position);
                    float chance = distance * 0.025f;
                    if(rnd.Next(0, 101) <= chance)
                    {
                        crit = true;
                    }
                }
            }
            else
            {
                if (modPlayer.HasSkill("Titan Grip"))
                {
                    knockback += knockback * modPlayer.SkillLevels["Titan Grip"] * 0.2f;
                }
            }

            if(modPlayer.HasSkill("Close Combat Specialist"))
            {
                if(Vector2.Distance(modPlayer.player.position, target.position) < 10)
                {
                    damage = (int)(damage * 1.5f);
                }
            }

            if (modPlayer.HasSkill("Execution") && target.life <= target.lifeMax * 0.2f)
            {
                damage += (int)Math.Ceiling(damage * 0.1f * modPlayer.SkillLevels["Execution"]);
            }
            if (modPlayer.HasSkill("First Touch") && target.life == target.lifeMax)
            {
                int amount = (int)Math.Floor(target.life * 0.1f);
                target.life -= amount;
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 50, 100, 100), Color.Cyan, amount);
            }
            if (modPlayer.HasSkill("One Above All") && rnd.Next(0, 101) <= 2 && target.type != NPCID.TargetDummy && !target.boss && !VapeConfig.IsIgnoredTypeChaos(target))
            {
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 20, 50, 50), Color.Red, "One Above All");
                target.StrikeNPC(target.life / 2, 0, 0);
            }
        }

        public static void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            // On Kill
            if (target.life <= 0)
            {
                if (modPlayer.HasSkill("Excitement"))
                {
                    modPlayer.player.AddBuff(11, 360);
                }
                if (modPlayer.HasSkill("Bloodlust"))
                {
                    modPlayer.player.AddBuff(2, 600);
                }
                if (modPlayer.HasSkill("Exploding Rage") && (proj == null ? item.melee : proj.melee) && rnd.Next(101) <= modPlayer.SkillLevels["Exploding Rage"] * 10)
                {
                    int dustCount = 20;
                    int bloodRange = 15;
                    for (int i = 0; i < dustCount; i++)
                    {
                        for (int j = 0; j < dustCount; j++)
                        {
                            double angle = rnd.Next(1, 360) * Math.PI / 180;
                            Vector2 bloodTarget = new Vector2(target.position.X + bloodRange * (float)Math.Cos(angle), target.position.Y + bloodRange * (float)Math.Sin(angle));
                            Vector2 bloodVelocity = target.position - bloodTarget;

                            int v = 10;
                            float speedMul = v / bloodVelocity.Length();
                            bloodVelocity.X = speedMul * bloodVelocity.X;
                            bloodVelocity.Y = speedMul * bloodVelocity.Y;
                            //Dust.NewDustPerfect(target.position, 5, bloodVelocity, Scale: 3.0f).noGravity = true; //perfect circle
                            Main.dust[Dust.NewDust(target.position, 60, 30, 5, bloodVelocity.X, bloodVelocity.Y, Scale: 3.0f)].noGravity = true;
                        }
                    }
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc != target && Vector2.Distance(npc.position, target.position) <= 150)
                        {
                            npc.StrikeNPC((int)Math.Ceiling(target.lifeMax * 0.1f), 0, 0);
                        }
                    }
                }
                if (modPlayer.HasSkill("Rage"))
                {
                    modPlayer.player.AddBuff(modPlayer.mod.BuffType<Rage>(), 600);
                }
                if (modPlayer.HasSkill("Mana Addict"))
                {
                    modPlayer.player.AddBuff(6, 600);
                }
                if (modPlayer.HasSkill("Magic Sparks") && (proj == null ? item.magic : proj.magic) && rnd.Next(0, 101) <= modPlayer.SkillLevels["Magic Sparks"] * 10)
                {
                    int dustCount = 20;
                    int sparkRange = 30;
                    for (int i = 0; i < dustCount; i++)
                    {
                        for (int j = 0; j < dustCount; j++)
                        {
                            double angle = rnd.Next(1, 360) * Math.PI / 180;
                            Vector2 sparkTarget = new Vector2(target.position.X + sparkRange * (float)Math.Cos(angle), target.position.Y + sparkRange * (float)Math.Sin(angle));
                            Vector2 sparkVelocity = target.position - sparkTarget;

                            int v = 15;
                            float speedMul = v / sparkVelocity.Length();
                            sparkVelocity.X = speedMul * sparkVelocity.X;
                            sparkVelocity.Y = speedMul * sparkVelocity.Y;
                            Dust dust = Dust.NewDustPerfect(target.position, 130 + j % 5, sparkVelocity);
                            dust.noGravity = true;
                        }
                    }
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc != target && Vector2.Distance(npc.position, target.position) <= 180)
                        {
                            int amount = (int)Math.Ceiling(npc.lifeMax * modPlayer.SkillLevels["Magic Sparks"] * 0.05f);
                            npc.life -= amount;
                            CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y - 50, 100, 100), Color.Cyan, amount);
                            if (modPlayer.HasSkill("Spectral Sparks"))
                            {
                                npc.defense -= (int)Math.Ceiling(npc.defense * 0.15f);
                            }
                        }
                    }
                }
                if (modPlayer.HasSkill("Energizing Kills") && !modPlayer.energized && (proj == null ? item.magic : proj.magic))
                {
                    if(modPlayer.energizedStacks <= 0)
                    {
                        modPlayer.energizedStacks = 1;
                    }
                    modPlayer.player.AddBuff(modPlayer.mod.BuffType<Energized>(), 60);
                }
                if(modPlayer.HasSkill("Static Field") && rnd.Next(0, 101) <= modPlayer.SkillLevels["Static Field"] * 5 && (proj == null ? item.magic : proj.magic))
                {
                    int duration = 600;
                    if(modPlayer.HasSkill("High-Voltage Field"))
                    {
                        duration *= 2;
                        modPlayer.player.AddBuff(12, duration);
                    }
                    Main.PlaySound(modPlayer.mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/StaticFieldApply"), modPlayer.player.position);
                    modPlayer.player.AddBuff(modPlayer.mod.BuffType<StaticField>(), duration);
                }


                // On Crit Kill
                if (crit)
                {
                    if (modPlayer.HasSkill("Overkill"))
                    {
                        int amount = (int)Math.Ceiling(modPlayer.player.statLifeMax * modPlayer.SkillLevels["Overkill"] * 0.03f);
                        modPlayer.player.HealEffect(amount);
                        modPlayer.player.statLife += amount;
                    }
                    if (modPlayer.HasSkill("Overkill Charge"))
                    {
                        int amount = (int)Math.Ceiling(modPlayer.player.statLifeMax * modPlayer.SkillLevels["Overkill Charge"] * 0.04f);
                        modPlayer.player.ManaEffect(amount);
                        modPlayer.player.statMana += amount;
                    }
                }
            }
            //On Hit
            if (modPlayer.HasSkill("One Above All") && rnd.Next(0, 101) <= 2 && target.type != NPCID.TargetDummy && !target.boss && !VapeConfig.IsIgnoredTypeChaos(target))
            {
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 20, 50, 50), Color.Red, "One Above All");
                target.StrikeNPC(target.life * 2, 0, 0);
            }
            if ((proj == null ? item.magic : proj.magic) && modPlayer.HasSkill("Bounce") && rnd.Next(0, 101) <= modPlayer.SkillLevels["Bounce"] * 10)
            {
                NPC closest = null;
                float closestDistance = 500;

                foreach (NPC npc in Main.npc)
                {
                    if (npc != target)
                    {
                        float distance = Vector2.Distance(npc.position, target.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closest = npc;
                        }
                    }
                }

                if (closest != null)
                {
                    Vector2 sparkVelocity = closest.position - target.position;

                    int v = 5;
                    float speedMul = v / sparkVelocity.Length();
                    sparkVelocity.X = speedMul * sparkVelocity.X;
                    sparkVelocity.Y = speedMul * sparkVelocity.Y;

                    float sparkDamage = (proj == null ? item.damage : proj.damage) * modPlayer.SkillLevels["Bounce"] * 0.1f;
                    Projectile spark = Projectile.NewProjectileDirect(new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2), sparkVelocity, modPlayer.mod.ProjectileType<ElectricSpark>(), (int)Math.Ceiling(sparkDamage), 20, modPlayer.player.whoAmI);
                    Main.PlaySound(modPlayer.mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Electricity"), modPlayer.player.position);
                }
            }
            if (modPlayer.HasSkill("Confusion") && rnd.Next(0, 101) <= modPlayer.SkillLevels["Confusion"] * 5)
            {
                target.AddBuff(31, 300);
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 20, 50, 50), Color.OrangeRed, "Confused");
                if (modPlayer.HasSkill("Confusion Field"))
                {
                    float confusionDistance = 200;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc != target && Vector2.Distance(target.position, npc.position) <= confusionDistance)
                        {
                            npc.AddBuff(31, 300);
                            CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y - 20, 50, 50), Color.OrangeRed, "Confused");
                        }
                    }
                }
            }

            if (!crit)
            {
                if (modPlayer.highfiveStacks > 0)
                {
                    modPlayer.player.ClearBuff(modPlayer.mod.BuffType<HighFive>());
                    modPlayer.highfiveStacks = 0;
                }
            }
            else
            {
                if(modPlayer.HasSkill("High Five"))
                {
                    if(modPlayer.highfiveStacks <= 0)
                    {
                        modPlayer.highfiveStacks = 1;
                    }
                    modPlayer.player.AddBuff(modPlayer.mod.BuffType<HighFive>(), 60);
                }
            }
        }

        public static void Shoot(VapePlayer modPlayer, Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

        }

        public static void Hurt(VapePlayer modPlayer, bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {

        }

        public static void UseItem(VapePlayer modPlayer, Item item)
        {
            if (item.magic && modPlayer.HasSkill("Leftover Supply") && rnd.Next(0, 101) <= modPlayer.SkillLevels["Leftover Supply"] * 2f)
            {
                modPlayer.player.statMana += item.mana;
            }
        }
    }
}
