using System;
using System.Collections.Generic;
using System.Linq;
using ArlongStreambot.core;
using Dapper;

namespace ArlongStreambot.audioplayer.soundboard
{
    public class SoundClipRepository : Repository<SoundClip, long>, IDisposable
    {
        public override string DbName => "audioplayer";
        public override string TableName => "ap_soundclip";

        public String findPathByDisplayName(string name)
        {
            string sourcePath = db.Connection
                .Query<String>($"select source_path from ap_soundclip where display_name = '{name}'")
                .FirstOrDefault();

            return sourcePath;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public List<string> FetchAllDisplayNames()
        {
            IEnumerable<string> names = db.Connection.Query<string>("select display_name from ap_soundclip").DefaultIfEmpty(null);
            return new List<string>(names.ToArray());
        }
    }
}