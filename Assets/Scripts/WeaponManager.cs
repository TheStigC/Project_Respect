using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponManager : MonoBehaviour {

    public GameObject projectilePrefab;
    public Transform firePoint;


	void Start ()
    {
		
	}
	

	void Update ()
    {

		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
            GameObject projectileClone;
            projectileClone = Instantiate(projectilePrefab, firePoint.transform.position,firePoint.transform.rotation) as GameObject;
            Destroy(projectileClone, 3f);
		}

	}
}
