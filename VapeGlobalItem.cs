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

        public ItemQuality quality;
        public Dictionary<string, int> statBonus;
        public bool wasQualified;
        public Item parent;

        private static Color uncommonColor = Color.LimeGreen;
        private static Color rareColor = Color.Blue;
        private static Color epicColor = Color.BlueViolet;

        private static Hashtable reforgeBuffer = new Hashtable();

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
            VapeGlobalItem global = item.GetGlobalItem<VapeGlobalItem>();
            global.parent = item;

            if (!global.wasQualified)
            {
                global.quality = ItemQuality.Common;
                global.statBonus = new Dictionary<string, int>();

                if (item.accessory || item.defense > 0)
                {
                    int chance = rnd.Next(0, 100);

                    if (chance <= 10)
                    {
                        global.Qualify(ItemQuality.Epic);
                    }
                    else if (chance <= 30)
                    {
                        global.Qualify(ItemQuality.Rare);
                    }
                    else if (chance <= 60)
                    {
                        global.Qualify(ItemQuality.Uncommon);
                    }
                }

                global.wasQualified = true;
            }
        }

        public void Qualify(ItemQuality newQuality)
        {
            if(newQuality == ItemQuality.Unique)
            {

            }
            else
            {
                this.quality = newQuality;
                this.GenerateStatBonuses(newQuality);
            }
        }

        private void GenerateStatBonuses(ItemQuality newQuality)
        {
            if (newQuality != ItemQuality.Common)
            {
                int statNumChance = rnd.Next(0, 100);

                for (int i = 0; i < (int)newQuality; i++)
                {
                    int stat = 0;
                    do
                    {
                        stat = rnd.Next(0, VapeRPG.baseStats.Length);
                    }
                    while (this.statBonus.ContainsKey(VapeRPG.baseStats[stat]));

                    int statMin = (i + this.parent.rare) * 5;
                    int statMax = (i + this.parent.rare + ((int)newQuality - 1)) * 10;

                    this.statBonus[VapeRPG.baseStats[stat]] = rnd.Next(statMin, statMax);
                    int newstat = 0;
                    if (statNumChance <= 20)
                    {
                        while (newstat == stat)
                        {
                            newstat = rnd.Next(0, VapeRPG.baseStats.Length);
                        }
                        this.statBonus[VapeRPG.baseStats[newstat]] = rnd.Next(statMin, statMax);
                    }
                    else if (statNumChance <= 5)
                    {
                        int newstat2 = 0;
                        while (newstat2 == stat || newstat2 == newstat)
                        {
                            newstat2 = rnd.Next(0, VapeRPG.baseStats.Length);
                        }
                        this.statBonus[VapeRPG.baseStats[newstat2]] = rnd.Next(statMin, statMax);
                    }
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            VapeGlobalItem global = item.GetGlobalItem<VapeGlobalItem>();
            TooltipLine itemQuality = new TooltipLine(global.mod, "Quality", global.quality.ToString());
            Color qualityColor = Color.White;
            switch (global.quality)
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
            }

            itemQuality.overrideColor = qualityColor;
            tooltips.Add(itemQuality);

            foreach (var x in global.statBonus)
            {
                TooltipLine bonus = new TooltipLine(global.mod, x.Key, String.Format("+{0} {1}", x.Value, x.Key));
                bonus.overrideColor = Color.Yellow;
                tooltips.Add(bonus);
            }
        }


        public override bool NeedsSaving(Item item)
        {
            return true;
        }

        public override TagCompound Save(Item item)
        {
            VapeGlobalItem global = item.GetGlobalItem<VapeGlobalItem>();
            TagCompound itemTC = new TagCompound();
            TagCompound statBonusTC = new TagCompound();

            foreach (var x in global.statBonus)
            {
                statBonusTC.Add(x.Key, x.Value);
            }

            itemTC.Add("Quality", Convert.ToString(global.quality));
            itemTC.Add("StatBonuses", statBonusTC);
            itemTC.Add("WasQualified", global.wasQualified);

            return itemTC;
        }

        public override void Load(Item item, TagCompound tag)
        {
            VapeGlobalItem global = item.GetGlobalItem<VapeGlobalItem>();
            TagCompound statBonusTC = tag.GetCompound("StatBonuses");
            global.statBonus = new Dictionary<string, int>();
            global.parent = item;
            global.wasQualified = tag.GetBool("WasQualified");

            if (global.wasQualified)
            {
                global.quality = (ItemQuality)Enum.Parse(ItemQuality.Common.GetType(), tag.GetString("Quality"));
                foreach (var x in statBonusTC)
                {
                    global.statBonus.Add(x.Key, (int)x.Value);
                }
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            VapeGlobalItem global = item.GetGlobalItem<VapeGlobalItem>();
            VapePlayer vp = player.GetModPlayer<VapePlayer>();
            if (global.statBonus.Count > 0)
            {
                foreach (var x in global.statBonus)
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
            VapeGlobalItem global = item.GetGlobalItem<VapeGlobalItem>();

            Hashtable itemProperties = new Hashtable();
            itemProperties.Add("StatBonus", global.statBonus);
            itemProperties.Add("Quality", global.quality);
            itemProperties.Add("WasQualified", global.wasQualified);
            itemProperties.Add("Parent", global.parent);

            reforgeBuffer.Add(item, itemProperties);
        }

        public override void PostReforge(Item item)
        {
            VapeGlobalItem global = item.GetGlobalItem<VapeGlobalItem>();

            Hashtable itemProperties = (Hashtable)reforgeBuffer[item];
            global.statBonus = (Dictionary<string, int>)itemProperties["StatBonus"];
            global.quality = (ItemQuality)itemProperties["Quality"];
            global.wasQualified = (bool)itemProperties["WasQualified"];
            global.parent = (Item)itemProperties["Parent"];

            reforgeBuffer.Remove(item);
        }
    }
}
