using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start()
    {
        camera_flg = 0;

       DoorRigidBody = GetComponent<Rigidbody>();
        // 位置固定、回転の X と Y を固定。
        DoorRigidBody.constraints =
        RigidbodyConstraints.FreezeRotationX |
        RigidbodyConstraints.FreezeRotationZ |
        RigidbodyConstraints.FreezePositionY;
        //RigidbodyConstraints.None;//解除


        //ゲートカメラを初期化　オフにする
        gatecamera.enabled = false;
        maincamera.enabled = true;

        GamaObj = GameObject.Find("GameObject");
        script = GamaObj.GetComponent<SimplePun>();

        LeftDoor = GameObject.Find("gate_left");
        RightDoor = GameObject.Find("gate_right");


        //HingeJointコンポーネント取得
        this.myHingeJoint = GetComponent<HingeJoint>();

        //扉の傾きを設定
        SetAngle(this.defaultAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if((LeftDoor.transform.rotation.y <= 0.5) && (camera_flg == 0)){
            DoorRigidBody.constraints = RigidbodyConstraints.None;
            DoorRigidBody.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
            //RigidbodyConstraints.None;//解除

            if ( script.key_flg == 1)
            {
                //ゲートカメラをオンにする
                gatecamera.enabled = true;
                maincamera.enabled = false;
            }

            //SetAngle(this.flickAngle);)
            //左を動かす
            if (script.key_flg == 1)
            {
                LeftDoor.transform.Rotate(0, 1, 0);
                Debug.Log(LeftDoor.transform.rotation);

                RightDoor.transform.Rotate(0, -1, 0);
                //SetAngle(this.flickAngle);
            }
            //数秒だけゲートカメラ表示で固定

            if(camera_flg == 0)
            {
                sw();
            }
            DoorRigidBody.constraints = RigidbodyConstraints.None;

        }
        
        /*
        //矢印キー離された時フリッパーを元に戻す
        if (Input.GetKeyUp(KeyCode.LeftArrow) && tag == "LeftFripperTag")
        {
            SetAngle(this.defaultAngle);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && tag == "RightFripperTag")
        {
            SetAngle(this.defaultAngle);
        }
        */
    }

    //とびら傾きを設定
    public void SetAngle(float angle)
    {

        JointMotor jointMotor = this.myHingeJoint.motor;
        jointMotor.targetVelocity = 100;
        jointMotor.force = 3;
        this.myHingeJoint.motor = jointMotor;
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