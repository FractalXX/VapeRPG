using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace VapeRPG.Sounds.Custom
{
    class StaticFieldApply : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            // By creating a new instance, this ModSound allows for overlapping sounds. Non-ModSound behavior is to restart the sound, only permitting 1 instance.
            if (soundInstance.State == SoundState.Playing)
                return null;
            soundInstance.Volume = volume * .5f;
            soundInstance.Pan = pan;
            soundInstance.Pitch = 0;
            return soundInstance;
        }
    }
}