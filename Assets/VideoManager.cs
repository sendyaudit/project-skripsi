using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class VideoData
{
    public string namaVideo;
    public string videoId;
}

public class VideoManager : MonoBehaviour
{
    public LINKVIDIO linkVidioScript;
    public List<VideoData> daftarVideo = new List<VideoData>();

    public void PutarVideoByNama(string namaVideo)
    {
        foreach (VideoData data in daftarVideo)
        {
            if (data.namaVideo == namaVideo)
            {
                linkVidioScript.TampilkanVideo(data.videoId);
                return;
            }
        }

        Debug.LogWarning("Video dengan nama " + namaVideo + " tidak ditemukan!");
    }
}
