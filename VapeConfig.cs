using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace VapeRPG
{
    public static class VapeConfig
    {
        public static bool UIEnabled = true;
        public static int ChaosChance = 5;
        public static int MaxMinionPerSpirit = 50;
        public static int MaxTurretPerSpirit = 100;

        static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "VapeRPG.json");

        static Preferences Configuration = new Preferences(ConfigPath);

        public static void Load()
        {
            //Reading the config file
            bool success = ReadConfig();

            if (!success)
            {
                ErrorLogger.Log("Failed to read VapeRPG's config file! Recreating config...");
                CreateConfig();
            }
        }

        //Returns "true" if the config file was found and successfully loaded.
        static bool ReadConfig()
        {
            if (Configuration.Load())
            {
                Configuration.Get("EnableUI", ref UIEnabled);
                Configuration.Get("ChaosChance", ref ChaosChance);
                Configuration.Get("MaxMinionPerSpirit", ref MaxMinionPerSpirit);
                Configuration.Get("MaxTurretPerSpirit", ref MaxTurretPerSpirit);
                return true;
            }
            return false;
        }

        //It would make more sense to call this method SaveConfig(), but since we don't have an in-game editor or anything, this will only be called if a config file wasn't found or it's invalid.
        static void CreateConfig()
        {
            Configuration.Clear();
            Configuration.Put("EnableUI", UIEnabled);
            Configuration.Put("ChaosChance", ChaosChance);
            Configuration.Put("MaxMinionPerSpirit", MaxMinionPerSpirit);
            Configuration.Put("MaxTurretPerSpirit", MaxTurretPerSpirit);
            Configuration.Save();
        }
    }
}
