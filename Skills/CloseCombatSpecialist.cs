using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class CloseCombatSpecialist : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Close combat specialist";
            this.Description = "You deal 1.5x damage to enemies within 10 tiles radius.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<OneAboveAll>();
        }

        public override void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (Vector2.Distance(modPlayer.player.position, target.position) < 10)
            {
                damage = (int)(damage * 1.5f);
            }
        }
    }
}
