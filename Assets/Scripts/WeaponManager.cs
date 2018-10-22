using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponManager : MonoBehaviour
{

    public GameObject projectilePrefabHand, projectilePrefabPistol, projectilePrefabShotgun;
    public Transform handFirePoint, firepointPistol, firepointShotgun;
    public GameObject handWeapon, pistol, shotgun;
    public int previousWeapon, selectedWeapon = 0;
    private CameraShake camShake;


    void Start()
    {
        SelectWeapon();
        camShake = Camera.main.GetComponent<CameraShake>();
    }


    void Update()
    {
        //SELECTING WEAPON
        previousWeapon = selectedWeapon;
        if (CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") > 0f || CrossPlatformInputManager.GetButtonDown("SwitchWeaponUp"))
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            SwitchToWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            SwitchToWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            SwitchToWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
        {
            SwitchToWeapon(4);
        }

        if (previousWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

        //SHOOTING
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {

            if (handWeapon.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = Instantiate(projectilePrefabHand, handFirePoint.transform.position, handFirePoint.transform.rotation) as GameObject;
                Destroy(projectileClone, 3f);

                camShake.Shake(0.025f, 0.1f, Camera.main);
            }
            else if (pistol.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = Instantiate(projectilePrefabPistol, firepointPistol.transform.position, firepointPistol.transform.rotation) as GameObject;
                Destroy(projectileClone, 3f);

                camShake.Shake(0.04f, 0.1f, Camera.main);
            }
            else if (shotgun.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = Instantiate(projectilePrefabShotgun, firepointShotgun.transform.position, firepointShotgun.transform.rotation) as GameObject;
                Destroy(projectileClone, 3f);

                camShake.Shake(0.08f, 0.1f, Camera.main);
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

    public void SwitchToWeapon(int weaponSlot)
    {
        selectedWeapon = weaponSlot;
        if (previousWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
}