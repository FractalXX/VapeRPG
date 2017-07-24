using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

namespace VapeRPG
{
    class VapeGlobalNpc : GlobalNPC
    {
        private const int expGainDistance = 3000;

        private static int chaosChance = 20;

        public bool hemorrhage = false;
        public int hemorrhageDamage = 0;

        public bool isChaos = false;

        private int chaosMultiplier = 1;

        private static int[] ignoredTypes =
        {
            NPCID.DungeonGuardian,
            NPCID.Bunny,
            NPCID.BunnySlimed,
            NPCID.BunnyXmas,
            NPCID.GoldBunny,
            NPCID.PartyBunny,
            NPCID.Penguin,
            NPCID.PenguinBlack,
            NPCID.Parrot,
            NPCID.Bird,
            NPCID.GoldBird,
            NPCID.ScorpionBlack,
            NPCID.Buggy,
            NPCID.Duck,
            NPCID.Duck2,
            NPCID.DuckWhite,
            NPCID.DuckWhite2,
            NPCID.Frog,
            NPCID.GoldFrog,
            NPCID.Worm,
            NPCID.GoldWorm,
            NPCID.TruffleWorm,
            NPCID.Goldfish,
            NPCID.GoldfishWalker,
            NPCID.Grasshopper,
            NPCID.GoldGrasshopper,
            NPCID.LightningBug,
            NPCID.Mouse,
            NPCID.GoldMouse,
            NPCID.Squirrel,
            NPCID.SquirrelGold,
            NPCID.SquirrelRed,
            NPCID.Scorpion,
            NPCID.Sluggy,
            NPCID.Snail,
            NPCID.GlowingSnail,
            NPCID.SeaSnail,
            NPCID.Butterfly,
            NPCID.GoldButterfly,
            NPCID.Firefly
        };

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void SetDefaults(NPC npc)
        {
            if (!npc.boss && !npc.SpawnedFromStatue && !npc.friendly && !IsIgnoredType(npc.type) && Main.rand.Next(0, 101) <= chaosChance)
            {
                ChaosTransform(npc);
            }
        }

        public override bool CheckDead(NPC npc)
        {
            VapeGlobalNpc global = npc.GetGlobalNPC<VapeGlobalNpc>();

            if (!IsIgnoredType(npc.type) && !npc.SpawnedFromStatue && !npc.friendly)
            {
                double gainedXp;
                if (npc.boss)
                {
                    foreach (Player player in Main.player)
                    {
                        if (player.active)
                        {
                            VapePlayer vp = player.GetModPlayer<VapePlayer>();
                            gainedXp = npc.lifeMax * (1 + (Math.Abs(npc.defDefense) + 1) / npc.defDamage);
                            vp.GainExperience((int)gainedXp);
                        }
                    }
                }
                else
                {
                    // If it isn't a boss, only nearby players gain experience based on the npc's HP
                    foreach (Player player in Main.player)
                    {
                        if (Vector2.Distance(player.position, npc.position) <= expGainDistance)
                        {
                            VapePlayer vp = player.GetModPlayer<VapePlayer>();
                            gainedXp = Math.Pow(2, Math.Sqrt((2 * (1 + npc.defDamage / (2 * npc.lifeMax)) * npc.lifeMax) / Math.Pow(npc.lifeMax, 1 / 2.6)));
                            if (npc.lifeMax <= 20)
                            {
                                gainedXp /= 2;
                            }
                            if (global.isChaos)
                            {
                                gainedXp *= global.chaosMultiplier / 2f;
                                vp.GainChaosExperience((int)(gainedXp / 3));
                            }
                            vp.GainExperience((int)gainedXp);
                        }
                    }
                }
            }
            return base.CheckDead(npc);
        }

