using System;
using System.Drawing;


namespace ArlongStreambot.audioplayer.soundboard
{
    public class SoundClipData
    {
        public long Id { get; set; }
        public String FileName { get; set; }
        public String SourceFilePath { get; set; }
        public String DestinationFilePath { get; set; }
        public SoundClip SoundClip { get; set; }

       
        public SoundClipData(string fileName, string sourceFilePath, string destinationFilePath = null)
        {
            this.SourceFilePath = sourceFilePath;
            this.FileName = fileName;
            this.DestinationFilePath = destinationFilePath;
            this.SoundClip = new SoundClip(fileName,
            fileName,
                destinationFilePath);
            
        }
    }
}