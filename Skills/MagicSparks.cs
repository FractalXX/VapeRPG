using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class MagicSparks : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Magic sparks";
            this.Description = "Magic kills have a chance (10%/level) to release little magic sparks which reduces the life of nearby enemies by 5%/level.";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<ManaAddict>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0 && (proj == null ? item.magic : proj.magic) && VapeRPG.rand.Next(0, 101) <= modPlayer.GetSkillLevel<MagicSparks>() * 10)
            {
                int dustCount = 20;
                int sparkRange = 30;
                for (int i = 0; i < dustCount; i++)
                {
                    for (int j = 0; j < dustCount; j++)
                    {
                        double angle = VapeRPG.rand.Next(1, 360) * Math.PI / 180;
                        Vector2 sparkTarget = new Vector2(target.position.X + sparkRange * (float)Math.Cos(angle), target.position.Y + sparkRange * (float)Math.Sin(angle));
                        Vector2 sparkVelocity = target.position - sparkTarget;

                        int v = 15;
                        float speedMul = v / sparkVelocity.Length();
                        sparkVelocity.X = speedMul * sparkVelocity.X;
                        sparkVelocity.Y = speedMul * sparkVelocity.Y;
                        Dust dust = Dust.NewDustPerfect(target.position, 130 + j % 5, sparkVelocity);
                        dust.noGravity = true;
                    }
                }
                foreach (NPC npc in Main.npc)
                {
                    if (npc != target && Vector2.Distance(npc.position, target.position) <= 180)
                    {
                        int amount = (int)Math.Ceiling(npc.lifeMax * modPlayer.GetSkillLevel<MagicSparks>() * 0.05f);
                        npc.life -= amount;
                        CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y - 50, 100, 100), Color.Cyan, amount);
                        if (modPlayer.GetSkillLevel<SpectralSparks>() > 0)
                        {
                            npc.defense -= (int)Math.Ceiling(npc.defense * 0.15f);
                        }
                    }
                }
            }
        }
    }
}
