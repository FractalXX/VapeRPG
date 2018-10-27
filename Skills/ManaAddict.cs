using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class ManaAddict : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Mana addict";
            this.Description = "Kills give you a mana regen buff for 10 seconds.";
            this.MaxLevel = 1;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<Excitement>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            modPlayer.player.AddBuff(6, 600);
        }
    }
}
