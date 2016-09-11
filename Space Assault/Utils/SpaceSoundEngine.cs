using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrrKlang;


namespace SpaceAssault.Utils
{
    public class ISpaceSoundEngine : ISoundEngine
    {
        public ISpaceSoundEngine() : base()
        {

        }
        public ISpaceSoundEngine(SoundOutputDriver driver) : base(driver)
        {

        }
        public ISpaceSoundEngine(SoundOutputDriver driver, SoundEngineOptionFlag options) : base(driver, options)
        {

        }
        public ISpaceSoundEngine(SoundOutputDriver driver, SoundEngineOptionFlag options, string deviceID) : base(driver, options, deviceID)
        {

        }
    }
}
