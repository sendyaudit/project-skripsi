using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game : MonoBehaviour
{
    // Komponen AudioSource untuk memainkan suara
    public AudioSource audioSource;

    // Suara untuk tombol back
    public AudioClip buttonClickSound;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = 0.1f; // Volume untuk background music

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackMenuButton(string scenename)
    {
        StartCoroutine(PlaySoundAndLoadScene(scenename));
    }

    IEnumerator PlaySoundAndLoadScene(string scenename)
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
            yield return new WaitForSeconds(buttonClickSound.length); // Tunggu sampai suara selesai
        }
        
        SceneManager.LoadScene(scenename);
    }
}