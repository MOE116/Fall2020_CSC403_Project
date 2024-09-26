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
        public static SoundPlayer bgMusicPlayerMainMenu;
        public static SoundPlayer bgMusicPlayerLevel;
        private static bool isBackgroundMusicPlaying = false;

        static MusicSettings()
        {
            // Initialize the background music player
            bgMusicPlayerMainMenu = new SoundPlayer(Resources.paintheme);
            bgMusicPlayerLevel = new SoundPlayer(Resources.ShootToThrill); // Replace with the actual resource name
        }
        public static void PlayBackgroundMusicMainMenu()
        {
            if (bgMusicPlayerMainMenu != null && !isBackgroundMusicPlaying)
            {
                bgMusicPlayerMainMenu.PlayLooping();
                isBackgroundMusicPlaying = true;
            }
        }
        public static void PlayBackgroundMusicLevel()
        {
            if (bgMusicPlayerLevel != null && !isBackgroundMusicPlaying)
            {
                bgMusicPlayerLevel.PlayLooping();
                isBackgroundMusicPlaying = true;
            }
        }
        public static void StopBackgroundMusic()
        {
            if (bgMusicPlayerMainMenu != null && isBackgroundMusicPlaying)
            {
                bgMusicPlayerMainMenu.Stop();
                isBackgroundMusicPlaying = false;
            }
            if (bgMusicPlayerLevel != null && isBackgroundMusicPlaying)
            {
                bgMusicPlayerLevel.Stop();
                isBackgroundMusicPlaying = false;
            }
        }
    }
}