using System;
using System.Collections.Generic;
using IrrKlang;
using Microsoft.Xna.Framework;

/// <summary>
/// use this for playing all sounds, documentation: http://www.ambiera.com/irrklang/docu/classirrklang_1_1_i_sound_engine.html
/// </summary>

namespace SpaceAssault.Utils
{
    public class ISpaceSoundEngine : ISoundEngine
    {
        Dictionary<string, ISoundSource> soundSources = new Dictionary<string, ISoundSource>();

        public ISpaceSoundEngine() : base()
        {
            setListenerPosToCameraTarget();
        }
        public ISpaceSoundEngine(SoundOutputDriver driver) : base(driver)
        {
            setListenerPosToCameraTarget();
        }
        public ISpaceSoundEngine(SoundOutputDriver driver, SoundEngineOptionFlag options) : base(driver, options)
        {
            setListenerPosToCameraTarget();
        }
        public ISpaceSoundEngine(SoundOutputDriver driver, SoundEngineOptionFlag options, string deviceID) : base(driver, options, deviceID)
        {
            setListenerPosToCameraTarget();
        }

        public ISoundSource AddSoundSourceFromFile(string soundName, string filePath)
        {
            soundSources.Add(soundName, AddSoundSourceFromFile(filePath, StreamMode.AutoDetect, true));

            return getISoundSource(soundName);
        }

        public ISound Play2D(string soundName, float volume, bool enableSoundEffects)
        {
            var curSoundSource = getISoundSource(soundName);
            if (curSoundSource == null) throw new NullReferenceException(soundName + "was not found in soundSources");
            ISound curSound = Play2D(getISoundSource(soundName), false, true, enableSoundEffects);
            curSound.Volume = volume;      // if you get a nullReferenceException here the soundfile is likely physically not existing (check correct filePath when loading the sound)
            curSound.Paused = false;
            return curSound;
        }

        public ISound Play3D(string soundName, float volume, Vector3 position, bool enableSoundEffects)
        {
            var curSoundSource = getISoundSource(soundName);
            if (curSoundSource == null) throw new NullReferenceException(soundName + "was not found in soundSources");
            ISound curSound = Play3D(curSoundSource, position.X, position.Y, position.Z, false, true, enableSoundEffects);
            curSound.Volume = volume;
            curSound.Paused = false;
            return curSound;
        }

        new public void Update()
        {
            base.Update();
            setListenerPosToCameraTarget();
        }

        public ISoundSource getISoundSource(string soundName)
        {
            ISoundSource sound;
            if (soundSources.TryGetValue(soundName, out sound))
            {
                return sound;
            }
            else return null;
        }

        public void setListenerPosToCameraTarget()
        {
            Vector3D curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
            SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
        }

    }
}
