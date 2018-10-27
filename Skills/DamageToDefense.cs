using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.Skills
{
    class DamageToDefense : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Damage to defense";
            this.Description = "Your damage is reduced by 10%/level but your defense increases by the same amount.";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Power;

            this.AddPrerequisite<Warmth>();
        }

        public override void UpdateStats(VapePlayer modPlayer)
        {
            float amount = modPlayer.GetSkillLevel<DamageToDefense>() * 0.1f;
            modPlayer.player.meleeDamage -= amount;
            modPlayer.player.magicDamage -= amount;
            modPlayer.player.rangedDamage -= amount;
            modPlayer.player.minionDamage -= amount;
            modPlayer.player.thrownDamage -= amount;
            modPlayer.player.statDefense += (int)Math.Ceiling(modPlayer.player.statDefense * amount);
        }
    }
}
