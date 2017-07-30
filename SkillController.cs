using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameInput;

using VapeRPG.Buffs;
using VapeRPG.Projectiles;

namespace VapeRPG
{
    static class SkillController
    {
        private static Random rnd = new Random();

        public static void UpdateStatBonuses(VapePlayer modPlayer)
        {
            if (modPlayer.HasSkill("Snail armor"))
            {
                float amount = modPlayer.SkillLevels["Snail armor"] / 15f;
                modPlayer.player.moveSpeed -= amount;
                modPlayer.player.statDefense += 1;
                modPlayer.player.statDefense = (int)Math.Floor(modPlayer.player.statDefense * (1 + amount / 2));
            }

            if (modPlayer.HasSkill("Damage to defense"))
            {
                float amount = modPlayer.SkillLevels["Damage to defense"] / 15f;
                modPlayer.player.meleeDamage -= amount;
                modPlayer.player.statDefense += 1;
                modPlayer.player.statDefense = (int)Math.Floor(modPlayer.player.statDefense * (1 + amount));
            }

            if (modPlayer.HasSkill("Sacrifice") && !modPlayer.player.dead)
            {
                float amount = modPlayer.SkillLevels["Sacrifice"] / 15f;
                modPlayer.player.statLifeMax2 = (int)Math.Floor(modPlayer.player.statLifeMax2 * (1 - amount / 2));
                modPlayer.player.meleeDamage += amount;
            }

            if (modPlayer.HasSkill("Longer flight"))
            {
                modPlayer.player.wingTimeMax += 10 * modPlayer.SkillLevels["Longer flight"];
            }
        }

        public static void ModifyHitByNPC(VapePlayer modPlayer, NPC npc, ref int damage, ref bool crit)
        {
            if (modPlayer.HasSkill("Thorns"))
            {
                npc.StrikeNPC(modPlayer.SkillLevels["Thorns"] * modPlayer.BaseStats["Strength"] / 10, 0, 0);
            }
        }

        public static void ModifyHitNPC(VapePlayer modPlayer, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (modPlayer.HasSkill("Hemorrhage"))
            {
                target.AddBuff(modPlayer.mod.BuffType<Hemorrhage>(), 300);
                target.GetGlobalNPC<VapeGlobalNpc>().hemorrhageDamage = (int)Math.Ceiling((item.damage / 2 + modPlayer.BaseStats["Strength"] * modPlayer.SkillLevels["Hemorrhage"]) * 0.1f);
            }
        }

        public static void ModifyHitNPCWithProj(VapePlayer modPlayer, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if(proj.magic)
            {
                if (proj.type != modPlayer.mod.ProjectileType<MagicSpark>() && modPlayer.HasSkill("Magic clusters"))
                {
                    for (int i = 0; i < modPlayer.SkillLevels["Magic clusters"]; i++)
                    {
                        double angle = rnd.Next(1, 360) * Math.PI / 180;
                        Vector2 sparkPosition = new Vector2(target.position.X + 10f * target.width * (float)Math.Cos(angle), target.position.Y + 10f * target.height * (float)Math.Sin(angle));
                        Vector2 sparkVelocity = target.position - sparkPosition;

                        int v = 15;
                        float speedMul = v / sparkVelocity.Length();
                        sparkVelocity.X = speedMul * sparkVelocity.X;
                        sparkVelocity.Y = speedMul * sparkVelocity.Y;

                        int spark = Projectile.NewProjectile(sparkPosition, sparkVelocity, modPlayer.mod.ProjectileType<MagicSpark>(), damage / 5, 0, Main.myPlayer);

                        float d = Vector2.Distance(sparkPosition, target.position);
                        float t = d / v;
                        Main.projectile[spark].timeLeft = (int)Math.Ceiling(t);

                        Main.projectile[spark].magic = true;
                        Main.projectile[spark].tileCollide = false;
                    }
                }

                if (crit)
                {
                    if (modPlayer.HasSkill("Mana crits"))
                    {
                        int amount = 5 * modPlayer.SkillLevels["Mana crits"];
                        modPlayer.player.statMana += amount;
                        modPlayer.player.ManaEffect(amount);
                    }
                }
            }
            else
            {
                if (proj.ranged && !proj.Name.Contains("Rocket") && modPlayer.HasSkill("Explosive shots"))
                {
                    Projectile explosionDummy = Projectile.NewProjectileDirect(target.position, Vector2.Zero, ProjectileID.RocketI, damage * modPlayer.SkillLevels["Explosive shots"] / 10, 20, Main.myPlayer);
                    explosionDummy.timeLeft = 10;
                }

                if (proj.ranged && !proj.Name.Contains("Rocket") && modPlayer.HasSkill("Incendiary shots"))
                {
                    target.AddBuff(24, 300);
                }
            }
        }

        public static void OnHitNPCWithProj(VapePlayer modPlayer, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (modPlayer.HasSkill("Regenerating Kills"))
            {
                modPlayer.player.AddBuff(modPlayer.mod.BuffType<RegenKill>(), 300);
            }
        }

        public static void Shoot(VapePlayer modPlayer, Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

        }

        public static void Hurt(VapePlayer modPlayer, bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (modPlayer.HasSkill("Longer invulnerability"))
            {
                modPlayer.player.immuneTime += 30 * modPlayer.SkillLevels["Longer invulnerability"];
            }

            if (modPlayer.HasSkill("Steroids"))
            {
                modPlayer.player.AddBuff(modPlayer.mod.BuffType<Steroids>(), 300);
            }
        }

        public static bool ConsumeAmmo(VapePlayer modPlayer, Item weapon, Item ammo)
        {
            return !(modPlayer.HasSkill("Ammo hoarding") && Main.rand.Next(0, 101) <= modPlayer.SkillLevels["Ammo hoarding"] * 5);
        }
    }
}
