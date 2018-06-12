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

        private Song themeSong;

        public void Load(ContentManager content)
        {
            uiEffect = content.Load<SoundEffect>("cloth");
            footstepEffect = content.Load<SoundEffect>("footstep");
            magicEffect = content.Load<SoundEffect>("magic");
            hitEffect = content.Load<SoundEffect>("hit");

            // themeSong = Content.Load<Song>("");
            MediaPlayer.IsRepeating = true;
            
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
            switch(song)
            {
                case "theme":
                    MediaPlayer.Play(themeSong);
                    MediaPlayer.Volume = volume;
                    break;
            };
        }
    }
}
