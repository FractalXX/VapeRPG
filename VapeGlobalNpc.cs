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
        /// Returns true if the mob is a chaos mob.
        /// </summary>
        public bool isChaos = false;

        /// <summary>
        /// Determines the stat scale of the NPC if it's a chaos mob.
        /// </summary>
        public int chaosMultiplier = 1;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public void ChaosTransform(NPC npc)
        {
            this.chaosMultiplier = rnd.Next(ModContent.GetInstance<VapeConfig>().MinChaosMultiplier, ModContent.GetInstance<VapeConfig>().MaxChaosMultiplier);

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
            if (!ModContent.GetInstance<VapeConfig>().IsIgnoredType(npc) && !npc.SpawnedFromStatue && !npc.friendly)
            {
                double gainedXp;
                if (npc.boss)
                {
                    foreach (Player player in Main.player.ToList().FindAll(x => x.active))
                    {
                        VapePlayer vp = player.GetModPlayer<VapePlayer>();
                        gainedXp = npc.lifeMax / 2;
                        vp.GainExperience((int)gainedXp);
                    }
                }
                else if(npc.lifeMax >= 10)
                {
                    // If it isn't a boss, only nearby players gain experience based on the npc's HP
                    foreach (Player player in Main.player.ToList().FindAll(x => x.active))
                    {
                        if (Vector2.Distance(player.position, npc.position) <= ModContent.GetInstance<VapeConfig>().ExperienceGainDistance)
                        {
                            VapePlayer vp = player.GetModPlayer<VapePlayer>();
                            if(ModContent.GetInstance<VapeConfig>().VanillaXpTable.ContainsKey(npc.type))
                            {
                                gainedXp = ModContent.GetInstance<VapeConfig>().VanillaXpTable[npc.type];
                            }
                            else
                            {
                                gainedXp = ModContent.GetInstance<VapeConfig>().GlobalXpMultiplier * Math.Pow(2, Math.Sqrt((2 * (1 + npc.defDamage / (2 * npc.lifeMax)) * npc.lifeMax) / Math.Pow(npc.lifeMax, 1 / 2.6)));
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
            // Fix for incompatibility with other mods such as Calamity, etc.
            if (npc != null && Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.SinglePlayer)
            {
                if (!npc.boss && npc.lifeMax >= 40 && !npc.SpawnedFromStatue && !npc.friendly && !ModContent.GetInstance<VapeConfig>().IsIgnoredTypeChaos(npc) && rnd.Next(0, 101) <= ModContent.GetInstance<VapeConfig>().ChaosChance)
                {
                    ChaosTransform(npc);
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if(type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<VapersGlobe>());
                nextSlot++;
            }
            base.SetupShop(type, shop, ref nextSlot);
        }
    }
}
