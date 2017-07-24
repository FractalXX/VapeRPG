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
            NPCID.GoldButterfly
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
            if (Main.rand.Next(0, 101) <= chaosChance && !npc.SpawnedFromStatue && !npc.friendly && !IsIgnoredType(npc.type))
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
                        VapePlayer vp = player.GetModPlayer<VapePlayer>();
                        gainedXp = npc.lifeMax * (1 + (Math.Abs(npc.defDefense) + 1) / npc.defDamage);
                        vp.GainExperience((int)gainedXp);
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
