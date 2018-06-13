using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class SoundManager
    {
        // sound effects
        private SoundEffect uiEffect;
        private SoundEffect footstepEffect;
        private SoundEffect magicEffect;
        private SoundEffect hitEffect;

        private SoundEffect themeSong;
        private SoundEffectInstance themeSongInstance;

        private SoundEffect cloraSong;
        private SoundEffectInstance cloraSongInstance;

        private SoundEffect advSong;
        private SoundEffectInstance advSongInstance;
        //private AudioFileReader themeSong = new AudioFileReader("Assets/songs/rey_theme.wav");
        /*private AudioFileReader cloraSong = new AudioFileReader("Assets/songs/clora_theme.wav");
        private IWavePlayer player = new WaveOut(WaveCallbackInfo.FunctionCallback());*/
        private string currentSong = "theme";
        private float currentVolume = 0.8f;

        public void Load(ContentManager content)
        {
            uiEffect = content.Load<SoundEffect>("cloth");
            footstepEffect = content.Load<SoundEffect>("footstep");
            magicEffect = content.Load<SoundEffect>("magic");
            hitEffect = content.Load<SoundEffect>("hit");

            cloraSong = content.Load<SoundEffect>("clora_theme");
            cloraSongInstance = cloraSong.CreateInstance();
            cloraSongInstance.IsLooped = true;

            themeSong = content.Load<SoundEffect>("rey_theme");
            themeSongInstance = themeSong.CreateInstance();
            themeSongInstance.IsLooped = true;

            advSong = content.Load<SoundEffect>("adventure");
            advSongInstance = advSong.CreateInstance();
            advSongInstance.IsLooped = true;

            //MediaPlayer.IsRepeating = true;
            /*player.PlaybackStopped += Player_PlaybackStopped;*/

        }

        /// <summary>
        /// play any sound effect
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(string sound, float volume, float pitch, float pan)
        {
            // create a sound effect instance
            SoundEffectInstance sei = null;
            
            // choose sound
            switch(sound)
            {
                case "ui":
                    sei = uiEffect.CreateInstance();
                    break;
                case "footstep":
                    sei = footstepEffect.CreateInstance();
                    break;
                case "magic":
                    sei = magicEffect.CreateInstance();
                    break;
                case "hit":
                    sei = hitEffect.CreateInstance();
                    break;
            }
            //play sound
            if (sei != null)
            {
                sei.Volume = volume;
                sei.Pitch = pitch;
                sei.Pan = pan;
                sei.Play();
            }
        }

        public void PlaySong(string song, float volume)
        {
            /*player.Stop();
            //player.Dispose();
            currentSong = song;
            currentVolume = volume;*/

            switch (song)
            {
                case "theme":
                    if (themeSongInstance != null)
                    {
                        cloraSongInstance.Stop();
                        advSongInstance.Stop();
                        themeSongInstance.Play();
                        themeSongInstance.Volume = volume;
                    }
                    break;
                case "clora":
                    if (cloraSongInstance != null)
                    {
                        themeSongInstance.Stop();
                        advSongInstance.Stop();
                        cloraSongInstance.Play();
                        cloraSongInstance.Volume = volume;
                    }
                    break;
                case "adventure":
                    if (advSongInstance != null)
                    {
                        themeSongInstance.Stop();
                        cloraSongInstance.Stop();
                        advSongInstance.Play();
                        advSongInstance.Volume = volume;
                    }
                    break;
            };
        }

        public void CloseAudio()
        {
            /*player.Stop();
            player.Dispose();*/
        }
    }
}
