using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opengate : MonoBehaviour
{
    //Photon　BGM　用
    GameObject GamaObj;
    SimplePun script; //SimplePunScriptが入る変数

    //メインカメラとゴールカメラ
    public Camera gatecamera;
    public Camera maincamera;


    //HingeJointコンポーネントを入れる
    private HingeJoint myHingeJoint;

    //初期の傾き
    private float defaultAngle = 0;
    //弾いた時の傾き
    private float flickAngle = 90;

    //ゲートの左右扉
    private GameObject LeftDoor;
    private GameObject RightDoor;

    private Rigidbody DoorRigidBody;

    public int camera_flg;

    private Animator myAnimator;




    // Use this for initialization
    void Start()
    {
        this.myAnimator = GetComponent<Animator>();

        myAnimator.enabled = false;
        camera_flg = 0;


        //ゲートカメラを初期化　オフにする
        gatecamera.enabled = false;
        maincamera.enabled = true;

        //キーフラグを取得するためSimplePunを利用している
        GamaObj = GameObject.Find("GameObject");
        script = GamaObj.GetComponent<SimplePun>();

    }

    // Update is called once per frame
    void Update()
    {

        if((camera_flg == 0) && (script.key_flg == 1))
        {
            Debug.Log(camera_flg);
            //ゲートカメラをオンにする
            gatecamera.enabled = true;
            maincamera.enabled = false;
            sw();
            myAnimator.enabled = true;
            this.myAnimator.SetBool("ROpen", true);
            this.myAnimator.SetBool("LOpen", true);
            camera_flg = 1;
        }
        
    }



    void sw()
    {
        if(gatecamera.enabled == true)
        {
            Invoke("MainCameraOn", 3.0f);
        }
        
    }


    void MainCameraOn()
    {
        camera_flg = 1;
        gatecamera.enabled = false;
        maincamera.enabled = true;
    }


}