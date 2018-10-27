using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Bloodlust : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Bloodlust";
            this.Description = "Killing enemies grants you a regeneration buff for 10 seconds.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<Rage>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0)
            {
                modPlayer.player.AddBuff(2, 600);
            }
        }
    }
}
