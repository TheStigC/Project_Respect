using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CivBehaviour : MonoBehaviour
{
    public GameObject firstPatrolPoint;
    NavMeshAgent agent;

    void Start()
    {

        firstPatrolPoint = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        //Random tal er ikke nødvendigt, det er bare en test om de så finder forskellige mål
        float waitTime = Random.Range(3, 5);
        InvokeRepeating("LookForPlayer", 0.5f, waitTime);
    }
    void Update()
    {


    }


    void LookForPlayer()
    {
        if (firstPatrolPoint == null)
        {
            firstPatrolPoint = GameObject.FindGameObjectWithTag("Player");
        }
        else
            CancelInvoke();
    }

    void SetFocus()
    {
        FollowTarget(firstPatrolPoint.GetComponent<Target>());
        firstPatrolPoint.GetComponent<Target>().OnFococused(transform);
    }


    public void FollowTarget(Target newTarget)
    {
        agent.SetDestination(firstPatrolPoint.transform.position);
        FaceTarget();
        agent.updateRotation = false;
        // player.transform = newTarget.playerTransform;
    }
    void FaceTarget()
    {
        Vector3 direction = (firstPatrolPoint.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
