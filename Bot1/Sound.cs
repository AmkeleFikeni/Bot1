using System;
using System.Media;

namespace Bot1
{
    public class Sound
    {
        public void PlayWelcomeSound()
        {
            try
            {
                SoundPlayer player =
                    new SoundPlayer(Bot1.Properties.Resources.amanga);

                player.Play();
            }
            catch (Exception)
            {
            }
        }
    }
}