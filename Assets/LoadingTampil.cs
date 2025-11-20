using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingTampil : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarfill;

    private float targetProgress = 0f;
    private float fillSpeed = 4f; // Semakin kecil nilainya, semakin lambat gerakannya

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        LoadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Tingkatkan nilai fillAmount secara bertahap menuju targetProgress.
            while (LoadingBarfill.fillAmount < targetProgress)
            {
                LoadingBarfill.fillAmount += fillSpeed * Time.deltaTime;
                yield return null;
            }

            yield return null;
        }

        // Penuhi loading bar hingga 100%
        targetProgress = 1f;
        while (LoadingBarfill.fillAmount < targetProgress)
        {
            LoadingBarfill.fillAmount += fillSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;
    }
}
