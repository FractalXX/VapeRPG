using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.Exceptions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;

namespace VapeRPG
{
    enum ItemQuality { Unique = -1, Common, Uncommon, Rare, Epic }

    class VapeGlobalItem : GlobalItem
    {
        private static Random rnd = new Random();

        /// <summary>
        /// The quality of the item.
        /// </summary>
        public ItemQuality quality;

        /// <summary>
        /// Stores the stat bonuses this item gives for the player.
        /// </summary>
        public Dictionary<string, int> statBonus;

        /// <summary>
        /// Returns true if the item was already qualified.
        /// </summary>
        public bool wasQualified;

        /// <summary>
        /// The block chance bonus this item gives for the player.
        /// </summary>
        public float blockChance;

        public bool tooltipVisible;

        // Quality Colors
        private static Color uncommonColor = Color.LimeGreen;
        private static Color rareColor = Color.Blue;
        private static Color epicColor = Color.BlueViolet;
        private static Color uniqueColor = Color.SkyBlue;

        // Stat pairs for unique items
        private static string[,] uniqueStatPairs =
        {
            {"Strength", "Haste" },
            {"Dexterity", "Haste" },
            {"Spirit", "Magic Power" }
        };

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public VapeGlobalItem()
        {
            this.statBonus = new Dictionary<string, int>();
            this.wasQualified = false;
            this.blockChance = 0;
            this.quality = ItemQuality.Common;
            this.tooltipVisible = false;
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            VapeGlobalItem global = (VapeGlobalItem)base.Clone(item, itemClone);
            global.quality = this.quality;
            global.wasQualified = this.wasQualified;
            global.statBonus = new Dictionary<string, int>();
            global.blockChance = this.blockChance;
            global.tooltipVisible = this.tooltipVisible;

            foreach (var x in this.statBonus)
            {
                global.statBonus[x.Key] = x.Value;
            }

            return global;
        }

        public override void SetDefaults(Item item)
        {

            switch (item.type)
            {
                case ItemID.EoCShield:
                    this.blockChance = 0.15f;
                    break;

                case ItemID.CobaltShield:
                    this.blockChance = 0.06f;
                    break;

                case ItemID.ObsidianShield:
                    this.blockChance = 0.08f;
                    break;

                case ItemID.AnkhShield:
                    this.blockChance = 0.12f;
                    break;
            }

            if (!this.wasQualified)
            {
                this.quality = ItemQuality.Common;

                if (item.accessory || item.defense > 0)
                {
                    int chance = rnd.Next(0, 100);

                    if (chance <= 3)
                    {
                        this.Qualify(item, ItemQuality.Epic);
                    }
                    else if (chance <= 10)
                    {
                        this.Qualify(item, ItemQuality.Rare);
                    }
                    else if (chance <= 20)
                    {
                        this.Qualify(item, ItemQuality.Uncommon);
                    }
                }

                this.wasQualified = true;
            }
        }

        public override void OnCraft(Item item, Recipe recipe)
        {
            this.quality = ItemQuality.Common;
            this.statBonus.Clear();
            this.tooltipVisible = true;

            if (IsQualifiable(item))
            {
                int chance = rnd.Next(0, 100);

                if (chance <= 5)
                {
                    this.Qualify(item, ItemQuality.Epic);
                }
                else if (chance <= 15)
                {
                    this.Qualify(item, ItemQuality.Rare);
                }
                else if (chance <= 35)
                {
                    this.Qualify(item, ItemQuality.Uncommon);
                }
            }

            this.wasQualified = true;
        }

        private static bool IsQualifiable(Item item)
        {
            return item.accessory || item.defense > 0;
        }

        public void Qualify(Item item, ItemQuality newQuality)
        {
            this.quality = newQuality;
            this.GenerateStatBonuses(item, newQuality);
        }