        public override void NPCLoot(NPC npc)
        {
            VapeGlobalNpc global = npc.GetGlobalNPC<VapeGlobalNpc>();

            #region UniqueDrops

            int chance;
            int spawnID;
            int itemID;

            if (npc.type == NPCID.EyeofCthulhu)
            {
                chance = Main.rand.Next(0, 3);

                if (chance == 0)
                {
                    spawnID = ItemID.GoldChainmail;
                }
                else if (chance == 1)
                {
                    spawnID = ItemID.GoldHelmet;
                }
                else
                {
                    spawnID = ItemID.GoldGreaves;
                }

                itemID = Item.NewItem(npc.position, spawnID);
                Main.item[itemID].GetGlobalItem<VapeGlobalItem>().Qualify(ItemQuality.Unique);
            }

            if (npc.type == NPCID.SkeletronHead)
            {

                chance = Main.rand.Next(0, 9);

                if (chance == 0)
                {
                    spawnID = ItemID.AncientShadowScalemail;
                }
                else if (chance == 1)
                {
                    spawnID = ItemID.AncientShadowHelmet;
                }
                else if (chance == 2)
                {
                    spawnID = ItemID.AncientShadowGreaves;
                }
                else if (chance == 3)
                {
                    spawnID = ItemID.AncientCobaltBreastplate;
                }
                else if (chance == 4)
                {
                    spawnID = ItemID.AncientCobaltHelmet;
                }
                else if (chance == 5)
                {
                    spawnID = ItemID.AncientCobaltLeggings;
                }
                else if (chance == 6)
                {
                    spawnID = ItemID.NecroBreastplate;
                }
                else if (chance == 7)
                {
                    spawnID = ItemID.NecroHelmet;
                }
                else
                {
                    spawnID = ItemID.NecroGreaves;
                }

                itemID = Item.NewItem(new Rectangle((int)npc.position.X, (int)npc.position.Y, 0, 0), spawnID);
                Main.item[itemID].GetGlobalItem<VapeGlobalItem>().Qualify(ItemQuality.Unique);
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                chance = Main.rand.Next(0, 3);

                if (chance == 0)
                {
                    spawnID = ItemID.MoltenBreastplate;
                }
                else if (chance == 1)
                {
                    spawnID = ItemID.MoltenHelmet;
                }
                else
                {
                    spawnID = ItemID.MoltenGreaves;
                }

                itemID = Item.NewItem(new Rectangle((int)npc.position.X, (int)npc.position.Y, 0, 0), spawnID);
                Main.item[itemID].GetGlobalItem<VapeGlobalItem>().Qualify(ItemQuality.Unique);
            }
            if (npc.type == NPCID.Retinazer || npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime)
            {
                chance = Main.rand.Next(0, 3);

                if (chance == 0)
                {
                    spawnID = ItemID.HallowedPlateMail;
                }
                else if (chance == 1)
                {
                    spawnID = ItemID.HallowedHelmet;
                }
                else if (chance == 2)
                {
                    spawnID = ItemID.HallowedHeadgear;
                }
                else if (chance == 3)
                {
                    spawnID = ItemID.HallowedMask;
                }
                else
                {
                    spawnID = ItemID.HallowedGreaves;
                }

                itemID = Item.NewItem(new Rectangle((int)npc.position.X, (int)npc.position.Y, 0, 0), spawnID);
                Main.item[itemID].GetGlobalItem<VapeGlobalItem>().Qualify(ItemQuality.Unique);
            }
            if (npc.type == NPCID.Plantera)
            {
                chance = Main.rand.Next(0, 12);

                if (chance == 0)
                {
                    spawnID = ItemID.ShroomiteBreastplate;
                }
                else if (chance == 1)
                {
                    spawnID = ItemID.ShroomiteMask;
                }
                else if (chance == 2)
                {
                    spawnID = ItemID.ShroomiteHelmet;
                }
                else if (chance == 3)
                {
                    spawnID = ItemID.ShroomiteHeadgear;
                }
                else if (chance == 4)
                {
                    spawnID = ItemID.ShroomiteLeggings;
                }
                else if (chance == 5)
                {
                    spawnID = ItemID.SpectreRobe;
                }
                else if (chance == 6)
                {
                    spawnID = ItemID.SpectreHood;
                }
                else if (chance == 7)
                {
                    spawnID = ItemID.SpectreMask;
                }
                else if (chance == 8)
                {
                    spawnID = ItemID.SpectrePants;
                }
                else if (chance == 9)
                {
                    spawnID = ItemID.TurtleScaleMail;
                }
                else if (chance == 10)
                {
                    spawnID = ItemID.TurtleHelmet;
                }
                else
                {
                    spawnID = ItemID.TurtleLeggings;
                }

                itemID = Item.NewItem(new Rectangle((int)npc.position.X, (int)npc.position.Y, 0, 0), spawnID);
                Main.item[itemID].GetGlobalItem<VapeGlobalItem>().Qualify(ItemQuality.Unique);
            }

            #endregion

            base.NPCLoot(npc);
        }

        public void ChaosTransform(NPC npc)
        {
            VapeGlobalNpc global = npc.GetGlobalNPC<VapeGlobalNpc>();
            global.chaosMultiplier = Main.rand.Next(3, 6);
            npc.scale *= global.chaosMultiplier / 2.7f;
            npc.lifeMax *= global.chaosMultiplier;
            npc.life = npc.lifeMax;
            npc.defDamage *= global.chaosMultiplier;
            npc.defDefense *= global.chaosMultiplier / 2;
            npc.color = new Color(179, 104, 255, 127);
            npc.stepSpeed *= global.chaosMultiplier / 2f;

            global.isChaos = true;
        }

        public override void ResetEffects(NPC npc)
        {
            npc.GetGlobalNPC<VapeGlobalNpc>().hemorrhage = false;
            if (npc.GetGlobalNPC<VapeGlobalNpc>().isChaos)
            {
                npc.GivenName = String.Format("Chaos {0}", npc.TypeName);
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            VapeGlobalNpc global = npc.GetGlobalNPC<VapeGlobalNpc>();
            if (global.hemorrhage)
            {
                npc.lifeRegen -= global.hemorrhageDamage;
            }
        }

        private static bool IsIgnoredType(int type)
        {
            return ignoredTypes.Contains(type);
        }
    }
}
