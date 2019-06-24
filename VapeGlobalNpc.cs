using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

using VapeRPG.Items;

namespace VapeRPG
{
    class VapeGlobalNpc : GlobalNPC
    {
        /// <summary>
        /// Stores references to players who damaged this NPC.
        /// </summary>
        private List<VapePlayer> attackingPlayers { get; set; }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public VapeGlobalNpc()
        {
            this.attackingPlayers = new List<VapePlayer>();
        }

        public override bool CheckDead(NPC npc)
        {
            if (!VapeConfig.IsIgnoredType(npc) && !npc.SpawnedFromStatue && !npc.friendly)
            {
                if (npc.lifeMax >= 10)
                {
                    double gainedXp;
                    if (VapeConfig.VanillaXpTable.ContainsKey(npc.type))
                    {
                        gainedXp = VapeConfig.VanillaXpTable[npc.type];
                    }
                    else
                    {
                        gainedXp = VapeConfig.FinalMultiplierForXpGain * Math.Pow(2, Math.Sqrt((2 * (1 + npc.defDamage / (2 * npc.lifeMax)) * npc.lifeMax) / Math.Pow(npc.lifeMax, 1 / 2.6)));
                        if (npc.lifeMax >= 1000)
                        {
                            gainedXp = npc.lifeMax / 2;
                        }
                        else if (npc.lifeMax <= 20)
                        {
                            gainedXp /= 2;
                        }
                    }
                    foreach (VapePlayer vp in this.attackingPlayers)
                    {
                        vp.GainExperience((int)gainedXp);
                    }
                }
            }
            return base.CheckDead(npc);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType<VapersGlobe>());
                nextSlot++;
            }
            base.SetupShop(type, shop, ref nextSlot);
        }

        public void SubscribePlayer(VapePlayer player)
        {
            if (!this.attackingPlayers.Contains(player))
            {
                this.attackingPlayers.Add(player);
            }
        }
    }
}
