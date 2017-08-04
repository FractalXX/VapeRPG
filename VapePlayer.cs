using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

using VapeRPG.UI.States;
using Terraria.DataStructures;

namespace VapeRPG
{
    class VapePlayer : ModPlayer
    {
        //Dictionary for the stats, and skill levels
        public Dictionary<string, int> BaseStats { get; private set; }
        public Dictionary<string, int> SkillLevels { get; private set; }

        public Dictionary<string, int> EffectiveStats { get; private set; }

        public Dictionary<string, float> ChaosBonuses { get; private set; }

        public int level;
        public long xp;
        public int chaosRank;
        public long chaosXp;

        public int statPoints;
        public int skillPoints;
        public int chaosPoints;

        public float dodgeChance;
        public float blockChance;

        private Vector2 expUIPos;

        private static int statPointsPerLevel;
        private static int skillPointsPerLevel;

        public bool regenKill;

        static VapePlayer()
        {
            statPointsPerLevel = 5;
            skillPointsPerLevel = 1;
        }

        public override TagCompound Save()
        {
            // The TagCompound which we will return
            TagCompound tc = new TagCompound();

            TagCompound baseStatsTC = new TagCompound();
            TagCompound skillLevelsTC = new TagCompound();
            TagCompound chaosBonusesTC = new TagCompound();

            // Boxing values into the compounds
            foreach (var x in BaseStats)
            {
                baseStatsTC.Add(x.Key, x.Value);
            }

            foreach (var x in SkillLevels)
            {
                skillLevelsTC.Add(x.Key, x.Value);
            }

            foreach (var x in ChaosBonuses)
            {
                chaosBonusesTC.Add(x.Key, x.Value);
            }

            tc.Add("BaseStats", baseStatsTC);
            tc.Add("SkillLevels", skillLevelsTC);
            tc.Add("ChaosBonuses", chaosBonusesTC);

            tc.Add("Level", this.level);
            tc.Add("Xp", this.xp);
            tc.Add("ChaosRank", this.chaosRank);
            tc.Add("ChaosXp", this.chaosXp);

            tc.Add("StatPoints", this.statPoints);
            tc.Add("SkillPoints", this.skillPoints);
            tc.Add("ChaosPoints", this.chaosPoints);

            tc.Add("expUIPos", this.expUIPos);

            return tc;
        }

        public override void Load(TagCompound tag)
        {
            // Checking if the player data exists at all
            this.level = tag.GetAsInt("Level");

            if (this.level > 0)
            {
                // Getting sub compounds
                TagCompound stats = tag.GetCompound("BaseStats");
                TagCompound skillLevels = tag.GetCompound("SkillLevels");
                TagCompound chaosBonuses = tag.GetCompound("ChaosBonuses");

                // Unboxing values into the proper dictionary
                foreach (var stat in stats)
                {
                    this.BaseStats[stat.Key] = (int)stat.Value;
                }

                foreach (var skillLevel in skillLevels)
                {
                    this.SkillLevels[skillLevel.Key] = (int)skillLevel.Value;
                }

                foreach (var chaosBonus in chaosBonuses)
                {
                    this.ChaosBonuses[chaosBonus.Key] = (float)chaosBonus.Value;
                }

                this.chaosRank = tag.GetAsInt("ChaosRank");
                this.chaosXp = tag.GetAsLong("ChaosXp");

                this.statPoints = tag.GetAsInt("StatPoints");
                this.skillPoints = tag.GetAsInt("SkillPoints");
                this.chaosPoints = tag.GetAsInt("ChaosPoints");

                this.xp = tag.GetAsLong("Xp");
                Vector2 expUIPos = tag.Get<Vector2>("expUIPos");

                VapeRPG vapeMod = (this.mod as VapeRPG);
                vapeMod.ExpUI.SetPanelPosition(expUIPos);
            }
            // If it doesn't, create a new player
            else
            {
                this.InitializeNewPlayer();
            }
        }

