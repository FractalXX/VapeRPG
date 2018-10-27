using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class OverkillCharge : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Overkill charge";
            this.Description = "Killing enemies with critical hits restore a small percent of your mana (4%/level).";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<MagicSparks>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0 && crit)
            {
                int amount = (int)Math.Ceiling(modPlayer.player.statLifeMax * modPlayer.GetSkillLevel<OverkillCharge>() * 0.04f);
                modPlayer.player.ManaEffect(amount);
                modPlayer.player.statMana += amount;
            }
        }
    }
}
