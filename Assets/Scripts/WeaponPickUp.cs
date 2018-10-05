using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour {

    public GameObject weaponOnTheGround;
    public GameObject weapon;
    public GameObject currentWeapon;
    public GameObject weaponContainer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon")
        {
            WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
            Debug.Log("jeg collider");
           currentWeapon.SetActive(false);
           weapon.SetActive(true);
           weapon.transform.SetParent(weaponContainer.transform);
           weaponManager.selectedWeapon = weaponContainer.transform.childCount - 1;  
           weaponOnTheGround.SetActive(false);

        }
    }
}
