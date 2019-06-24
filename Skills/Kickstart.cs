using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Kickstart : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Kickstart";
            this.Description = "Your critical chance increases by 5%/10% against enemies above 70% health.";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<FirstTouch>();
        }

        public override void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(!crit && target.life >= target.lifeMax * 0.7f && VapeRPG.random.Next(0, 101) <= modPlayer.GetSkillLevel<Kickstart>() * 5)
            {
                crit = true;
            }
        }
    }
}
