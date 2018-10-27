using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class TitanGrip : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Titan grip";
            this.Description = "Critical hits have increased knockback (20%/level).";
            this.MaxLevel = 2;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<HighFive>();
        }

        public override void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            knockback += knockback * modPlayer.GetSkillLevel<TitanGrip>() * 0.2f;
        }
    }
}
