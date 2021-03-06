using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 制御するVideo Playerのリスト
    /// </summary>
    [SerializeField]
    private List<VideoPlayer> playerList;
    /// <summary>
    /// 再生
    /// </summary>
    public void Play()
    {
        foreach (VideoPlayer player in playerList)
        {
            if (!player.isPlaying)
            {
                player.Play();
            }
        }
    }
    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        foreach (VideoPlayer player in playerList)
        {
            if (player.isPlaying)
            {
                player.Pause();
            }
        }
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        foreach (VideoPlayer player in playerList)
        {
            player.Stop();
        }
    }
}