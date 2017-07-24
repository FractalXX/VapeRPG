using System;
using Terraria;
using Terraria.ModLoader;
using VapeRPG;

namespace ExampleMod.Commands
{
    public class XpCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "xp"; }
        }

        public override string Usage
        {
            get { return "/xp <xp>"; }
        }

        public override string Description
        {
            get { return "Gives xp."; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            int val;

            VapePlayer player = caller.Player.GetModPlayer<VapePlayer>();

            if(int.TryParse(args[0], out val))
            {
                player.GainExperience(val);
            }
        }
    }
}