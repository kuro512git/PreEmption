using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;



public class CornController : Photon.Pun.MonoBehaviourPun
{

    //オンライン化に必要なコンポーネントを設定
    public PhotonView myPV;
    public PhotonTransformView myPTV;


    // Start is called before the first frame update
    void Start()
    {
        if (myPV.IsMine)    //自キャラであれば実行
        {

        }
        else
        {
            gameObject.SetActive (false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //自キャラであれば実行
        if (!myPV.IsMine)
        {
            return;
        }
        gameObject.transform.rotation = Quaternion.Euler( 0, 0, 0);
    }
}
