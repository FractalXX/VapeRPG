using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class ExplodingRage : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Exploding rage";
            this.Description = "Killing enemies with melee has a chance (10%/20%/30%) to result in their bodies exploding dealing 10% of their maximum life in a medium sized area.";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<Bloodlust>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0 && (proj == null ? item.melee : proj.melee) && VapeRPG.rand.Next(101) <= modPlayer.GetSkillLevel<ExplodingRage>() * 10)
            {
                int dustCount = 20;
                int bloodRange = 15;
                for (int i = 0; i < dustCount; i++)
                {
                    for (int j = 0; j < dustCount; j++)
                    {
                        double angle = VapeRPG.rand.Next(1, 360) * Math.PI / 180;
                        Vector2 bloodTarget = new Vector2(target.position.X + bloodRange * (float)Math.Cos(angle), target.position.Y + bloodRange * (float)Math.Sin(angle));
                        Vector2 bloodVelocity = target.position - bloodTarget;

                        int v = 10;
                        float speedMul = v / bloodVelocity.Length();
                        bloodVelocity.X = speedMul * bloodVelocity.X;
                        bloodVelocity.Y = speedMul * bloodVelocity.Y;
                        //Dust.NewDustPerfect(target.position, 5, bloodVelocity, Scale: 3.0f).noGravity = true; // perfect circle
                        Main.dust[Dust.NewDust(target.position, 60, 30, 5, bloodVelocity.X, bloodVelocity.Y, Scale: 3.0f)].noGravity = true;
                    }
                }
                foreach (NPC npc in Main.npc)
                {
                    if (npc != target && Vector2.Distance(npc.position, target.position) <= 150)
                    {
                        npc.StrikeNPC((int)Math.Ceiling(target.lifeMax * 0.1f), 0, 0);
                    }
                }
            }
        }
    }
}
