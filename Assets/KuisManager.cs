using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class kuisManager : MonoBehaviour
{
    [Header("UI")]
    public Text pertanyaanText;
    public Image gambarSoalUI; // Tambahan baru
    public Text skorText;
    public Text skorAkhirText;
    public Button[] jawabButtons;
    public Button btnKembaliKeMenu;
    public Text waktuText;
    public TMP_InputField inputNama;

    [Header("Panel")]
    public GameObject Benar;
    public GameObject Salah;
    public GameObject GameSelesai;

    [Header("Canvas Panel")]
    public GameObject canvasQuizz;
    public GameObject panelMenuKuis;

    [Header("Data")]
    public KuisData kuisData;

    [Header("Timer")]
    public float waktuPerSoal = 0f;
    private float waktuTersisa;
    private bool soalBerjalan = false;
    private bool sudahMenjawab = false;

    private KuisQuestion[] soalAktif;
    private const int JUMLAH_SOAL_AKTIF = 10; // Hanya ambil 10 soal

    private int currentQuestion = 0;
    private static int score = 0;

    void Start()
    {
        Benar.SetActive(false);
        Salah.SetActive(false);
        GameSelesai.SetActive(false);
        btnKembaliKeMenu.gameObject.SetActive(false);
        soalBerjalan = false;

        if (canvasQuizz != null) canvasQuizz.SetActive(false);
    }

    void Update()
    {
        if (soalBerjalan && !sudahMenjawab)
        {
            waktuTersisa -= Time.deltaTime;
            UpdateWaktuUI();

            if (waktuTersisa <= 0)
            {
                waktuTersisa = 0;
                soalBerjalan = false;
                CheckReply(-1);
            }
        }
    }

public void MulaiKuis()
{
    score = 0;
    currentQuestion = 0;

    // Ambil 10 soal acak dari semua soal di kuisData
    List<KuisQuestion> semuaSoal = new List<KuisQuestion>(kuisData.questions);
    for (int i = 0; i < semuaSoal.Count; i++)
    {
        var temp = semuaSoal[i];
        int randomIndex = Random.Range(i, semuaSoal.Count);
        semuaSoal[i] = semuaSoal[randomIndex];
        semuaSoal[randomIndex] = temp;
    }

    soalAktif = semuaSoal.GetRange(0, Mathf.Min(JUMLAH_SOAL_AKTIF, semuaSoal.Count)).ToArray();

    foreach (KuisQuestion q in soalAktif)
    {
        ShuffleAnswers(q);
    }

    skorText.text = score.ToString();
    SetQuestion(currentQuestion);

    if (canvasQuizz != null) canvasQuizz.SetActive(true);
    if (panelMenuKuis != null) panelMenuKuis.SetActive(false);
}
void SetQuestion(int questionIndex)
{
    var question = soalAktif[questionIndex];

    // Menampilkan teks soal
    pertanyaanText.text = question.pertanyaanText;

    // ✅ Menampilkan gambar soal jika ada
    if (gambarSoalUI != null)
    {
        if (question.gambarPertanyaan != null)
        {
            gambarSoalUI.gameObject.SetActive(true);
            gambarSoalUI.sprite = question.gambarPertanyaan;
        }
        else
        {
            gambarSoalUI.gameObject.SetActive(false); // Sembunyikan jika tidak ada gambar
        }
    }

    sudahMenjawab = false;
    soalBerjalan = true;
    waktuTersisa = waktuPerSoal;
    UpdateWaktuUI();

    for (int i = 0; i < jawabButtons.Length; i++)
    {
        jawabButtons[i].onClick.RemoveAllListeners();
        jawabButtons[i].interactable = true;
        jawabButtons[i].GetComponentInChildren<Text>().text = question.jawaban[i];

        int replyIndex = i;
        jawabButtons[i].onClick.AddListener(() => CheckReply(replyIndex));
    }
}

    void CheckReply(int replyIndex)
    {
        if (sudahMenjawab) return;
        sudahMenjawab = true;
        soalBerjalan = false;

        foreach (Button r in jawabButtons)
        {
            r.interactable = false;
        }

        if (replyIndex == soalAktif[currentQuestion].correctReplyIndex)
        {
            score++;
            skorText.text = score.ToString();
            Benar.SetActive(true);
        }
        else
        {
            Salah.SetActive(true);
        }

        StartCoroutine(Next());
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(2);
        currentQuestion++;

        if (currentQuestion < soalAktif.Length)
        {
            Reset();
        }
        else
        {
            SelesaiKuis();
            GameSelesai.SetActive(true);
            skorAkhirText.gameObject.SetActive(true);

            float scorePercentage = (float)score / soalAktif.Length * 100f;
            skorAkhirText.text = "SKOR MU: " + scorePercentage.ToString("F0") + "%";

            if (scorePercentage < 10)
                skorAkhirText.text += "\nBelum jago main bola :( ";
            else if (scorePercentage < 50)
                skorAkhirText.text += "\nMasih kurang jago :( ";
            else if (scorePercentage < 70)
                skorAkhirText.text += "\nLumayan jago :)";
            else if (scorePercentage < 90)
                skorAkhirText.text += "\nSudah jago tapi sedikit lagi :D";
            else
                skorAkhirText.text += "\nExcellent!!!!!";

            btnKembaliKeMenu.gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        Benar.SetActive(false);
        Salah.SetActive(false);
        SetQuestion(currentQuestion);
    }

    public void KembaliKeMenu()
    {
        score = 0;
        SceneManager.LoadScene("main menu");
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < kuisData.questions.Length; i++)
        {
            int rand = Random.Range(i, kuisData.questions.Length);
            var temp = kuisData.questions[i];
            kuisData.questions[i] = kuisData.questions[rand];
            kuisData.questions[rand] = temp;
        }
    }

    void ShuffleAnswers(KuisQuestion question)
    {
        string[] originalAnswers = question.jawaban;
        int correctIndex = question.correctReplyIndex;

        List<int> indices = new List<int>();
        for (int i = 0; i < originalAnswers.Length; i++) indices.Add(i);

        for (int i = 0; i < indices.Count; i++)
        {
            int rand = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[rand];
            indices[rand] = temp;
        }

        string[] shuffledAnswers = new string[originalAnswers.Length];
        int newCorrectIndex = 0;

        for (int i = 0; i < indices.Count; i++)
        {
            shuffledAnswers[i] = originalAnswers[indices[i]];
            if (indices[i] == correctIndex) newCorrectIndex = i;
        }

        question.jawaban = shuffledAnswers;
        question.correctReplyIndex = newCorrectIndex;
    }

    void UpdateWaktuUI()
    {
        if (waktuText != null)
        {
            waktuText.text = Mathf.CeilToInt(waktuTersisa).ToString();
        }
    }

    public void SelesaiKuis()
    {
        int skorAkhir = score;
        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.SetLatestScore(skorAkhir);
        }
        else
        {
            Debug.LogError("LeaderboardManager.Instance belum tersedia di scene!");
        }
    }

    public void KonfirmasiNama()
    {
        string nama = inputNama != null ? inputNama.text : "";
        if (string.IsNullOrEmpty(nama))
        {
            Debug.LogWarning("Nama masih kosong!");
            return;
        }

        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.AddEntry(nama, score);
            Debug.Log("Data leaderboard ditambahkan: " + nama + " - " + score);

            var tampil = FindObjectOfType<TampilkanLeaderboard>();
            if (tampil != null)
            {
                tampil.enabled = false;
                tampil.enabled = true;
            }
        }
        else
        {
            Debug.LogError("LeaderboardManager.Instance tidak ditemukan!");
        }
    }
}
