using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Items
{
    public abstract class ScrollBase : ModItem
    {
        public int cooldown;
        public int cooldownRemaining;
        public int damage;

        public override ModItem Clone(Item item)
        {
            ScrollBase clone =  base.Clone(item) as ScrollBase;
            clone.cooldown = this.cooldown;
            clone.cooldownRemaining = this.cooldownRemaining;
            clone.damage = this.damage;
            return clone;
        }

        /*public override void NetSend(BinaryWriter writer)
        {
            writer.Write(this.cooldownRemaining);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            this.cooldownRemaining = reader.ReadByte();
        }*/

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (this.cooldownRemaining > 0)
            {
                this.cooldownRemaining--;
            }
        }

        public override void SetDefaults()
        {
            this.item.width = 32;
            this.item.height = 34;
            this.item.useStyle = 4;
            this.item.maxStack = 1;
            this.item.useAnimation = 15;
            this.item.useTime = 15;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Empty scroll");
            Tooltip.SetDefault("Made to hold a spell.");
        }

        public override bool UseItem(Player player)
        {
            return this.Use(player);
        }

        public bool Use(Player player)
        {
            if (this.cooldownRemaining <= 0)
            {
                this.CastSpell(player);
                this.cooldownRemaining = this.cooldown;
                return true;
            }
            return false;
        }

        public abstract void CastSpell(Player player);
    }
}
