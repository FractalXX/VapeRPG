using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Overkill : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Overkill";
            this.Description = "Killing enemies with critical hits restore a small percent of your life (3%/level).";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<Rage>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0 && crit)
            {
                int amount = (int)Math.Ceiling(modPlayer.player.statLifeMax * modPlayer.GetSkillLevel<Overkill>() * 0.03f);
                modPlayer.player.HealEffect(amount);
                modPlayer.player.statLife += amount;
            }
        }
    }
}
