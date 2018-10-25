using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBehaviour : MonoBehaviour
{
    public float speed, stoppingDistance, retreatDistance, attackSpeed, rotationSpeed;
    private float timeBetweenShots;
    public GameObject player;
    public GameObject projectile, firePoint;
    NavMeshAgent agent;





    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        timeBetweenShots = attackSpeed;
    }


    void Update()
    {
        if (player != null)
        {
            FollowTarget(player.GetComponent<Target>());
        }
        /*  //FIND PLAYER ROTATION AND ROTATE TOWARDS HIM
          Vector3 targetDir = player.position - transform.position;
          float step = rotationSpeed * Time.deltaTime;
          Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);


          //RETREAT
          if (Vector3.Distance(transform.position, player.position) <= retreatDistance)
          {
              transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
              transform.rotation = Quaternion.LookRotation(newDir);
          }*/


        //SHOOT
        if (timeBetweenShots <= 0)
        {
            Instantiate(projectile, firePoint.transform.position, this.transform.rotation);
            timeBetweenShots = attackSpeed;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }


    }
    void SetFocus()
    {
        FollowTarget(player.GetComponent<Target>());
        player.GetComponent<Target>().OnFococused(transform);
    }
        
    
    public void FollowTarget(Target newTarget)
    {
        agent.SetDestination(player.transform.position);
        FaceTarget();
        agent.stoppingDistance = newTarget.radius * .8f;
        agent.updateRotation = false;
       // player.transform = newTarget.playerTransform;
    }
    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

