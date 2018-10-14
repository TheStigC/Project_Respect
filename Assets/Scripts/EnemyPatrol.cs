using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    public float movementSpeed, startWaitTime;
    private float waitTime;

    public Transform[] moveSpots;
    private int nextSpot;


    void Start()
    {
        waitTime = startWaitTime;
        nextSpot = 0;
    }


    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[nextSpot].position, movementSpeed * Time.fixedDeltaTime);

        Vector3 targetPosition = new Vector3(moveSpots[nextSpot].position.x, transform.position.y, moveSpots[nextSpot].position.z);
        transform.LookAt(targetPosition);


        if (Vector3.Distance(transform.position, moveSpots[nextSpot].position)<0.2f)
        {
            if (waitTime <= 0)
            {
                if (nextSpot >= moveSpots.Length-1)
                {
                    nextSpot = 0;
                }
                else
                {
                    nextSpot += 1;
                }

                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
