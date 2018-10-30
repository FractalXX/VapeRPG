using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using Terraria.DataStructures;

using VapeRPG.UI.States;
using VapeRPG.Buffs;
using VapeRPG.Projectiles;
using VapeRPG.Items;

namespace VapeRPG
{
    public class VapePlayer : ModPlayer
    {
        private static int statPointsPerLevel;
        private static Random rnd;

        //Dictionary for the stats, and skill levels
        public IDictionary<string, int> BaseStats { get; private set; }

        public IDictionary<string, int> EffectiveStats { get; private set; }

        public IDictionary<string, float> ChaosBonuses { get; private set; }

        public int level;
        public long xp;
        public int chaosRank;
        public long chaosXp;

        public int statPoints;
        public int skillPoints;
        public int chaosPoints;

        public float dodgeChance;
        public float blockChance;

        // Buffs
        internal bool rageBuff;
        internal bool energized;
        internal bool strengthened;

        internal int fieldCounter = 0;

        // Buff Stacks
        internal int energizedStacks;
        internal int highfiveStacks;

        private Dictionary<Type, int> skillLevels;

        private Vector2 expUIPosition;
        private Vector2 skillUIPosition;

        static VapePlayer()
        {
            statPointsPerLevel = VapeConfig.StatPointsPerLevel;
            rnd = new Random();
        }

