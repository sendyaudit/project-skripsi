using UnityEngine;

public class AutoHidePanel : MonoBehaviour
{
    public GameObject targetPanel;      // Panel yang ingin ditampilkan dan disembunyikan
    public float tampilSelama = 2f;     // Durasi panel ditampilkan (default 2 detik)

    public void TampilkanPanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);      // Aktifkan panel
            CancelInvoke("SembunyikanPanel"); // Pastikan tidak dobel invoke
            Invoke("SembunyikanPanel", tampilSelama); // Sembunyikan setelah 2 detik
        }
    }

    private void SembunyikanPanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
    }
}
