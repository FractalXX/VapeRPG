using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace VapeRPG.Skills
{
    class Confusion : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Confusion";
            this.Description = "Hits have a chance to confuse your enemy. (5%/level)";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<OneAboveAll>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if(VapeRPG.rand.Next(0, 101) <= modPlayer.GetSkillLevel<Confusion>() * 5)
            {
                target.AddBuff(31, 300);
                CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y - 20, 50, 50), Color.OrangeRed, "Confused");
                if (modPlayer.HasSkill<ConfusionField>())
                {
                    float confusionDistance = 200;
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc != target && Vector2.Distance(target.position, npc.position) <= confusionDistance)
                        {
                            npc.AddBuff(31, 300);
                            CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y - 20, 50, 50), Color.OrangeRed, "Confused");
                        }
                    }
                }
            }
        }
    }
}
