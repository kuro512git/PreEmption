using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogController : MonoBehaviour
{
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;
    public GameObject xTarget;

    private NavMeshAgent myAgent;


    void Start()
    {
        // Nav Mesh Agent を取得します。
        myAgent = GetComponent<NavMeshAgent>();

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }


        //NavMeshPath path;
        var path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target1.transform.position, NavMesh.AllAreas, path);

        var length = path.corners[path.corners.Length - 1] - target1.transform.position;
        if (length.magnitude > 1.0f)
        {
            Debug.Log("到達しません");
        }

    }

    void Update()
    {

        // 次に目指すべき位置を取得
        var nextPoint = myAgent.steeringTarget;
        Vector3 targetDir = nextPoint - transform.position;

        // その方向に向けて旋回する(120度/秒)
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

        // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < 30f)
        {
            transform.position += transform.forward * 5.0f * Time.deltaTime;
            // もしもの場合の補正
            //if (Vector3.Distance(nextPoint, transform.position) < 0.5f) transform.position = nextPoint;
        }

        // targetに向かって移動します。
        myAgent.SetDestination(target1.transform.position);
        myAgent.nextPosition = transform.position;

    }

    private void OnTriggerEnter(Collider other)
    {


        //　ターゲットに接触したかどうかの判定
        if (other.gameObject.tag == "target")
        {
            myAgent.transform.position = new Vector3(myAgent.transform.position.x + 20, myAgent.transform.position.y, myAgent.transform.position.z);
            int number = Random.Range(1, 3);
            //anim.SetBool("Attack" + number, true);
            //Debug.Log(other.gameObject.tag);
            
            switch (number)
            {
                case 1: xTarget = target1; break;
                case 2: xTarget = target2; break;
                case 3: xTarget = target3; break;
            }

            Debug.Log(xTarget);

            //NavMeshPath path;
            var path2 = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, xTarget.transform.position, NavMesh.AllAreas, path2);
            /*
            var length = path.corners[path.corners.Length - 1] - xTarget.transform.position;
            if (length.magnitude > 1.0f)
            {
                Debug.Log("到達しません");
            }
            */
        }
    }
}


