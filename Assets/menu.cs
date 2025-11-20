using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public GameObject menupanel;
    public GameObject infopanel;
    public GameObject exitConfirmPanel;
    public GameObject panelMenuKuis;

    public AudioSource sfxAudioSource;
    public AudioClip buttonClickSound;

    void Start()
    {
        menupanel.SetActive(true);
        infopanel.SetActive(false);
        panelMenuKuis.SetActive(false);

        if (exitConfirmPanel != null)
            exitConfirmPanel.SetActive(false);

        AudioListener.volume = 0.1f; // Volume untuk background music
    }

    public void StartButton(string scenename)
    {
        StartCoroutine(PlaySoundAndLoadScene(scenename));
    }

    IEnumerator PlaySoundAndLoadScene(string scenename)
    {
        if (sfxAudioSource != null && buttonClickSound != null)
        {
            sfxAudioSource.PlayOneShot(buttonClickSound, 1f); // Volume SFX = 1
            yield return new WaitForSeconds(buttonClickSound.length);
        }

        SceneManager.LoadScene(scenename);
    }

    public void InfoButton()
    {
        PlayClick();
        menupanel.SetActive(false);
        infopanel.SetActive(true);
    }

    public void BackButton()
    {
        PlayClick();
        menupanel.SetActive(true);
        infopanel.SetActive(false);
    }

    public void QuitButton()
    {
        PlayClick();
        if (exitConfirmPanel != null)
        {
            exitConfirmPanel.SetActive(true);
            menupanel.SetActive(false);
        }
    }

    public void ConfirmQuit()
    {
        Application.Quit();
        Debug.Log("...anda sudah keluar...");
    }

    public void CancelQuit()
    {
        PlayClick();
        if (exitConfirmPanel != null)
        {
            exitConfirmPanel.SetActive(false);
            menupanel.SetActive(true);
        }
    }

    public void KuisButton()
    {
        PlayClick();
        menupanel.SetActive(false);
        panelMenuKuis.SetActive(true); 
    }

    void PlayClick()
    {
        if (sfxAudioSource != null && buttonClickSound != null)
        {
            sfxAudioSource.PlayOneShot(buttonClickSound, 1f);
        }
    }
}