        private void GenerateStatBonuses(Item item, ItemQuality newQuality)
        {
            if (newQuality != ItemQuality.Common)
            {
                if (newQuality == ItemQuality.Unique)
                {
                    this.statBonus.Clear();

                    int statMin = (item.rare + 1) * 6;
                    int statMax = (item.rare + 1) * 9;
                    int statPairIndex = rnd.Next(0, uniqueStatPairs.GetLength(0));

                    // Fargo fix
                    if (statMin < 0) statMin *= -1;
                    if (statMax < 0) statMax *= -1;

                    this.statBonus[uniqueStatPairs[statPairIndex, 0]] = rnd.Next(statMin, statMax);
                    this.statBonus[uniqueStatPairs[statPairIndex, 1]] = rnd.Next(statMin, statMax);
                    this.statBonus["Vitality"] = rnd.Next(statMin, statMax);
                }
                else
                {
                    int statNumChance = rnd.Next(0, 100);

                    for (int i = 0; i < (int)newQuality; i++)
                    {
                        int stat = 0;
                        do
                        {
                            stat = rnd.Next(0, VapeRPG.BaseStats.Length);
                        }
                        while (this.statBonus.ContainsKey(VapeRPG.BaseStats[stat]));

                        int statMin = (item.rare + 1) * 3;
                        int statMax = (item.rare + 1 + ((int)newQuality - 1)) * 6;

                        // Fargo fix
                        if (statMin < 0) statMin *= -1;
                        if (statMax < 0) statMax *= -1;

                        this.statBonus[VapeRPG.BaseStats[stat]] = rnd.Next(statMin, statMax);
                        int newstat = 0;
                        if (statNumChance <= 20)
                        {
                            while (newstat == stat)
                            {
                                newstat = rnd.Next(0, VapeRPG.BaseStats.Length);
                            }
                            this.statBonus[VapeRPG.BaseStats[newstat]] = rnd.Next(statMin, statMax);
                        }
                        else if (statNumChance <= 5)
                        {
                            int newstat2 = 0;
                            while (newstat2 == stat || newstat2 == newstat)
                            {
                                newstat2 = rnd.Next(0, VapeRPG.BaseStats.Length);
                            }
                            this.statBonus[VapeRPG.BaseStats[newstat2]] = rnd.Next(statMin, statMax);
                        }
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if(this.tooltipVisible)
            {
                if (this.quality != ItemQuality.Common)
                {
                    TooltipLine itemQuality = new TooltipLine(this.mod, "Quality", this.quality.ToString());
                    Color qualityColor = Color.White;
                    switch (this.quality)
                    {
                        case ItemQuality.Uncommon:
                            qualityColor = uncommonColor;
                            break;

                        case ItemQuality.Rare:
                            qualityColor = rareColor;
                            break;

                        case ItemQuality.Epic:
                            qualityColor = epicColor;
                            break;

                        case ItemQuality.Unique:
                            qualityColor = uniqueColor;
                            break;
                    }

                    itemQuality.overrideColor = qualityColor;
                    tooltips.Add(itemQuality);
                }

                foreach (var x in this.statBonus)
                {
                    if (x.Value > 0)
                    {
                        TooltipLine bonus = new TooltipLine(this.mod, x.Key, String.Format("+{0} {1}", x.Value, x.Key));
                        bonus.overrideColor = Color.Yellow;
                        tooltips.Add(bonus);
                    }
                }

                if (this.blockChance > 0)
                {
                    TooltipLine bonus = new TooltipLine(this.mod, "Block Chance", String.Format("{0} Block Chance", this.blockChance));
                    bonus.overrideColor = Color.White;
                    tooltips.Add(bonus);
                }
            }
        }

        public override bool OnPickup(Item item, Player player)
        {
            this.tooltipVisible = true;
            return base.OnPickup(item, player);
        }

        public override bool NeedsSaving(Item item)
        {
            return true;
        }

        public override TagCompound Save(Item item)
        {
            TagCompound itemTC = new TagCompound();
            TagCompound statBonusTC = new TagCompound();

            foreach (var x in this.statBonus)
            {
                statBonusTC.Add(x.Key, x.Value);
            }

            itemTC.Add("Quality", Convert.ToString(this.quality));
            itemTC.Add("StatBonuses", statBonusTC);
            itemTC.Add("WasQualified", this.wasQualified);

            return itemTC;
        }

        public override void Load(Item item, TagCompound tag)
        {
            TagCompound statBonusTC = tag.GetCompound("StatBonuses");
            this.wasQualified = tag.GetBool("WasQualified");
            this.statBonus.Clear();

            if (this.wasQualified)
            {
                this.quality = (ItemQuality)Enum.Parse(ItemQuality.Common.GetType(), tag.GetString("Quality"));
                foreach (var x in statBonusTC)
                {
                    // Fix for potential error due to removing Intellect from stats
                    if(x.Key != "Intellect")
                    {
                        this.statBonus.Add(x.Key, (int)x.Value);
                    }
                }

                if (this.quality == ItemQuality.Unique)
                {
                    item.expert = true;
                }
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if(item.type == ItemID.LifeCrystal || item.type == ItemID.ManaCrystal || item.type == ItemID.LifeFruit)
            {
                return false;
            }
            return true;
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if(IsQualifiable(item))
            {
                int dustType = 63;
                switch (this.quality)
                {
                    case ItemQuality.Uncommon:
                        dustType = 61;
                        break;

                    case ItemQuality.Rare:
                        dustType = 59;
                        break;

                    case ItemQuality.Epic:
                        dustType = 62;
                        break;

                    case ItemQuality.Unique:
                        dustType = 64;
                        break;
                }

                if(VapeConfig.VapeLootRariyRings)
                {
                    int dustCount = 360;
                    for (int i = 0; i < dustCount; i += 2)
                    {
                        double angle = i * Math.PI / 180;
                        Vector2 dustPosition = new Vector2(item.position.X + item.width / 2 + item.width * (float)Math.Cos(angle), item.position.Y + item.height / 2 + item.height * (float)Math.Sin(angle));

                        Dust dust = Dust.NewDustPerfect(dustPosition, dustType, Vector2.Zero);
                        dust.noGravity = true;
                    }
                }
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            this.tooltipVisible = true;
        }

        public override void UpdateEquip(Item item, Player player)
        {
            VapePlayer vp = player.GetModPlayer<VapePlayer>();
            this.tooltipVisible = true;

            vp.blockChance += this.blockChance;

            if (this.statBonus.Count > 0)
            {
                foreach (var x in this.statBonus)
                {
                    vp.EffectiveStats[x.Key] += x.Value;
                }
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            return base.AltFunctionUse(item, player);
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            // TODO: Draw colored frame for each quality
        }

        public override bool NewPreReforge(Item item)
        {
            return this.quality == ItemQuality.Common;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((int)this.quality);
            writer.Write(this.wasQualified);
            writer.Write(this.statBonus.Count);

            foreach (var x in this.statBonus)
            {
                writer.Write(String.Format("{0}:{1}", x.Key, x.Value));
            }

            base.NetSend(item, writer);
        }

        public override bool UseItem(Item item, Player player)
        {
            SkillController.UseItem(player.GetModPlayer<VapePlayer>(), item);
            return base.UseItem(item, player);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            this.quality = (ItemQuality)reader.ReadInt32();
            this.wasQualified = reader.ReadBoolean();
            int statCount = reader.ReadInt32();

            this.statBonus.Clear();

            for (int i = 0; i < statCount; i++)
            {
                string[] keyValuePair = reader.ReadString().Split(':');
                this.statBonus[keyValuePair[0]] = int.Parse(keyValuePair[1]);
            }

            base.NetReceive(item, reader);
        }
    }
}
