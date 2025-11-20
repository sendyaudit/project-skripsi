using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Kelas ini merepresentasikan satu entri leaderboard (nama dan skor)
[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;

    public LeaderboardEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

// Wrapper yang dibutuhkan agar daftar entri bisa diserialisasi menjadi JSON
[System.Serializable]
public class LeaderboardWrapper
{
    public List<LeaderboardEntry> entries;

    public LeaderboardWrapper(List<LeaderboardEntry> entries)
    {
        this.entries = entries;
    }
}

// Script utama untuk mengelola leaderboard
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance; // Objek tunggal (singleton) agar bisa diakses dari mana saja


    [Header("UI References")]
    public TMP_InputField nameInputField;// Input untuk nama pemain
    public Transform leaderboardContent;// Tempat menampilkan daftar pemain di UI
    public GameObject leaderboardEntryPrefab;// Prefab tampilan 1 entri pemain
    public Button confirmButton;// Tombol untuk mengirim nama

    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();// Daftar semua pemain dan skor
    private int latestScore = 0;// Skor terbaru dari pemain

    private const string LEADERBOARD_KEY = "leaderboard_data";// Nama penyimpanan data di PlayerPrefs
    private const int MAX_ENTRIES = 5;// Maksimal pemain yang ditampilkan

    public List<LeaderboardEntry> Leaderboard => leaderboard;// Mengambil data leaderboard
    public TampilkanLeaderboard tampilkanLeaderboard;// Script lain untuk menampilkan leaderboard (kalau ada)

    void Awake()
    {
        if (Instance == null) //Cek apakah sudah ada instance LeaderboardManager
        {
            Instance = this; //Kalau belum ada, jadikan objek ini sebagai instance utama
            DontDestroyOnLoad(gameObject); //Buat objek ini tetap ada walaupun pindah scene
            LoadLeaderboard(); //Muat data leaderboard yang tersimpan sebelumnya
        }
        else
        {
            Destroy(gameObject);//Kalau sudah ada instance, hapus yang ini (hindari duplikat)
        }
    }

    // Simpan skor terbaru sebelum ditambahkan ke leaderboard
    public void SetLatestScore(int score)
    {
        latestScore = score;
    }

    // Fungsi saat tombol "kirim nama" ditekan
    public void SubmitNama()
    {
        string nama = nameInputField != null ? nameInputField.text.Trim() : "";

        if (string.IsNullOrEmpty(nama))
        {
            Debug.LogWarning("Nama tidak boleh kosong!");
            return;
        }

        AddEntry(nama, latestScore);
        nameInputField.text = ""; 
        if (confirmButton != null)
            confirmButton.interactable = false;
    }

    public void AddEntry(string name, int score)
    {
        if (string.IsNullOrWhiteSpace(name)) return;

        // Jika leaderboard belum penuh, langsung tambah
        if (leaderboard.Count < MAX_ENTRIES)
        {
            leaderboard.Add(new LeaderboardEntry(name, score));
        }
        else
        {
            // Urutkan dari skor tertinggi ke terendah
            leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
            LeaderboardEntry lastEntry = leaderboard[leaderboard.Count - 1];

            if (score > lastEntry.score)
            {
                leaderboard[leaderboard.Count - 1] = new LeaderboardEntry(name, score);
            }
            else if (score == lastEntry.score)
            {
                // Jika skornya sama, entri baru menggantikan entri lama
                leaderboard[leaderboard.Count - 1] = new LeaderboardEntry(name, score);
            }
            else
            {
                Debug.Log($"⛔ Skor {score} terlalu rendah untuk masuk ke leaderboard.");
                return;
            }
        }

        // Urutkan ulang leaderboard
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
        SaveLeaderboard();
        UpdateLeaderboardUI();

        if (tampilkanLeaderboard != null)
            tampilkanLeaderboard.ShowLeaderboard();

        Debug.Log($"✅ Data leaderboard ditambahkan: {name} - {score}");
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new LeaderboardWrapper(leaderboard));
        PlayerPrefs.SetString(LEADERBOARD_KEY, json);
        PlayerPrefs.Save();
    }

    public void LoadLeaderboard()
    {
        leaderboard.Clear();
        string json = PlayerPrefs.GetString(LEADERBOARD_KEY, "");

        if (!string.IsNullOrEmpty(json))
        {
            LeaderboardWrapper wrapper = JsonUtility.FromJson<LeaderboardWrapper>(json);
            if (wrapper != null && wrapper.entries != null)
                leaderboard = wrapper.entries;
        }

        UpdateLeaderboardUI();
    }

    public void UpdateLeaderboardUI()
    {
        if (leaderboardContent == null || leaderboardEntryPrefab == null)
        {
            Debug.LogWarning("⚠️ Leaderboard UI not set up properly.");
            return;
        }

        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < leaderboard.Count; i++)
        {
            GameObject obj = Instantiate(leaderboardEntryPrefab, leaderboardContent);

            TMP_Text textField = obj.GetComponentInChildren<TMP_Text>();
            if (textField != null)
            {
                textField.text = $"{i + 1}. {leaderboard[i].name.ToUpper()} - {leaderboard[i].score}";
            }
            else
            {
                Debug.LogWarning("⚠️ LeaderboardEntry prefab is missing TMP_Text component!");
            }
        }
    }

    public void ResetLeaderboard()
    {
        leaderboard.Clear(); // Kosongkan daftar leaderboard
        PlayerPrefs.DeleteKey(LEADERBOARD_KEY); // Hapus data yang tersimpan
        PlayerPrefs.Save(); // Simpan perubahan
        LoadLeaderboard(); // Tambahkan ini untuk memastikan leaderboard di-refresh secara menyeluruh
        
        Debug.Log("🧹 Leaderboard berhasil direset tanpa konfirmasi.");
    }

    public void TampilkanLeaderboard()
    {
        LoadLeaderboard();
    }
}