        public void InitializeNewPlayer()
        {
            foreach (string stat in VapeRPG.BaseStats)
            {
                this.BaseStats.Add(stat, 1);
            }

            this.level = 1;
            this.xp = 1;
            this.chaosRank = 0;
            this.chaosXp = 0;

            this.statPoints = 5;
            this.skillPoints = 1;
            this.chaosPoints = 100;
        }

        public override void Initialize()
        {
            // Instantiating the dictionaries
            this.BaseStats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            this.SkillLevels = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            this.EffectiveStats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            this.ChaosBonuses = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);

            foreach (Skill skill in VapeRPG.Skills)
            {
                this.SkillLevels.Add(skill.name, 0);
            }

            foreach (string stat in VapeRPG.MinorStats)
            {
                this.ChaosBonuses.Add(stat, 0);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (VapeRPG.CharWindowHotKey.JustPressed)
            {
                CharUIState.visible = !CharUIState.visible;
            }
        }


        /// <summary>
        /// Gives experience points for the player.
        /// </summary>
        /// <param name="value">The amount of experience given.</param>
        /// <param name="chaos">Determines if the given xp should be chaos xp or not.</param>
        public void GainExperience(int value, bool chaos=false)
        {
            if(chaos)
            {
                if (this.xp < (this.mod as VapeRPG).XpNeededForChaosRank[VapeRPG.MaxLevel])
                {
                    CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 100, 50, 50), Color.DeepPink, String.Format("+{0} Chaos XP", value));
                    this.chaosXp += value;
                }
            }
            else
            {
                if (this.xp < (this.mod as VapeRPG).XpNeededForLevel[VapeRPG.MaxLevel])
                {
                    // Fancy text above the player
                    CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 50, 50, 50), Color.LightGreen, String.Format("+{0} XP", value));
                    this.xp += (long)value;
                }
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = this.mod.GetPacket();

                packet.Write((byte)VapeRPGMessageType.ClientSyncXp);
                packet.Write(this.player.whoAmI);
                packet.Write(this.xp);
                packet.Write(this.chaosXp);

                packet.Send();
            }
        }

        public override void PostUpdate()
        {
            VapeRPG vapeMod = (this.mod as VapeRPG);
            // Just for saving it properly when the player exits
            this.expUIPos = vapeMod.ExpUI.GetPanelPosition();

            if (this.xp > vapeMod.XpNeededForLevel[VapeRPG.MaxLevel])
            {
                this.xp = vapeMod.XpNeededForLevel[VapeRPG.MaxLevel];
            }

            if (this.chaosXp > vapeMod.XpNeededForChaosRank[VapeRPG.MaxLevel])
            {
                this.chaosXp = vapeMod.XpNeededForChaosRank[VapeRPG.MaxLevel];
            }

            // Checking if the player has enough xp to level up
            if (this.xp >= vapeMod.XpNeededForLevel[this.level + 1] && this.level != VapeRPG.MaxLevel)
            {
                this.LevelUp();
            }

            if (this.chaosXp >= vapeMod.XpNeededForChaosRank[this.chaosRank + 1] && this.chaosRank != VapeRPG.MaxLevel)
            {
                this.ChaosRankUp();
            }

            this.CheckExpUIOverflow();

            if(Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = this.mod.GetPacket();

                packet.Write((byte)VapeRPGMessageType.ClientSyncStats);
                packet.Write(this.player.whoAmI);

                foreach(var x in this.BaseStats)
                {
                    packet.Write(String.Format("{0} {1}", x.Key, x.Value));
                }

                foreach(var x in this.EffectiveStats)
                {
                    packet.Write(String.Format("{0} {1}", x.Key, x.Value));
                }

                packet.Send();
            }

            if(this.regenKill)
            {
                this.player.manaRegenBonus += this.SkillLevels["Regenerating Kills"];
            }

            // Updating the UI

            if(!Main.dedServ)
            {
                vapeMod.ExpUI.UpdateXpBar(this.xp, vapeMod.XpNeededForLevel[this.level], vapeMod.XpNeededForLevel[this.level + 1]);
                vapeMod.ExpUI.UpdateChaosXpBar(this.chaosXp, vapeMod.XpNeededForChaosRank[this.chaosRank], vapeMod.XpNeededForChaosRank[this.chaosRank + 1]);
                vapeMod.ExpUI.UpdateLevel(this.level, this.chaosRank);

                vapeMod.ExpUI.UpdateHpMp(this.player.statLife, this.player.statMana, this.player.statLifeMax2, this.player.statManaMax2);

                if (CharUIState.visible)
                {
                    vapeMod.CharUI.UpdateStats(this.BaseStats, this.EffectiveStats, this.statPoints, this.skillPoints);
                    vapeMod.CharUI.UpdateBonusPanel(this.chaosPoints, player.meleeDamage, player.magicDamage, player.rangedDamage, player.meleeCrit, player.magicCrit, player.rangedCrit, 1f / player.meleeSpeed, player.moveSpeed, this.dodgeChance, this.blockChance);
                }

                CustomBuffUIState.visible = !Main.playerInventory;
            }
        }

        public override void ResetEffects()
        {
            this.dodgeChance = 0;
            this.blockChance = 0;

            this.player.meleeDamage = 0.6f;
            this.player.magicDamage = 0.65f;
            this.player.rangedDamage = 0.625f;

            this.regenKill = false;

            foreach (var x in VapeRPG.BaseStats)
            {
                if (this.BaseStats.ContainsKey(x))
                {
                    this.EffectiveStats[x] = this.BaseStats[x];
                }
            }
        }

        private void UpdateStatBonuses()
        {
            this.player.statLifeMax2 += (int)(this.level * 3.53172) + this.EffectiveStats["Vitality"] + this.EffectiveStats["Strength"] / 2;
            this.player.statManaMax2 += this.EffectiveStats["Intellect"] + this.level / 2;

            this.player.meleeDamage += this.EffectiveStats["Strength"] / 500f;
            this.player.magicDamage += this.EffectiveStats["Magic power"] / 430f;
            this.player.rangedDamage += this.EffectiveStats["Dexterity"] / 465f;

            this.player.meleeCrit += this.EffectiveStats["Strength"] / 10;
            this.player.magicCrit += (int)(this.EffectiveStats["Magic power"] / 7f);
            this.player.rangedCrit += (int)(this.EffectiveStats["Agility"] / 8.5f);

            this.player.meleeSpeed += this.EffectiveStats["Agility"] / 265f;

            this.dodgeChance += this.EffectiveStats["Dexterity"] / 900f;

            this.UpdateChaosBonuses();

            SkillController.UpdateStatBonuses(this);
        }

        private void UpdateChaosBonuses()
        {
            this.player.meleeDamage += this.ChaosBonuses["Melee Damage"];
            this.player.rangedDamage += this.ChaosBonuses["Ranged Damage"];
            this.player.magicDamage += this.ChaosBonuses["Magic Damage"];

            this.player.meleeCrit += (int)this.ChaosBonuses["Melee Crit"];
            this.player.rangedCrit += (int)this.ChaosBonuses["Ranged Crit"];
            this.player.magicCrit += (int)this.ChaosBonuses["Magic Crit"];

            this.player.meleeSpeed += this.ChaosBonuses["Melee Speed"];
            this.player.moveSpeed += this.ChaosBonuses["Movement Speed"];
            this.dodgeChance += this.ChaosBonuses["Dodge Chance"];
        }

        public override void PostUpdateEquips()
        {
            this.UpdateStatBonuses();
        }

        /// <summary>
        /// Raises the player's level by one. (with effect)
        /// </summary>
        public void LevelUp()
        {
            // Level up particle effect
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDust(this.player.position, Main.rand.Next(5, 15), Main.rand.Next(5, 15), 130, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                Dust.NewDust(this.player.position, Main.rand.Next(5, 15), Main.rand.Next(5, 15), 131, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                Dust.NewDust(this.player.position, Main.rand.Next(5, 15), Main.rand.Next(5, 15), 132, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                Dust.NewDust(this.player.position, Main.rand.Next(5, 15), Main.rand.Next(5, 15), 133, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
                Dust.NewDust(this.player.position, Main.rand.Next(5, 15), Main.rand.Next(5, 15), 134, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
            }
            CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 50, 100, 100), Color.Cyan, "Level Up");

            this.statPoints += statPointsPerLevel;
            this.skillPoints += skillPointsPerLevel;

            this.level++;
            if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(String.Format("You have reached level {0}!", this.level), 127, 255, 0);
            else
            {
                NetMessage.SendData(25, -1, -1, NetworkText.FromLiteral(String.Format("{0} has reached level {1}!", this.player.name, this.level)), 255, 127, 0, 0, 0);

                if(Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = this.mod.GetPacket();

                    packet.Write((byte)VapeRPGMessageType.ClientSyncLevel);
                    packet.Write(this.player.whoAmI);
                    packet.Write(this.level);

                    packet.Send();
                }
            }
        }

        /// <summary>
        /// Raises the player's chaos rank by one. (with effect)
        /// </summary>
        public void ChaosRankUp()
        {
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDust(this.player.position, Main.rand.Next(5, 15), Main.rand.Next(5, 15), 179, Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));
            }
            CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 100, 100, 100), Color.Violet, "Chaos Rank Up");

            this.chaosPoints++;

            this.chaosRank++;
            if (Main.netMode == 0) Main.NewText(String.Format("You have reached chaos rank {0}!", this.chaosRank), 179, 104, 255);
            else NetMessage.SendData(25, -1, -1, NetworkText.FromLiteral(String.Format("{0} has reached chaos rank {1}!", this.player.name, this.chaosRank)), 179, 104, 255, 0, 0);
        }

        /// <summary>
        /// Returns true if the player has the skill with the given name.
        /// </summary>
        /// <param name="skillName">The name of the skill.</param>
        /// <returns></returns>
        public bool HasSkill(string skillName)
        {
            return this.SkillLevels[skillName] > 0;
        }

        private void CheckExpUIOverflow()
        {
            VapeRPG vapeMod = (this.mod as VapeRPG);

            bool expUIOverflow = false;

            if (this.expUIPos.X >= Main.screenWidth)
            {
                this.expUIPos.X = Main.screenWidth - vapeMod.ExpUI.Width.Pixels;
                expUIOverflow = true;
            }
            if (this.expUIPos.Y >= Main.screenHeight)
            {
                this.expUIPos.Y = Main.screenHeight - vapeMod.ExpUI.Height.Pixels;
                expUIOverflow = true;
            }

            if (expUIOverflow)
            {
                vapeMod.ExpUI.SetPanelPosition(this.expUIPos);
            }
        }

        public override void PostUpdateBuffs()
        {
            VapeRPG vapeMod = this.mod as VapeRPG;
            vapeMod.BuffUI.UpdateBuffs(this.player.buffType);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Main.rand.NextDouble() <= this.dodgeChance)
            {
                this.player.immune = true;
                this.player.immuneTime = 40;
                CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y + 10, 100, 100), Color.Lime, "Dodged");
                Main.PlaySound(2, this.player.position);
                playSound = false;
                genGore = false;
                return false;
            }

            if(Main.rand.NextDouble() <= this.blockChance)
            {
                this.player.immune = true;
                this.player.immuneTime = 40;
                CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y + 10, 100, 100), Color.Lime, "Blocked");
                Main.PlaySound(37, this.player.position);
                playSound = false;
                genGore = false;
                return false;
            }

            return true;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            SkillController.OnHitNPCWithProj(this, proj, target, damage, knockback, crit);
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            return SkillController.ConsumeAmmo(this, weapon, ammo);
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            SkillController.ModifyHitByNPC(this, npc, ref damage, ref crit);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            SkillController.ModifyHitNPC(this, item, target, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            SkillController.ModifyHitNPCWithProj(this, proj, target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SkillController.Shoot(this, item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            return true;
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            SkillController.Hurt(this, pvp, quiet, damage, hitDirection, crit);
        }
    }
}
