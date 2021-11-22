using System;
using Terraria;
using Terraria.ModLoader;
using VapeRPG;

using Microsoft.Xna.Framework;

namespace VapeRPG.Commands
{
    public class ResetCommand : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;

		public override string Command
			=> "resetCharacter";

        public override string Usage
            => "/resetCharacter";

		public override string Description
			=> "Resets your Vape RPG Character to level 1.";

		public override void Action(CommandCaller caller, string input, string[] args)
        {
            VapePlayer player = caller.Player.GetModPlayer<VapePlayer>();
			if(player != null)
            {
                player.level = 1;
                player.chaosRank = 1;

                player.xp = 0;
                player.chaosXp = 0;

                player.statPoints = 0;
                player.chaosPoints = 0;

                foreach (string stat in VapeRPG.BaseStats)
                {
                    player.BaseStats[stat] = 1;
                }

                foreach (Skill skill in VapeRPG.Skills)
                {
                    player.SkillLevels[skill.name] = 0;
                }
            }
        }
    }
}