using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponManager : MonoBehaviour
{

    public GameObject projectilePrefabHand, projectilePrefabBaseballBat, projectilePrefabPistol, projectilePrefabShotgun, projectilePrefabBazooka;
    public Transform firepointHand, firepointBaseballBat, firepointPistol, firepointShotgun;
    public GameObject handWeapon, baseballBat, pistol, shotgun, bazooka;
    public int previousWeapon, selectedWeapon = 0;
    private CameraShake camShake;
    public Image iconContainer;
    public bool handsUnlocked = true, baseballBatUnlocked = false, pistolUnlocked = false, shotgunUnlocked = false, bazookaUnlocked = false;
    public List<Sprite> iconsList;



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
            ScrollForth();
        }
        if (CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") < 0f || CrossPlatformInputManager.GetButtonDown("SwitchWeaponDown"))
        {
            ScrollBack();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && handsUnlocked)
        {
            SwitchToWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && baseballBatUnlocked)
        {
            SwitchToWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && pistolUnlocked)
        {
            SwitchToWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && shotgunUnlocked)
        {
            SwitchToWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && bazookaUnlocked)
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
                projectileClone = Instantiate(projectilePrefabHand, firepointHand.transform.position, firepointHand.transform.rotation) as GameObject;
                Destroy(projectileClone, 0.5f);

                camShake.Shake(0.025f, 0.1f, Camera.main);
            }
            else if (baseballBat.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = Instantiate(projectilePrefabBaseballBat, firepointBaseballBat.transform.position, firepointBaseballBat.transform.rotation) as GameObject;
                Destroy(projectileClone, 0.5f);

                camShake.Shake(0.04f, 0.1f, Camera.main);
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

            if (i == selectedWeapon && !weapon.name.Contains("Locked"))
            {
                weapon.gameObject.SetActive(true);
                iconContainer.sprite = iconsList[i];
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

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

    public void UnlockNewWeapon(string boolName)
    {
        if (boolName == "baseballBatUnlocked")
        {
            baseballBatUnlocked = true;
        }
        if (boolName == "pistolUnlocked")
        {
            pistolUnlocked = true;
        }
        if (boolName == "shotgunUnlocked")
        {
            shotgunUnlocked = true;
        }
    }

    public void ScrollForth()
    {
        if (selectedWeapon >= transform.childCount - 1)
        {
            selectedWeapon = 0;
        }
        else
        {
            selectedWeapon++;
        }
        SelectWeapon();
    }

    public void ScrollBack()
    {

        if (selectedWeapon <= 0)
        {
            selectedWeapon = transform.childCount - 1;
        }
        else
        {
            selectedWeapon--;
        }
        SelectWeapon();
    }


}