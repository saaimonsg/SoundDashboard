using System;
using System.Drawing;


namespace ArlongStreambot.audioplayer.soundboard
{
    public class SoundClip
    {
        public long id { get; set; }
        public String file_name { get; set; }
        public String display_name { get; set; }
        public String image { get; set; }
        public String source_path { get; set; }
        public bool is_favourite { get; set; }
        public long sound_bank_id { get; set; }

        public SoundClip()
        {
        }

        public SoundClip(long id, string fileName, string displayName, string image, string sourcePath, bool isFavourite, long soundBankId)
        {
            this.id = id;
            file_name = fileName;
            display_name = displayName;
            this.image = image;
            source_path = sourcePath;
            is_favourite = isFavourite;
            sound_bank_id = soundBankId;
        }

        public SoundClip(string file, string display_name, string source_path)
        {
            this.file_name = file;
            this.display_name = display_name;
            this.source_path = source_path;
            this.image = "";
            this.is_favourite = false;
            this.sound_bank_id = 0;
        }
    }
}