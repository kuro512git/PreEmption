using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamingVideoPlayer : MonoBehaviour
{
    private GameObject obj;
    private RawImage videoBase;
    public VideoPlayer videoPlayer;
    private new GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("videoBase");
        videoBase = obj.GetComponent<RawImage>();

        RenderTexture renderTexture;
        renderTexture = new RenderTexture(960, 540, 0);

        camera = GameObject.Find("Main Camera");

        videoBase.material.mainTexture = renderTexture;

        if (videoPlayer == null) videoPlayer = camera.AddComponent<VideoPlayer>();

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.url = "http://printworks.sakura.ne.jp/ishiguro/sample.mp4";
        videoPlayer.errorReceived += ErrorReceived;
        videoPlayer.prepareCompleted += PrepareCompleted;
        videoPlayer.Prepare();
        videoPlayer.isLooping = false;
        videoPlayer.aspectRatio = VideoAspectRatio.Stretch;
        videoPlayer.targetCameraAlpha = 1;
        videoPlayer.loopPointReached += EndReached;
    }

    // エラー発生時に呼ばれる
    private void ErrorReceived(VideoPlayer vp, string message)
    {
        Debug.Log("エラー発生");
        vp.errorReceived -= ErrorReceived;
        vp.prepareCompleted -= PrepareCompleted;
        Destroy(videoPlayer);
        vp = null;
    }

    //　動画の読み込みが完了したら呼ばれる
    void PrepareCompleted(VideoPlayer vp)
    {
        vp.prepareCompleted -= PrepareCompleted;
        Debug.Log("ロード完了");
        vp.Play();
    }

    // 動画再生完了時に呼ばれる
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("再生完了");
        vp.errorReceived -= ErrorReceived;
        Destroy(videoPlayer);
        videoPlayer = null;
        // 動画再生完了時の処理
    }

    // Update is called once per frame
    void Update()
    {

    }
}