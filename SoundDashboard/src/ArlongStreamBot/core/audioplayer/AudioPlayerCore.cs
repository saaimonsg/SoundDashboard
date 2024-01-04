using System.IO;
using ArlongStreambot.audioplayer.soundboard;
using ArlongStreambot.core;
using Dapper;
using log4net;

namespace ArlongStreambot
{
    public class AudioPlayerCore
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AudioPlayerCore));

        public AudioPlayerView _audioPlayerView { get; set; } = new AudioPlayerView();
        public AudioPlayerViewController _AudioPlayerViewController { get; set; } = new AudioPlayerViewController();

        public AudioPlayerCore()
        {
            if (!Directory.Exists(ResourceHandler.soundFolder))
            {
                log.Info("Default directory doesn't exist.");
                Directory.CreateDirectory(ResourceHandler.soundFolder);
                log.Info("Default directory created");
            }

            using (var sql = new SqliteDatabaseConnection("audioplayer"))
            {
                // thank you tbdgamer 
                sql.Connection.Execute(SoundClipQueryConstants.CreateTable);
            }
        }
    }
}


// https://www.hispasonic.com/foros/programar-asio-c-para-aplicacion-multicanal-propia/147180


// Console.WriteLine("Device Type: {0}", device.GetType()); // NAudio.CoreAudioApi.MMDevice
// Console.WriteLine("Device Name: {0}", device.FriendlyName); // Microphone (High Definition Audio Device)
// Console.WriteLine("Device ID: {0}", device.ID); // {0.0.1.00000000}.{2eebde9c-626a-4135-9ae3-7023dde46f34}
// Console.WriteLine("Device IconPath: {0}", device.IconPath); // %windir%\system32\mmres.dll,-3014
// Console.WriteLine("Device State: {0}", device.State); // Active
// Console.WriteLine("Device Data Flow: {0}", device.DataFlow); // Capture
// Console.WriteLine("------------------------------");