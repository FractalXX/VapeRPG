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

        public ItemQuality quality; // The quality of the item
        public Dictionary<string, int> statBonus; // To store the statbonuses for this item
        public bool wasQualified; // To prevent the item from being re qualified

        public float blockChance;

        // Quality Colors
        private static Color uncommonColor = Color.LimeGreen;
        private static Color rareColor = Color.Blue;
        private static Color epicColor = Color.BlueViolet;
        private static Color uniqueColor = Color.SkyBlue;

        private static Hashtable reforgeBuffer = new Hashtable(); // For storing, then loading the item's properties when it's reforged

        // Stat paris for unique items
        private static string[,] uniqueStatPairs =
        {
            {"Strength", "Agility" },
            {"Dexterity", "Agility" },
            {"Intellect", "Magic Power" }
        };

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void SetDefaults(Item item)
        {
            switch(item.type)
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
                this.statBonus = new Dictionary<string, int>();

                if (item.accessory || item.defense > 0)
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

                    int statMin = (item.rare + 1) * 10;
                    int statMax = (item.rare + 1) * 15;
                    int statPairIndex = rnd.Next(0, uniqueStatPairs.GetLength(0));

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

                        int statMin = (item.rare + 1) * 5;
                        int statMax = (item.rare + 1 + ((int)newQuality - 1)) * 10;

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
                if(x.Value > 0)
                {
                    TooltipLine bonus = new TooltipLine(this.mod, x.Key, String.Format("+{0} {1}", x.Value, x.Key));
                    bonus.overrideColor = Color.Yellow;
                    tooltips.Add(bonus);
                }
            }
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
            this.statBonus = new Dictionary<string, int>();
            this.wasQualified = tag.GetBool("WasQualified");

            if (this.wasQualified)
            {
                this.quality = (ItemQuality)Enum.Parse(ItemQuality.Common.GetType(), tag.GetString("Quality"));
                foreach (var x in statBonusTC)
                {
                    this.statBonus.Add(x.Key, (int)x.Value);
                }

                if (this.quality == ItemQuality.Unique)
                {
                    item.expert = true;
                }
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            VapePlayer vp = player.GetModPlayer<VapePlayer>();
            if (this.statBonus.Count > 0)
            {
                foreach (var x in this.statBonus)
                {
                    vp.EffectiveStats[x.Key] += x.Value;
                }
            }
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            /*Color frameColor = Color.White;
            switch (item.GetGlobalItem<VapeGlobalItem>().quality)
            {
                case ItemQuality.Uncommon:
                    frameColor = uncommonColor;
                    break;

                case ItemQuality.Rare:
                    frameColor = rareColor;
                    break;

                case ItemQuality.Epic:
                    frameColor = epicColor;
                    break;
            }
            spriteBatch.Draw(VapeRPG.itemQualityFrame, Vector2.Zero, frame, frameColor);*/
        }

        public override void PreReforge(Item item)
        {
            Hashtable itemProperties = new Hashtable();
            itemProperties.Add("StatBonus", this.statBonus);
            itemProperties.Add("Quality", this.quality);
            itemProperties.Add("WasQualified", this.wasQualified);

            reforgeBuffer.Add(item, itemProperties);
        }

        public override void PostReforge(Item item)
        {

            Hashtable itemProperties = (Hashtable)reforgeBuffer[item];
            this.statBonus = (Dictionary<string, int>)itemProperties["StatBonus"];
            this.quality = (ItemQuality)itemProperties["Quality"];
            this.wasQualified = (bool)itemProperties["WasQualified"];

            reforgeBuffer.Remove(item);
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((int)this.quality);
            writer.Write(this.wasQualified);

            foreach (var x in VapeRPG.BaseStats)
            {
                int value;
                this.statBonus.TryGetValue(x, out value);
                writer.Write(String.Format("{0} {1}", x, value));
            }

            base.NetSend(item, writer);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            this.quality = (ItemQuality)reader.ReadInt32();
            this.wasQualified = reader.ReadBoolean();

            for (int i = 0; i < VapeRPG.BaseStats.Length; i++)
            {
                string[] keyValuePair = reader.ReadString().Split(' ');
                int value = int.Parse(keyValuePair[1]);
                if(value > 0)
                {
                    this.statBonus[keyValuePair[0]] = int.Parse(keyValuePair[1]);
                }
            }

            base.NetReceive(item, reader);
        }
    }
}
