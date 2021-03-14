using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;



public class sustainerController : Photon.Pun.MonoBehaviourPun
{
    //　地面に接地しているかどうか
    [SerializeField]
    private bool isGrounded;

    //メインカメラとゴールカメラ
    public Camera gatecamera_sus;
    public Camera maincamera_sus;

    //joystick
    [SerializeField] VariableJoystick m_VariableJoystick;
    [SerializeField] Animator m_Animator;
    [SerializeField] float m_Speed = 1;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Direction;
    //private Camera m_Camera;

    //カギオブジェクトなど
    private GameObject m_key;
    private Collider m_key_collider;
    private Rigidbody m_key_Rigidbody;

    private Transform _transform;
    private float posi_y;

    //オンライン化に必要なコンポーネントを設定
    public PhotonView myPV;
    public PhotonTransformView myPTV;

    //カギの衝突音
    public AudioClip getkey;

    //カギを落とす音
    public AudioClip dropkey;

    //コインの衝突音
    public AudioClip getcoin;
    AudioSource audioSource;

    //ゲームマネージャ（GameObject）スクリプトにアクセス
    GameObject GamaObj;
    SimplePun script; //SimplePunScriptが入る変数

    GameObject GateObj;
    Opengate opengatescript; //SimplePunScriptが入る変数

    //ゴールのgameobject
    GameObject GoalGameObj;

    public Vector3 myTransform;

    private float pos_x;
    private float pos_z;
    private Animator myAnimator;
    private Animator myGAnimator;

    //ゲーム終了の判定
    private bool isEnd = false;


    // Use this for initialization
    void Start()
    {

        this.myAnimator = GetComponent<Animator>();


        if (myPV.IsMine)    //自キャラであれば実行
        {
            //Keyの取得フラグ
            GamaObj = GameObject.Find("GameObject");
            script = GamaObj.GetComponent<SimplePun>();

            GoalGameObj = GameObject.Find("GoalTxt");

            //扉にアタッチしているオープンゲートスクリプト取得
            GateObj = GameObject.Find("LeftCore");
            opengatescript = GateObj.GetComponent<Opengate>();

            //インスタンスにjoystickを登録
            m_VariableJoystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>();
            m_Rigidbody = GetComponent<Rigidbody>();

            m_key_Rigidbody = GameObject.Find("key").GetComponent<Rigidbody>();
            m_key = GameObject.Find("key");

            audioSource = GetComponent<AudioSource>();

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


        //keyを取得したらkeyは頭の上に表示する
        if(script.key_flg == 1)
        {

            m_key_Rigidbody.transform.position = new Vector3(m_Rigidbody.transform.position.x, m_Rigidbody.transform.position.y+ 2.3f, m_Rigidbody.transform.position.z);
        }

        if(m_key_Rigidbody.transform.position.y < 0.5)
        {
            m_key_Rigidbody.isKinematic = true;
            if(m_key_Rigidbody.transform.position.x < -50f)
            {
                pos_x = -50f;
            }
            else
            {
                pos_x = m_key_Rigidbody.transform.position.x;
            }
            if (m_key_Rigidbody.transform.position.x > 46.5f)
            {
                pos_x = 46.5f;
            }
            else
            {
                pos_x = m_key_Rigidbody.transform.position.x;
            }
            if (m_key_Rigidbody.transform.position.z > -23f)
            {
                pos_z = 23f;
            }
            else
            {
                pos_z = m_key_Rigidbody.transform.position.z;
            }
            if (m_key_Rigidbody.transform.position.z > 73f)
            {
                pos_z = 73f;
            }
            else
            {
                pos_z = m_key_Rigidbody.transform.position.z;
            }
            m_key_Rigidbody.transform.position = new Vector3(pos_x, m_key_Rigidbody.transform.position.y + 0.75f, pos_z);
        }



        //joystick
        if (m_Direction != new Vector3(0, 0, 0))
        {
            transform.localRotation = Quaternion.LookRotation(m_Direction);
            //m_Animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(m_Direction.x), Mathf.Abs(m_Direction.z)));
            m_Animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(m_Direction.x), Mathf.Abs(m_Direction.z)));
            //Debug.Log(Mathf.Max(Mathf.Abs(m_Direction.x)));
            //m_Rigidbody.MovePosition(m_Direction * m_Speed * Time.deltaTime);
            if (transform.position.y > 0)
            {
                posi_y = transform.position.y;
            }
            else
            {
                posi_y = 0f;
            }

            //m_Rigidbody.MovePosition(new Vector3( transform.position.x + m_VariableJoystick.Horizontal*8 * Time.deltaTime, posi_y * Time.deltaTime, transform.position.z + m_VariableJoystick.Vertical* 8 * Time.deltaTime));
            m_Rigidbody.velocity = transform.forward * m_Speed;
            if (isGrounded == false)
            {
                m_Rigidbody.AddForce(0, -100, 0, ForceMode.Acceleration);
            }
            
            myTransform = m_Rigidbody.transform.position;
            //地面との当たり判定で離れたらaddforceでY軸方向のちからを上から少しかけ、地面におしつける
            //addforceでまとめた方が、リアルに近い考え方で構築できる
            //制限速度をこえたら何もしない　というやりかたもいいかも
            //if (制限速度）{ // 加速しない; }

            //m_Rigidbody.AddForce(transform.forward * m_Speed, ForceMode.Impulse);//吹っ飛んでいく
            //m_Rigidbody.AddForce(transform.forward * m_Speed, ForceMode.Force);//止めるときはaddforceを使用して逆向きの力をかける
            //m_Rigidbody.AddForce(transform.forward * m_Speed, ForceMode.Acceleration);

            //物英演算との合成処理　車のブレーキのような逆向きの力　back
        }
        else
        {
            m_Animator.SetFloat("Speed", 0);
            //transform.position += transform.forward * 0.05f;
        }

