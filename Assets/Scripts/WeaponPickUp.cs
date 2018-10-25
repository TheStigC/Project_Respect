using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : MonoBehaviour
{

    public GameObject weaponOnTheGround;
    public GameObject weapon;
    public GameObject weaponContainer;
    public int weaponSlot;
    public string unlockString;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon")
        {
            WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
            weaponManager.SwitchToWeapon(weaponSlot);
            weaponManager.UnlockNewWeapon(unlockString);
            weapon.SetActive(true);
            Destroy(weaponContainer.transform.GetChild(weaponSlot).gameObject);
            weapon.transform.SetParent(weaponContainer.transform);
            weapon.transform.SetSiblingIndex(weaponSlot);
            weaponOnTheGround.SetActive(false);
        }
    }
}
