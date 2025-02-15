using UnityEngine;

public class AudioDefine
{
    public const string MasterVolume = nameof(MasterVolume);
    // public const string EffectVolume = nameof(EffectVolume);
    public const string MusicVolume = nameof(MusicVolume);
    public const int MusicGroup = 1;
    public const int EffectGroup = 2;
    public static void Load()
    {
        var effect = PlayerPrefs.GetInt(AudioDefine.MasterVolume, 1);
        var music = PlayerPrefs.GetInt(AudioDefine.MusicVolume, 1);
        SetAllVolume(MasterVolume, effect > 0);
        SetMusicVolume(MusicVolume, effect > 0);
    }
    public static void SetAllVolume(string name, bool enable)
    {
        GameEntry.Audio.SetFloat(AudioDefine.MasterVolume, enable ? 0f : -80f);
    }
    public static void SetMusicVolume(string name, bool enable)
    {
        GameEntry.Audio.SetFloat(AudioDefine.MusicVolume, enable ? 0f : -80f);
    }
}