using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Skills
{
    class Bounce : Skill
    {
        protected override void SetDefaults()
        {
            this.Name = "Bounce";
            this.Description = "Magic hits have a chance (10%/level) to spawn a spark that bounces to another enemy. The spark's damage is 10%/20%/30% of the original damage.";
            this.MaxLevel = 3;
            this.Tree = SkillTree.Shredder;

            this.AddPrerequisite<OneAboveAll>();
        }

        public override void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if((proj == null ? item.magic : proj.magic) && VapeRPG.random.Next(0, 101) <= modPlayer.GetSkillLevel<Bounce>() * 10)
            {
                NPC closest = null;
                float closestDistance = 500;

                foreach (NPC npc in Main.npc)
                {
                    if (npc != target)
                    {
                        float distance = Vector2.Distance(npc.position, target.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closest = npc;
                        }
                    }
                }

                if (closest != null)
                {
                    Vector2 sparkVelocity = closest.position - target.position;

                    int v = 5;
                    float speedMul = v / sparkVelocity.Length();
                    sparkVelocity.X = speedMul * sparkVelocity.X;
                    sparkVelocity.Y = speedMul * sparkVelocity.Y;

                    float sparkDamage = (proj == null ? item.damage : proj.damage) * modPlayer.GetSkillLevel<Bounce>() * 10 * 0.1f;
                    Projectile spark = Projectile.NewProjectileDirect(new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2), sparkVelocity, modPlayer.mod.ProjectileType<Projectiles.ElectricSpark>(), (int)Math.Ceiling(sparkDamage), 20, modPlayer.player.whoAmI);
                    Main.PlaySound(modPlayer.mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Electricity"), modPlayer.player.position);
                }
            }
        }
    }
}
