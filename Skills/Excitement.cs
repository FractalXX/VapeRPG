using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Excitement : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Excitement";
            this.Description = "Killing enemies grants you a shine buff for 6 seconds.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Reaper;
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0)
            {
                modPlayer.player.AddBuff(11, 360);
            }
        }
    }
}
