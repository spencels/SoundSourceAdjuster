using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundSourceAdjuster
{
    public class AudioDevice
    {
        public string Name;
        public string Id;
        public bool IsDefault = false;
        public bool IsDefaultCommunication = false;
    }

    public class AudioDevicesService
    {
        private IAudioController controller = new CoreAudioController();

        public List<IDevice> EnumerateAudioDevices()
        {
            var devices = controller.GetDevices(DeviceType.Playback);

            return devices.ToList();
        }
    }
}
