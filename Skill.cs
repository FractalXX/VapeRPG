using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace VapeRPG
{
    public enum SkillTree { Shredder, Reaper, Power }

    public abstract class Skill
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int MaxLevel { get; protected set; }
        public SkillTree Tree { get; protected set; }
        public Texture2D Icon { get; private set; }

        private IList<Type> prerequisites;

        protected Skill()
        {
            this.prerequisites = new List<Type>();
            this.Icon = this.TryGetSkillFrame();
            this.SetDefaults();
        }

        public IList<Type> GetPrerequisites()
        {
            return this.prerequisites;
        }

        public void AddPrerequisite<T>()
            where T: Skill
        {
            this.prerequisites.Add(typeof(T));
        }

        protected abstract void SetDefaults();

        private Texture2D TryGetSkillFrame()
        {
            Texture2D frame;
            try
            {
                frame = ModLoader.GetTexture("VapeRPG/Skills/" + this.GetType().Name);
            }
            catch (Exception)
            {
                frame = ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrame");
            }

            return frame;
        }

        public virtual void Hurt(VapePlayer modPlayer, bool pvp, bool quiet, double damage, int hitDirection, bool crit) { }
        public virtual void ModifyHitByNPC(VapePlayer modPlayer, NPC npc, ref int damage, ref bool crit) { }
        public virtual void ModifyHitByProjectile(VapePlayer modPlayer, Projectile proj, ref int damage, ref bool crit) { }
        public virtual void ModifyHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit) { }
        public virtual void OnHitNPC(VapePlayer modPlayer, Item item, Projectile proj, NPC target, int damage, float knockback, bool crit) { }
        public virtual void Shoot(VapePlayer modPlayer, Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) { }
        public virtual void UpdateStats(VapePlayer modPlayer) { }
        public virtual void UseItem(VapePlayer modPlayer, Item item) { }
    }
}