        /*
        //joystick
        if (m_Direction != new Vector3(0, 0, 0))
        {
            transform.localRotation = Quaternion.LookRotation(m_Direction);
        }
        m_Animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(m_Direction.x), Mathf.Abs(m_Direction.z)));

        var charaTransformRotate = m_Rigidbody.transform.localRotation.eulerAngles;
        var q = Quaternion.Euler(0.0f, charaTransformRotate.y, 0.0f);
        Vector3 y_Direction = q.eulerAngles;

        m_Rigidbody.Move(m_Direction * m_Speed * Time.deltaTime);

        var angle = m_Camera.transform.localRotation.eulerAngles;
        */


    }

    public void FixedUpdate()
    {

        m_Direction = Vector3.forward * m_VariableJoystick.Vertical + Vector3.right * m_VariableJoystick.Horizontal;
        

    }


    private void OnTriggerEnter(Collider other)
    {
        //コインに衝突した場合（追加）
        if (other.gameObject.tag == "key")
        {
            script.key_flg = 1;
            //重力無効
            m_key_Rigidbody.isKinematic = true;
            m_key_collider = m_key.GetComponent<Collider>();
            m_key_collider.isTrigger = false;

            audioSource.clip = getkey;
            audioSource.Play();

        }



        //コインに衝突した場合
        if (other.gameObject.tag == "CoinTag")
        {
            Debug.Log("coin");
            //スコアを加算
            //this.score += 10;

            //ScoreTextに獲得した点数を表示
            //this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";

            //パーティクルの再生
            GetComponent<ParticleSystem>().Play();

            Debug.Log(other.gameObject.tag);
            //コインゲットの音
            audioSource.clip = getcoin;
            audioSource.Play();

            //接触したコインのオブジェクトを破棄
            Destroy(other.gameObject);
        }

        //ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            opengatescript.gatecamera.enabled = true;
            opengatescript.maincamera.enabled = false;
            this.isEnd = true;
            myGAnimator = GoalGameObj.GetComponent<Animator>();
            myGAnimator.enabled = true;
            this.myAnimator.enabled = true;
            this.myAnimator.SetBool("Rising", true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //　地面に着地したかどうかの判定
        if (collision.gameObject.tag != "graund")
        {
            isGrounded = true;
        }


        //コインに衝突した場合（追加）
        if (collision.gameObject.tag == "dog")
        {
            if (script.key_flg != 0)
            {
                //重力有効
                 m_key_Rigidbody.isKinematic = false;

                Debug.Log(collision.gameObject.tag);
                float rx = Random.Range(2f, 6f);
                float ry = Random.Range(2f, 6f);
                float rz = Random.Range(2f, 6f);
                m_key_Rigidbody.AddForce(rx,ry,rz, ForceMode.Impulse);
            
                m_key_collider = m_key.GetComponent<Collider>();
                m_key_collider.isTrigger = true;

                //カギを持ってなかったら敵にあたっても音はならなさない

                audioSource.clip = dropkey;
                audioSource.Play();
            }
            script.key_flg = 0;

        }
    }
 
    private void OnCollisionExit(Collision collision){
            //　地面から降りた時の処理
            isGrounded = false;
    }

}
