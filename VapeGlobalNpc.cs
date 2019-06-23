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
        /// The override color for chaos mobs.
        /// </summary>
        public static Color ChaosColor = new Color(179, 104, 255, 127);

        private static Random rnd = new Random();

        /// <summary>
        /// Returns true if this NPC is a chaos NPC.
        /// </summary>
        public bool isChaos = false;

        /// <summary>
        /// Determines the stat scale of the NPC if it's a chaos NPC.
        /// </summary>
        public int chaosMultiplier = 1;

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

        public void ChaosTransform(NPC npc)
        {
            this.chaosMultiplier = rnd.Next(VapeConfig.MinChaosMultiplier, VapeConfig.MaxChaosMultiplier);

            npc.scale *= this.chaosMultiplier / 2.7f;
            npc.lifeMax *= this.chaosMultiplier;
            npc.life = npc.lifeMax;
            npc.defDamage *= this.chaosMultiplier;
            npc.defDefense *= this.chaosMultiplier / 2;
            npc.color = ChaosColor;
            npc.stepSpeed *= this.chaosMultiplier / 2f;

            this.isChaos = true;

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, NetworkText.Empty, npc.whoAmI);

                ModPacket packet = this.mod.GetPacket();
                packet.Write((byte)VapeRPGMessageType.ServerTransformChaosNPC);
                packet.Write(this.chaosMultiplier);
                packet.Write(npc.whoAmI);
                packet.Send();
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (!VapeConfig.IsIgnoredType(npc) && !npc.SpawnedFromStatue && !npc.friendly)
            {
                if (npc.lifeMax >= 10)
                {
                    double gainedXp;
                    foreach (VapePlayer vp in this.attackingPlayers)
                    {
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
                        if (this.isChaos)
                        {
                            vp.GainExperience(1 + (int)(gainedXp / 3), true);
                            gainedXp *= (2 - 1 / this.chaosMultiplier);
                        }
                        vp.GainExperience((int)gainedXp);
                    }
                }
            }
            return base.CheckDead(npc);
        }

        public override void ResetEffects(NPC npc)
        {
            if (this.isChaos)
            {
                npc.GivenName = String.Format("Chaos {0}", npc.TypeName);
            }
        }

        public override void SetDefaults(NPC npc)
        {
            // TODO: Extract Chaos NPC feature to a separate mod
            // Fix for incompatibility with other mods such as Calamity, etc.
            /*if (npc != null && Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.SinglePlayer)
            {
                if (!npc.boss && npc.lifeMax >= 40 && !npc.SpawnedFromStatue && !npc.friendly && !VapeConfig.IsIgnoredTypeChaos(npc) && rnd.Next(0, 101) <= VapeConfig.ChaosChance)
                {
                    ChaosTransform(npc);
                }
            }*/
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
