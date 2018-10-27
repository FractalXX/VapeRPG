using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Rage : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Rage";
            this.Description = "Killing enemies grants you the Rage buff, increasing your melee damage (3%/level) for 10 seconds.";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<Excitement>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0)
            {
                modPlayer.player.AddBuff(modPlayer.mod.BuffType<Buffs.Rage>(), 600);
            }
        }
    }
}
