using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using Fall2020_CSC403_Project.Properties;

namespace Fall2020_CSC403_Project
{
    public static class MusicSettings
    {
        public static SoundPlayer bgMusicPlayer;
        private static bool isBackgroundMusicPlaying = false;

        static MusicSettings()
        {
            // Initialize the background music player
            bgMusicPlayer = new System.Media.SoundPlayer(Resources.ShootToThrill); // Replace with the actual resource name
        }
        public static void PlayBackgroundMusic()
        {
            if (bgMusicPlayer != null && !isBackgroundMusicPlaying )
            {
                bgMusicPlayer.PlayLooping();
                isBackgroundMusicPlaying = true;
            }
        }
        public static void StopBackgroundMusic()
        {
            if (bgMusicPlayer != null && isBackgroundMusicPlaying )
            {
                bgMusicPlayer.Stop();
                isBackgroundMusicPlaying = false;
            }
        }
    }
}