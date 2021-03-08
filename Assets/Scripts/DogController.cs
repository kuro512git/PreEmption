using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogController : MonoBehaviour
{
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;
    [SerializeField] GameObject target4;
    public GameObject tmpTarget;
    public GameObject xTarget;

    GameObject[] array = new GameObject[8];

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

        array[0] = target1;
        array[1] = target2;
        array[2] = target3;
        array[3] = target4;
        array[4] = target1;
        array[5] = target2;
        array[6] = target3;
        array[7] = target4;
        int num = Random.Range(0, 7);
        xTarget = array[num];
    }

    void Update()
    {
        //destination
        myAgent.SetDestination(xTarget.transform.position);

    }

    private void turn_dog()
    {
        // 次に目指すべき位置を取得
        //steeringTarget
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
            //anim.SetBool("Attack" + number, true);
            Debug.Log(other.gameObject.tag);



            int number = Random.Range(0, 7);
            tmpTarget = array[number];

            //今と次のターゲットを比べる
            if (tmpTarget != xTarget)
            {
                xTarget = tmpTarget;
            }
            else
            {
                //今と次のターゲットが同じだったら入れ替わるまでやりなおし
                do
                {
                    number = Random.Range(0, 6);
                    tmpTarget = array[number];
                } while (tmpTarget == xTarget);
                xTarget = tmpTarget;
            }
            
        }
    }
}


