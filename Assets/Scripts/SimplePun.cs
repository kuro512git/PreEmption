﻿using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class SimplePun : MonoBehaviourPunCallbacks
{

    //Keyの取得フラグ
    public int key_flg;



    // Use this for initialization
    void Start()
    {
        key_flg = 0;

        //旧バージョンでは引数必須でしたが、PUN2では不要です。
        PhotonNetwork.ConnectUsingSettings();
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }


    //ルームに入室前に呼び出される
    public override void OnConnectedToMaster()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    //ルームに入室後に呼び出される
    public override void OnJoinedRoom()
    {
        //キャラクターを生成
        GameObject sustainer_obj = PhotonNetwork.Instantiate("sustainer_prefab", Vector3.zero, Quaternion.identity, 0);
        
        //自分だけが操作できるようにスクリプトを有効にする
        sustainerController sustainScript = sustainer_obj.GetComponent<sustainerController>();
        sustainScript.enabled = true;
    }
}
