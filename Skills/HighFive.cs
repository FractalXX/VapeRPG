using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class HighFive : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "High five";
            this.Description = "Critical hits further increase your crit and damage by 2%. Resets on non-crits or 50 stacks.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<OneAboveAll>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (!crit)
            {
                if (modPlayer.highfiveStacks > 0)
                {
                    modPlayer.player.ClearBuff(modPlayer.mod.BuffType<Buffs.HighFive>());
                    modPlayer.highfiveStacks = 0;
                }
            }
            else
            {
                if (modPlayer.highfiveStacks <= 0)
                {
                    modPlayer.highfiveStacks = 1;
                }
                modPlayer.player.AddBuff(modPlayer.mod.BuffType<Buffs.HighFive>(), 60);
            }
        }
    }
}