        /// <summary>
        /// Raises the player's chaos rank by one. (with effect)
        /// </summary>
        public void ChaosRankUp()
        {
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDust(this.player.position, rnd.Next(5, 15), rnd.Next(5, 15), 179, rnd.Next(-10, 10), rnd.Next(-10, 10));
            }
            CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 100, 100, 100), Color.Violet, "Chaos Rank Up");

            this.chaosPoints++;

            this.chaosRank++;
            if (Main.netMode == 0) Main.NewText(String.Format("You have reached chaos rank {0}!", this.chaosRank), 179, 104, 255);
            else NetMessage.SendData(25, -1, -1, NetworkText.FromLiteral(String.Format("{0} has reached chaos rank {1}!", this.player.name, this.chaosRank)), 179, 104, 255, 0, 0);
        }

        /// <summary>
        /// Gives experience points for the player.
        /// </summary>
        /// <param name="value">The amount of experience given.</param>
        /// <param name="chaos">Determines if the given xp should be chaos xp or not.</param>
        public void GainExperience(int value, bool chaos = false)
        {
            long xp = this.xp;
            long chaosXp = this.chaosXp;

            VapeRPG vapeMod = this.mod as VapeRPG;

            if (chaos)
            {
                if (this.chaosXp < vapeMod.XpNeededForChaosRank[VapeRPG.MaxLevel])
                {
                    CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 100, 50, 50), Color.DeepPink, String.Format("+{0} Chaos XP", value));
                    this.chaosXp += value;
                }
            }
            else
            {
                if (this.xp < vapeMod.XpNeededForLevel[VapeRPG.MaxLevel])
                {
                    // Fancy text above the player
                    CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 50, 50, 50), Color.LightGreen, String.Format("+{0} XP", value));
                    this.xp += (long)(value * VapeConfig.GlobalXpMultiplier);
                }
            }

            // For debugging
            if (this.xp < 0)
            {
                this.xp = xp;
                Main.NewText("[Vape RPG Warning]: Xp after gain would have been either negative or bigger than maximum. To avoid corruption, it remained unchanged.", Color.Red);
                Main.NewText("[Vape RPG Warning]: Please report this bug with details in the mod's topic on the Terraria forums.", Color.Red);
            }
            if (this.chaosXp < 0)
            {
                this.chaosXp = chaosXp;
                Main.NewText("[Vape RPG Warning]: Chaos Xp after gain would have been either negative or bigger than maximum. To avoid corruption, it remained unchanged.", Color.Red);
                Main.NewText("[Vape RPG Warning]: Please report this bug with details in the mod's topic on the Terraria forums.", Color.Red);
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

        /// <summary>
        /// Returns true if the player has the skill with the given name.
        /// </summary>
        /// <param name="skillName">The name of the skill.</param>
        /// <returns></returns>
        public bool HasSkill<T>()
            where T : Skill
        {
            return this.skillLevels.ContainsKey(typeof(T)) && this.skillLevels[typeof(T)] > 0;
        }

        public bool HasSkill(Type type)
        {
            return this.skillLevels.ContainsKey(type) && this.skillLevels[type] > 0;
        }

        public int GetSkillLevel<T>()
            where T : Skill
        {
            return this.skillLevels[typeof(T)];
        }

        public int GetSkillLevel(Type type)
        {
            return this.skillLevels[type];
        }

        public void SetSkillLevel<T>(int level)
        {
            this.skillLevels[typeof(T)] = level;
        }

        public void SetSkillLevel(Type type, int level)
        {
            this.skillLevels[type] = level;
        }

        public bool HasPrerequisiteForSkill(Skill skill)
        {
            int c = 0;
            var prerequisites = skill.GetPrerequisites();
            foreach (Type s in prerequisites)
            {
                if (this.HasSkill(s))
                {
                    c++;
                }
            }
            return c == prerequisites.Count;
        }

        public override void Initialize()
        {
            // Instantiating the dictionaries
            this.BaseStats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            this.skillLevels = new Dictionary<Type, int>();
            this.EffectiveStats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            this.ChaosBonuses = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);

            this.energizedStacks = 0;
            this.highfiveStacks = 0;

            this.fieldCounter = 0;

            foreach (string stat in VapeRPG.BaseStats)
            {
                this.BaseStats.Add(stat, 1);
            }

            foreach (Skill skill in VapeRPG.Skills)
            {
                this.skillLevels.Add(skill.GetType(), 0);
            }

            foreach (string stat in VapeRPG.MinorStats)
            {
                this.ChaosBonuses.Add(stat, 0);
            }
        }

        internal void InitializeNewPlayer()
        {
            this.level = 1;
            this.xp = 1;
            this.chaosRank = 0;
            this.chaosXp = 0;

            this.statPoints = 5;
            this.skillPoints = 1;
            this.chaosPoints = 0;
        }

        /// <summary>
        /// Raises the player's level by one. (with effect)
        /// </summary>
        public void LevelUp()
        {
            // Level up particle effect
            for (int i = 0; i < 50; i++)
            {
                Dust.NewDust(this.player.position, rnd.Next(5, 15), rnd.Next(5, 15), 130, rnd.Next(-10, 10), rnd.Next(-10, 10));
                Dust.NewDust(this.player.position, rnd.Next(5, 15), rnd.Next(5, 15), 131, rnd.Next(-10, 10), rnd.Next(-10, 10));
                Dust.NewDust(this.player.position, rnd.Next(5, 15), rnd.Next(5, 15), 132, rnd.Next(-10, 10), rnd.Next(-10, 10));
                Dust.NewDust(this.player.position, rnd.Next(5, 15), rnd.Next(5, 15), 133, rnd.Next(-10, 10), rnd.Next(-10, 10));
                Dust.NewDust(this.player.position, rnd.Next(5, 15), rnd.Next(5, 15), 134, rnd.Next(-10, 10), rnd.Next(-10, 10));
            }
            CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y - 50, 100, 100), Color.Cyan, "Level Up");

            this.statPoints += statPointsPerLevel;
            if (this.level % 5 == 0) this.skillPoints++;

            this.level++;
            if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(String.Format("You have reached level {0}!", this.level), 127, 255, 0);
            else
            {
                NetMessage.SendData(25, -1, -1, NetworkText.FromLiteral(String.Format("{0} has reached level {1}!", this.player.name, this.level)), 255, 127, 0, 0, 0);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = this.mod.GetPacket();

                    packet.Write((byte)VapeRPGMessageType.ClientSyncLevel);
                    packet.Write(this.player.whoAmI);
                    packet.Write(this.level);

                    packet.Send();
                }
            }
        }

        public override void Load(TagCompound tag)
        {
            // Checking if the player data exists at all
            this.level = tag.GetAsInt("Level");

            if (this.level > 0)
            {
                SaveVersionHandler.Load(this, tag);
            }
            // If it doesn't, create a new player
            else
            {
                this.InitializeNewPlayer();
            }
        }

        public override void OnEnterWorld(Player player)
        {
            this.energizedStacks = 0;
            this.highfiveStacks = 0;
            this.fieldCounter = 0;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (VapeRPG.CharWindowHotKey.JustPressed)
            {
                CharUIState.visible = !CharUIState.visible;
                StatHelpUIState.visible = false;
            }

            VapeRPG vapeMod = this.mod as VapeRPG;
            for(int i = 0; i < SkillBarUIState.SKILL_SLOT_COUNT; i++)
            {
                if(VapeRPG.SkillHotKeys[i].JustPressed)
                {
                    vapeMod.SkillBarUI.SkillSlots[i].UseSkill(this.player);
                }
            }
        }

        public override void PostUpdate()
        {
            VapeRPG vapeMod = (this.mod as VapeRPG);
            // Just for saving it properly when the player exits
            this.expUIPosition = vapeMod.ExpUI.Position;
            this.skillUIPosition = vapeMod.SkillBarUI.Position;

            if (this.level > VapeRPG.MaxLevel) this.level = VapeRPG.MaxLevel;
            else if (this.level < 1) this.level = 1;

            if (this.xp > vapeMod.XpNeededForLevel[VapeRPG.MaxLevel])
            {
                this.xp = vapeMod.XpNeededForLevel[VapeRPG.MaxLevel];
            }

            if (this.chaosXp > vapeMod.XpNeededForChaosRank[VapeRPG.MaxLevel])
            {
                this.chaosXp = vapeMod.XpNeededForChaosRank[VapeRPG.MaxLevel];
            }

            // Checking if the player has enough xp to level up
            if (this.level < VapeRPG.MaxLevel && this.xp >= vapeMod.XpNeededForLevel[this.level + 1])
            {
                this.LevelUp();
            }

            if (this.chaosRank < VapeRPG.MaxLevel && this.chaosXp >= vapeMod.XpNeededForChaosRank[this.chaosRank + 1])
            {
                this.ChaosRankUp();
            }

            this.CheckExpUIOverflow();

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = this.mod.GetPacket();

                packet.Write((byte)VapeRPGMessageType.ClientSyncStats);
                packet.Write(this.player.whoAmI);

                foreach (var x in this.BaseStats)
                {
                    packet.Write(String.Format("{0} {1}", x.Key, x.Value));
                }

                foreach (var x in this.EffectiveStats)
                {
                    packet.Write(String.Format("{0} {1}", x.Key, x.Value));
                }

                packet.Send();
            }

            if (this.fieldCounter > 0 && this.fieldCounter % 15 == 0)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (!npc.friendly && npc.active)
                    {
                        if (Vector2.Distance(npc.position, player.position) <= StaticField.range)
                        {
                            int baseDamage = 15 * this.GetSkillLevel<Skills.StaticField>();
                            npc.StrikeNPC((int)Math.Ceiling(baseDamage * this.player.minionDamage), 0, 0);
                            if (this.HasSkill<Skills.HighVoltageField>())
                            {
                                npc.AddBuff(32, 300);
                            }
                        }
                    }
                }
            }

            // Updating the UI

            if (!Main.dedServ)
            {
                int nextLevel = this.level + 1;
                if (this.level == VapeRPG.MaxLevel)
                {
                    nextLevel = VapeRPG.MaxLevel;
                }

                int nextRank = this.chaosRank + 1;
                if (this.chaosRank == VapeRPG.MaxLevel)
                {
                    nextRank = VapeRPG.MaxLevel;
                }

                vapeMod.ExpUI.UpdateXpBar(this.xp, vapeMod.XpNeededForLevel[this.level], vapeMod.XpNeededForLevel[nextLevel]);
                vapeMod.ExpUI.UpdateChaosXpBar(this.chaosXp, vapeMod.XpNeededForChaosRank[this.chaosRank], vapeMod.XpNeededForChaosRank[nextRank]);
                vapeMod.ExpUI.UpdateLevel(this.level, this.chaosRank);

                if (CharUIState.visible)
                {
                    vapeMod.CharUI.UpdateStats(this.BaseStats, this.EffectiveStats, this.statPoints, this.skillPoints);
                    vapeMod.CharUI.UpdateBonusPanel(this.chaosPoints, player.meleeDamage, player.magicDamage, player.rangedDamage, player.meleeCrit, player.magicCrit, player.rangedCrit, 1f / player.meleeSpeed, player.maxRunSpeed, this.dodgeChance, this.blockChance, player.maxMinions, player.minionDamage);
                    vapeMod.CharUI.UpdateLevel(this.level, this.chaosRank);
                    vapeMod.CharUI.UpdateXpBar(this.xp, vapeMod.XpNeededForLevel[this.level], vapeMod.XpNeededForLevel[nextLevel]);
                    vapeMod.CharUI.UpdateChaosXpBar(this.chaosXp, vapeMod.XpNeededForChaosRank[this.chaosRank], vapeMod.XpNeededForChaosRank[nextRank]);
                }
            }
        }

        public override void PostUpdateBuffs()
        {
            if (this.rageBuff)
            {
                this.player.meleeDamage += this.GetSkillLevel<Skills.Rage>() * 0.03f;
                if (this.HasSkill<Skills.Fury>())
                {
                    this.player.meleeSpeed += this.GetSkillLevel<Skills.Rage>() * 0.03f;
                }
            }
            if (this.player.FindBuffIndex(mod.BuffType<StaticField>()) != -1)
            {
                this.fieldCounter++;
            }
            else
            {
                this.fieldCounter = 0;
            }
        }

        public override void PostUpdateEquips()
        {
            this.UpdateStatBonuses();
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            bool failed = false;
            if (rnd.NextDouble() <= this.dodgeChance)
            {
                this.player.immune = true;
                this.player.immuneTime = 40;
                CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y + 10, 100, 100), Color.Lime, "Dodged");
                Main.PlaySound(2, this.player.position);
                playSound = false;
                genGore = false;
                failed = true;
            }

            if (rnd.NextDouble() <= this.blockChance)
            {
                this.player.immune = true;
                this.player.immuneTime = 40;
                CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y + 10, 100, 100), Color.Lime, "Blocked");
                Main.PlaySound(37, this.player.position);
                playSound = false;
                genGore = false;
                failed = true;
            }

            if (this.player.statLife - damage <= 0 && this.player.FindBuffIndex(mod.BuffType<StaticField>()) != -1 && this.HasSkill<Skills.HighVoltageField>() && this.player.FindBuffIndex(mod.BuffType<FieldSickness>()) == -1)
            {
                this.player.immune = true;
                this.player.immuneTime = 120;
                this.player.statLife = 50;
                this.player.AddBuff(mod.BuffType<FieldSickness>(), 18000);
                CombatText.NewText(new Rectangle((int)this.player.position.X, (int)this.player.position.Y + 10, 100, 100), Color.Lime, "Defibrillated");
                Main.PlaySound(37, this.player.position);
                failed = true;
            }

            if (failed && this.GetSkillLevel<Skills.Strengthen>() > 0)
            {
                this.player.AddBuff(mod.BuffType<Strengthened>(), 18000);
            }

            if (this.strengthened)
            {
                damage -= (int)(damage * 0.15f * this.GetSkillLevel<Skills.Strengthen>());
                this.player.ClearBuff(this.mod.BuffType<Strengthened>());
            }

            if (this.energized)
            {
                int sparkRange = 10;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Electricity"), this.player.position);
                for (int i = 0; i < 360; i += 72)
                {
                    double angle = i * Math.PI / 180;
                    Vector2 sparkTarget = new Vector2(this.player.position.X + sparkRange * (float)Math.Cos(angle), this.player.position.Y + sparkRange * (float)Math.Sin(angle));
                    Vector2 sparkVelocity = sparkTarget - this.player.position;

                    int v = 3;
                    float speedMul = v / sparkVelocity.Length();
                    sparkVelocity.X = speedMul * sparkVelocity.X;
                    sparkVelocity.Y = speedMul * sparkVelocity.Y;
                    Projectile spark = Projectile.NewProjectileDirect(this.player.position, sparkVelocity, mod.ProjectileType<ElectricSpark>(), (int)Math.Ceiling(10 * this.level * 0.05f), 40, this.player.whoAmI);
                    spark.penetrate = 1;
                }
            }

            return !failed;
        }

        public override void ResetEffects()
        {
            this.player.meleeDamage = VapeConfig.DefMeleeDamage;
            this.player.magicDamage = VapeConfig.DefMagicDamage;
            this.player.rangedDamage = VapeConfig.DefRangedDamage;
            this.player.minionDamage = VapeConfig.DefMinionDamage;
            this.player.thrownDamage = VapeConfig.DefThrownDamage;

            this.player.meleeCrit = VapeConfig.DefMeleeCrit;
            this.player.magicCrit = VapeConfig.DefMagicCrit;
            this.player.rangedCrit = VapeConfig.DefRangedCrit;
            this.player.thrownCrit = VapeConfig.DefThrownCrit;

            this.player.meleeSpeed = VapeConfig.DefMeleeSpeed;
            this.dodgeChance = VapeConfig.DefDodge;
            this.blockChance = 0;

            this.rageBuff = false;
            this.energized = false;
            this.strengthened = false;

            foreach (var x in VapeRPG.BaseStats)
            {
                if (this.BaseStats.ContainsKey(x))
                {
                    this.EffectiveStats[x] = this.BaseStats[x];
                }
            }
        }

        public override TagCompound Save()
        {
            // The TagCompound which we will return
            TagCompound tc = new TagCompound();

            TagCompound baseStatsTC = new TagCompound();
            TagCompound skillLevelsTC = new TagCompound();
            TagCompound chaosBonusesTC = new TagCompound();
            TagCompound skillScrollsTC = new TagCompound();

            // Boxing values into the compounds
            foreach (var x in BaseStats)
            {
                baseStatsTC.Add(x.Key, x.Value);
            }

            foreach (var x in skillLevels)
            {
                skillLevelsTC.Add(x.Key.Name, x.Value);
            }

            foreach (var x in ChaosBonuses)
            {
                chaosBonusesTC.Add(x.Key, x.Value);
            }

            VapeRPG vapeMod = this.mod as VapeRPG;
            for(int i = 0; i < vapeMod.SkillBarUI.SkillSlots.Length; i++)
            {
                skillScrollsTC.Add(i.ToString(), vapeMod.SkillBarUI.SkillSlots[i].item);
            }

            tc.Add("BaseStats", baseStatsTC);
            tc.Add("SkillLevels", skillLevelsTC);
            tc.Add("ChaosBonuses", chaosBonusesTC);
            tc.Add("SkillScrolls", skillScrollsTC);

            tc.Add("Level", this.level);
            tc.Add("Xp", this.xp);
            tc.Add("ChaosRank", this.chaosRank);
            tc.Add("ChaosXp", this.chaosXp);

            tc.Add("StatPoints", this.statPoints);
            tc.Add("SkillPoints", this.skillPoints);
            tc.Add("ChaosPoints", this.chaosPoints);

            tc.Add("expUIPos", this.expUIPosition);
            tc.Add("skillUIPos", this.skillUIPosition);

            return tc;
        }

        private void CheckExpUIOverflow()
        {
            VapeRPG vapeMod = (this.mod as VapeRPG);

            bool expUIOverflow = false;

            if (this.expUIPosition.X >= Main.screenWidth)
            {
                this.expUIPosition.X = Main.screenWidth - vapeMod.ExpUI.Width.Pixels;
                expUIOverflow = true;
            }
            if (this.expUIPosition.Y >= Main.screenHeight)
            {
                this.expUIPosition.Y = Main.screenHeight - vapeMod.ExpUI.Height.Pixels;
                expUIOverflow = true;
            }

            if (expUIOverflow)
            {
                vapeMod.ExpUI.SetPosition(this.expUIPosition);
            }

        }

        private void UpdateChaosBonuses()
        {
            this.player.meleeDamage += this.ChaosBonuses["Melee Damage"];
            this.player.rangedDamage += this.ChaosBonuses["Ranged Damage"];
            this.player.magicDamage += this.ChaosBonuses["Magic Damage"];

            this.player.meleeCrit += (int)this.ChaosBonuses["Melee Crit"];
            this.player.rangedCrit += (int)this.ChaosBonuses["Ranged Crit"];
            this.player.magicCrit += (int)this.ChaosBonuses["Magic Crit"];

            this.player.minionDamage += this.ChaosBonuses["Minion Damage"];
            this.player.maxMinions += (int)this.ChaosBonuses["Max Minions"];
            this.player.maxTurrets += (int)Math.Floor(this.ChaosBonuses["Max Minions"] / 2);

            this.player.meleeSpeed += this.ChaosBonuses["Melee Speed"];
            this.player.maxRunSpeed += this.ChaosBonuses["Max Run Speed"] * 3;
            this.dodgeChance += this.ChaosBonuses["Dodge Chance"];
            if (this.dodgeChance > VapeConfig.MaxDodgeChance)
            {
                this.dodgeChance = VapeConfig.MaxDodgeChance;
            }
        }

        private void UpdateStatBonuses()
        {
            this.player.statLifeMax = (int)(100 + (this.level * VapeConfig.LifePerLevel) + this.EffectiveStats["Vitality"] * VapeConfig.LifePerVitality + this.EffectiveStats["Strength"] / 2);
            this.player.statManaMax = (int)(20 + this.level * VapeConfig.ManaPerLevel + this.EffectiveStats["Magic power"] * VapeConfig.ManaPerMagicPower);
            this.player.statDefense += (int)(this.EffectiveStats["Vitality"] / VapeConfig.VitalityPerDefense);

            this.player.meleeDamage += this.EffectiveStats["Strength"] / VapeConfig.MeleeDamageDivider;
            this.player.magicDamage += this.EffectiveStats["Magic power"] / VapeConfig.MagicDamageDivider + this.EffectiveStats["Spirit"] / VapeConfig.MagicDamageBySpiritDivider;
            this.player.rangedDamage += this.EffectiveStats["Dexterity"] / VapeConfig.RangedDamageDivider;

            this.player.meleeCrit += (int)(this.EffectiveStats["Strength"] / VapeConfig.MeleeCritDivider);
            this.player.magicCrit += (int)(this.EffectiveStats["Magic power"] / VapeConfig.MagicCritDivider);
            this.player.rangedCrit += (int)(this.EffectiveStats["Dexterity"] / VapeConfig.RangedCritDivider);

            this.player.minionDamage += this.EffectiveStats["Spirit"] / VapeConfig.MinionDamageDivider;
            this.player.maxMinions += (int)(this.EffectiveStats["Spirit"] / VapeConfig.SpiritPerMaxMinion);
            this.player.maxTurrets += (int)(this.EffectiveStats["Spirit"] / VapeConfig.SpiritPerMaxTurret);

            this.player.meleeSpeed += this.EffectiveStats["Haste"] / VapeConfig.MeleeSpeedDivider;
            this.player.moveSpeed += this.EffectiveStats["Haste"] / VapeConfig.MoveSpeedDivider;

            this.dodgeChance += this.EffectiveStats["Haste"] / VapeConfig.DodgeDivider;

            this.UpdateChaosBonuses();

            foreach (Skill skill in VapeRPG.Skills)
            {
                skill.UpdateStats(this);
            }
        }

        #region Forwarding events to skills
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if(this.HasSkill(skill.GetType()))
                {
                    skill.OnHitNPC(this, null, proj, target, damage, knockback, crit);
                }
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if (this.HasSkill(skill.GetType()))
                {
                    skill.ModifyHitByNPC(this, npc, ref damage, ref crit);
                }
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if (this.HasSkill(skill.GetType()))
                {
                    skill.ModifyHitByProjectile(this, proj, ref damage, ref crit);
                }
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if (this.HasSkill(skill.GetType()))
                {
                    skill.ModifyHitNPC(this, item, null, target, ref damage, ref knockback, ref crit);
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if (this.HasSkill(skill.GetType()))
                {
                    skill.OnHitNPC(this, item, null, target, damage, knockback, crit);
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if (this.HasSkill(skill.GetType()))
                {
                    skill.ModifyHitNPC(this, null, proj, target, ref damage, ref knockback, ref crit);
                }
            }
        }

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if (this.HasSkill(skill.GetType()))
                {
                    skill.Shoot(this, item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
                }
            }
            return true;
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                if (this.HasSkill(skill.GetType()))
                {
                    skill.Hurt(this, pvp, quiet, damage, hitDirection, crit);
                }
            }
        }
        #endregion
    }
}
