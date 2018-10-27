using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace VapeRPG.Skills
{
    class OneAboveAll : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "One above all";
            this.Description = "You have 2% chance to one hit kill any non-boss, non-pillar enemy.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Shredder;
        }

        public override void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(VapeRPG.rand.Next(0, 101) <= 2 && target.type != NPCID.TargetDummy && !target.boss && !VapeConfig.IsIgnoredTypeChaos(target))
            {
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 20, 50, 50), Color.Red, "One Above All");
                target.StrikeNPC(target.life, 0, 0);
            }
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (VapeRPG.rand.Next(0, 101) <= 2 && target.type != NPCID.TargetDummy && !target.boss && !VapeConfig.IsIgnoredTypeChaos(target))
            {
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 20, 50, 50), Color.Red, "One Above All");
                target.StrikeNPC(target.life, 0, 0);
            }
        }
    }
}
