using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArlongStreambot.audioplayer.soundboard
{
    public interface ISoundClipService
    {
    // Clip
    SoundClip Get(long id);
    SoundClip Create(long id);
    SoundClip Delete(long id);
    SoundClip Mute(long id);
    SoundClip ChangeImage(long id, Image image);
    SoundClip ChangeName(long id, string newName);
    
    List<SoundClip> GetAll();
    List<SoundClip> SetSoundClipAsFavourite(long id);
    List<SoundClip> UnsetSoundClipAsFavourite(long id);
    List<SoundClip> ShowOnlyFavourites(Boolean value);
    List<SoundClip> GetFavoritesClips();

    
    }
}