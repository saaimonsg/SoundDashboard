using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ArlongStreambot.audioplayer.soundboard;
using ArlongStreambot.core;
using log4net;
using NAudio.Wave;

namespace ArlongStreambot
{
    public class AudioPlayerViewController : IDisposable
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(AudioPlayerViewController));
        private static int OutputDeviceNumber { get; set; } = -1;
        private static int InputDeviceNumber { get; set; } = -1;
        private WaveOutEvent _outputDevice;
        private WaveInEvent _inputDevice;
        private AudioFileReader _audioFile;


        public AudioPlayerViewController()
        {
            
        }

        private void CreateOutputDevice()
        {
            _outputDevice = new WaveOutEvent();
            _outputDevice.DesiredLatency = 80;
            _outputDevice.DeviceNumber = OutputDeviceNumber;
            _outputDevice.PlaybackStopped += OnPlaybackStoppedEventHandler();
        }

        public void ChangeDeviceOutput(int device)
        {
            if (_outputDevice != null)
            {
                _outputDevice.Dispose();
                _outputDevice = null;
            }

            OutputDeviceNumber = device;
            CreateOutputDevice();
        }

        public EventHandler OnButtonPlayClickEventHandler2(string destinationFilePath)
        {
            return ((sender, args) => { PlaySoundClip(destinationFilePath); });
        }

        public void PlaySoundClip(String sourcePath)
        {
            CreateOutputDevice();
            _audioFile = new AudioFileReader(sourcePath);

            if (_outputDevice.PlaybackState == PlaybackState.Playing)
            {
                _outputDevice.Stop();
                _audioFile.Position = 0;
            }
            _outputDevice.Init(_audioFile);
            _outputDevice.Play();
        }

        private EventHandler<StoppedEventArgs> OnPlaybackStoppedEventHandler()
        {
            return (sender, args) =>
            {
                _audioFile.Dispose();
                _outputDevice.Dispose();
            };
        }

        public void OnButtonStopClickEventHandler(object sender, EventArgs args)
        {
            _outputDevice?.Stop();
        }

        private void OnPlaybackStoppedEventHandler(object sender, StoppedEventArgs args)
        {
        }

        public void SaveAudioFileDragAndDrop(SoundClipData data)
        {
            if (data.SourceFilePath != data.DestinationFilePath)
            {
                if (!File.Exists(data.DestinationFilePath))
                {
                    File.Copy(data.SourceFilePath, data.DestinationFilePath);
                }
                else
                {
                    File.Delete(data.DestinationFilePath);
                    File.Copy(data.SourceFilePath, data.DestinationFilePath);
                }
            }
        }

        public bool ValidateAudioFormat(string sourceFilePath)
        {
            foreach (var extension in ResourceHandler.audioExtensions)
            {
                if (sourceFilePath.EndsWith(extension))
                {
                    return true;
                }
            }

            return false;
        }


        public void Dispose()
        {

        }
    }
}


// TODO playback state (stopped and paused) method l:48 
// else if (_outputDevice.PlaybackState == PlaybackState.Stopped)
// {
//     _audioFile.Position = 0;
//     _outputDevice.Init(_audioFile);
// }
// else if (_outputDevice.PlaybackState == PlaybackState.Paused)
// {
//     _audioFile.Position = _outputDevice.GetPosition();
//     _outputDevice.Init(_audioFile);
// }