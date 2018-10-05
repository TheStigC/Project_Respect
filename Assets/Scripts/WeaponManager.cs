using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponManager : MonoBehaviour
{

    public GameObject projectilePrefabHand;
    public GameObject projectilePrefabPistol;
    public Transform handFirePoint;
    public Transform firepointPistol;
    public GameObject pistol;
    public GameObject handWeapon;
    public int selectedWeapon = 0;


    void Start()
    {
        SelectWeapon();
    }


    void Update()
    {
        int previusWeapon = selectedWeapon;
        if (CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") > 0f|| CrossPlatformInputManager.GetButtonDown("SwitchWeaponUp"))
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
        }
            }
            if (CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") < 0f || CrossPlatformInputManager.GetButtonDown("SwitchWeaponDown"))
            {
                Debug.Log("jeg ruller ned");
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = transform.childCount - 1;
                }
                else
                {
                    selectedWeapon--;
                }
            }
            if(previusWeapon != selectedWeapon)
            {
                SelectWeapon();
            }

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {

            if (handWeapon.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = Instantiate(projectilePrefabHand, handFirePoint.transform.position, handFirePoint.transform.rotation) as GameObject;
                Destroy(projectileClone, 3f);
            }
            else if (pistol.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = Instantiate(projectilePrefabPistol, firepointPistol.transform.position, firepointPistol.transform.rotation) as GameObject;
                Destroy(projectileClone, 3f);
            }
        }
    }




    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon) weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}