using UnityEngine;
using UnityEngine.UI;

public class NumberLooper : MonoBehaviour
{
    public Text textUI; // Drag & drop UI Text dari Canvas ke sini di Inspector

    void Start()
    {
        string hasil = "";

        // Perulangan dari 1 sampai 10
        for (int i = 1; i <= 10; i++)
        {
            hasil += i + "\n"; // Tambahkan angka ke string dengan baris baru
        }

        textUI.text = hasil; // Tampilkan hasil di UI Text
    }
}

