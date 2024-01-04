namespace ArlongStreambot.audioplayer.soundboard
{
    public class SoundClipQueryConstants
    {
        public static string CreateTable =
            "CREATE TABLE IF NOT EXISTS ap_soundclip (id INTEGER PRIMARY KEY AUTOINCREMENT,file_name VARCHAR unique,display_name VARCHAR,image VARCHAR,source_path VARCHAR unique,is_favourite BOOLEAN,sound_bank_id INTEGER)";
        
    }
}