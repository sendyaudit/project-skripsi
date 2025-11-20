using UnityEngine;

public class musiicbackground : MonoBehaviour
{
    public AudioSource musicAudioSource; // Hubungkan dari Inspector

    void Start()
    {
        if (musicAudioSource != null && !musicAudioSource.isPlaying)
        {
            musicAudioSource.Play(); // Play otomatis saat scene dimulai
        }
    }

    public void MuteHandler(bool isMuted)
    {
        if (musicAudioSource == null) return;

        if (isMuted)
        {
            musicAudioSource.Pause(); // Saat toggle aktif = mute
        }
        else
        {
            musicAudioSource.UnPause(); // Saat toggle nonaktif = unmute
        }
    }
}
