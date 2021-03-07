using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class MyCameraController : MonoBehaviour
{
    

    //キャラオブジェクト
    private GameObject sastainer;


    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        //キャラオブジェクトを取得
        sastainer = GameObject.FindGameObjectWithTag("Player");

        Debug.Log(sastainer.transform.position.z);

        this.transform.position = new Vector3(this.sastainer.transform.position.x + 3f, this.sastainer.transform.position.y, this.sastainer.transform.position.z);
    }
}