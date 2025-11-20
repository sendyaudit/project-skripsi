using UnityEngine;
using UnityEngine.UI;

public class LINKVIDIO : MonoBehaviour
{
    private WebViewObject webViewObject;
    private bool isVideoVisible = false;


    public GameObject tombolKembaliUI;   // Tombol UI kembali
    public GameObject canvasUtama;       // <- Tambahkan referensi canvas utama di sini

void Update()
{
    if (Application.platform == RuntimePlatform.Android)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isVideoVisible)
            {
                TutupVideo();
                return;
            }
        }
    }
}

public void TampilkanVideo(string videoId)
{
    if (webViewObject == null)
    {
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init();
        webViewObject.SetMargins(0, 0, 0, 0);
    }

    string url = $"https://www.youtube.com/watch?v={videoId}&autoplay=1&controls=1";
    webViewObject.LoadURL(url);
    webViewObject.SetVisibility(true);
    isVideoVisible = true;

    tombolKembaliUI.SetActive(true);
    canvasUtama.SetActive(false);
}

public void TutupVideo()
{
    if (webViewObject != null)
    {
        Destroy(webViewObject.gameObject); // HANCURKAN WebView sepenuhnya
        webViewObject = null;
        isVideoVisible = false;
    }

    tombolKembaliUI.SetActive(false);
    canvasUtama.SetActive(true);
}
}
