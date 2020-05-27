using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Skills
{
    class EnergizingKills : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Energizing kills";
            this.Description = "Killing enemies with magic or summon damage grants you a stack of 'Energized'. After getting 20 stacks you receive a movement speed increase and release low-damaging sparks with mediocre knockback on being hit for 10 seconds.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<ManaAddict>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 && !modPlayer.energized && (proj == null ? item.magic : proj.magic))
            {
                if (modPlayer.energizedStacks <= 0)
                {
                    modPlayer.energizedStacks = 1;
                }
                modPlayer.player.AddBuff(ModContent.BuffType<Buffs.Energized>(), 60);
            }
        }
    }
}
