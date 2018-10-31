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
    class VapeGlobalItem : GlobalItem
    {
        private static Random rnd = new Random();

        // Stat pairs for unique items
        private static string[,] uniqueStatPairs =
        {
            {"Strength", "Haste" },
            {"Dexterity", "Haste" },
            {"Spirit", "Magic Power" }
        };

        /// <summary>
        /// The block chance bonus this item gives for the player.
        /// </summary>
        public float blockChance;

        public VapeGlobalItem()
        {
            this.blockChance = 0;
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        private static bool IsQualifiable(Item item)
        {
            return item.accessory || item.defense > 0;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.LifeCrystal || item.type == ItemID.ManaCrystal || item.type == ItemID.LifeFruit)
            {
                return false;
            }
            return true;
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            VapeGlobalItem global = (VapeGlobalItem)base.Clone(item, itemClone);
            global.blockChance = this.blockChance;

            return global;
        }

        public override void Load(Item item, TagCompound tag)
        {

        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (this.blockChance > 0)
            {
                TooltipLine bonus = new TooltipLine(this.mod, "Block Chance", String.Format("{0} Block Chance", this.blockChance));
                bonus.overrideColor = Color.White;
                tooltips.Add(bonus);
            }
        }

        public override bool NeedsSaving(Item item)
        {
            return true;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            base.NetSend(item, writer);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            base.NetReceive(item, reader);
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
        }

        public override TagCompound Save(Item item)
        {
            TagCompound itemTC = new TagCompound();
            /*TagCompound statBonusTC = new TagCompound();

            foreach (var x in this.statBonus)
            {
                statBonusTC.Add(x.Key, x.Value);
            }

            itemTC.Add("Quality", Convert.ToString(this.quality));
            itemTC.Add("StatBonuses", statBonusTC);
            itemTC.Add("WasQualified", this.wasQualified);*/

            return itemTC;
        }

        public override bool UseItem(Item item, Player player)
        {
            foreach (Skill skill in VapeRPG.Skills)
            {
                skill.UseItem(player.GetModPlayer<VapePlayer>(), item);
            }
            return base.UseItem(item, player);
        }
    }
}
