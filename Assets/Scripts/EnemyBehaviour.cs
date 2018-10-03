using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{


    public float speed, stoppingDistance, retreatDistance, attackSpeed, rotationSpeed;
    private float timeBetweenShots;
    public Transform player;
    public GameObject projectile, firePoint;





    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenShots = attackSpeed;
    }


    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //FIND PLAYER ROTATION AND ROTATE TOWARDS HIM
        Vector3 targetDir = player.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);



        //CHASE A PLAYER
        if (Vector3.Distance(transform.position, player.position) >= stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        //STAY IDLE
        else if (Vector3.Distance(transform.position, player.position) <= stoppingDistance && Vector3.Distance(transform.position, player.position) >= retreatDistance)
        {
            transform.position = this.transform.position;
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        //RETREAT
        else if (Vector3.Distance(transform.position, player.position) <= retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDir);
        }


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
}
