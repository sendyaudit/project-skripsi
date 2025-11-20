using UnityEngine;
using TMPro;

public class TampilkanLeaderboard : MonoBehaviour
{
    public TMP_Text leaderboardText;

    void OnEnable()
    {
        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        if (leaderboardText == null || LeaderboardManager.Instance == null) return;

        var data = LeaderboardManager.Instance.Leaderboard;
        string hasil = "";

        for (int i = 0; i < 5; i++)
        {
            if (i < data.Count)
                hasil += $"{i + 1}. {data[i].name} - {data[i].score}\n";
            else
                hasil += $"{i + 1}. -\n";
        }

        leaderboardText.text = hasil;
    }
}
