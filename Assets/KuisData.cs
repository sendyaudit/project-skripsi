using UnityEngine;

//newkuisdata
[System.Serializable]
public class KuisQuestion
{
    public string pertanyaanText;
    public string[] jawaban;
    public int correctReplyIndex;
    public Sprite gambarPertanyaan;
}

[CreateAssetMenu(fileName = "KuisData", menuName = "Quiz/Kuis Data")]
public class KuisData : ScriptableObject
{
    public KuisQuestion[] questions;
}
