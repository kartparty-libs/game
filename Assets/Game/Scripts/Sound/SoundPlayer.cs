using UnityEngine;

public class SoundPlayer
{
    public static void Play(string sound)
    {
    }

    public static void Play(AudioSource audioSource, AudioClip soundClip)
    {
        PlaySound(audioSource, soundClip);
    }

    static void PlaySound(AudioSource audioSource, AudioClip soundClip)
    {
        // 检查AudioSource是否为空以及音效文件是否存在
        if(!audioSource.enabled)
        {
            return;
        }
        if (audioSource != null && soundClip != null)
        {
            // 播放音效
            audioSource.clip = soundClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or AudioClip is missing!");
        }
    }
}
