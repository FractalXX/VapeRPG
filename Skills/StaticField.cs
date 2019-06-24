using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Skills
{
    class StaticField : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Static field";
            this.Description = "Killing enemies has a chance (5%/10%/15%) to summon an electric shield around you. The shield does summon damage (20/level) and lasts 10 seconds.";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Reaper;

            this.AddPrerequisite<Excitement>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(target.life <= 0 && VapeRPG.random.Next(0, 101) <= modPlayer.GetSkillLevel<StaticField>() * 5 && (proj == null ? item.magic : proj.magic))
            {
                int duration = 600;
                if (modPlayer.GetSkillLevel<HighVoltageField>() > 0)
                {
                    duration *= 2;
                    modPlayer.player.AddBuff(12, duration);
                }
                Main.PlaySound(modPlayer.mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/StaticFieldApply"), modPlayer.player.position);
                modPlayer.player.AddBuff(modPlayer.mod.BuffType<Buffs.StaticField>(), duration);
            }
        }
    }
}
