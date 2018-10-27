using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Execution : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Execution";
            this.Description = "If your enemy is under 20% health you deal increased damage (10%/level) to them.";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<Kickstart>();
        }

        public override void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            damage += (int)Math.Ceiling(damage * 0.1f * modPlayer.GetSkillLevel<Execution>());
        }
    }
}
