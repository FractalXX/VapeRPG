using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class HawkEye : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Hawk eye";
            this.Description = "Your critical chance increases with distance to your enemy.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<TitanGrip>();
        }

        public override void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(!crit)
            {
                float distance = Vector2.Distance(modPlayer.player.position, target.position);
                float chance = distance * 0.025f;
                if (VapeRPG.rand.Next(0, 101) <= chance)
                {
                    crit = true;
                }
            }
        }
    }
}
