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


    //joystick
    [SerializeField] VariableJoystick m_VariableJoystick;
    [SerializeField] Animator m_Animator;
    [SerializeField] float m_Speed = 1;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Direction;
    private Camera m_Camera;

    private Transform _transform;
    private float posi_y;

    //オンライン化に必要なコンポーネントを設定
    public PhotonView myPV;
    public PhotonTransformView myPTV;


    // Use this for initialization
    void Start()
    {
        if (myPV.IsMine)    //自キャラであれば実行
        {
            // transformに毎回アクセスすると重いので、キャッシュしておく
            _transform = transform;

            //インスタンスにjoystickを登録
            m_VariableJoystick = GameObject.Find("Variable Joystick").GetComponent<VariableJoystick>();
            m_Rigidbody = GetComponent<Rigidbody>();

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
                m_Rigidbody.AddForce(0, -50, 0, ForceMode.Acceleration);
            }
            
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
            //接触したコインのオブジェクトを破棄（追加）
            Destroy(other.gameObject);
        }


        //コインに衝突した場合
        if (other.gameObject.tag == "CoinTag")
        {
            //スコアを加算
            //this.score += 10;

            //ScoreTextに獲得した点数を表示
            //this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";

            //パーティクルの再生
            GetComponent<ParticleSystem>().Play();

            //接触したコインのオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        //　地面に着地したかどうかの判定
        if (collision.gameObject.tag != "graund")
        {
            isGrounded = true;
        }
    }
 
    private void OnCollisionExit(Collision collision){
            //　地面から降りた時の処理
            isGrounded = false;
    }

}
