using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class FirstTouch : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "First touch";
            this.Description = "If your enemy is at its max health, your first hit (minions included) instantly damages it for 10% of their health.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<Warmth>();
        }

        public override void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(target.life == target.lifeMax)
            {
                int amount = (int)Math.Floor(target.life * 0.1f);
                target.life -= amount;
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 50, 100, 100), Color.Cyan, amount);
            }
        }
    }
}
