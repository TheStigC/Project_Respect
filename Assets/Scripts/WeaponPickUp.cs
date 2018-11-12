using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Geo.Respect
{
    public class WeaponPickUp : MonoBehaviour
    {
        public float respawnTime = 3f;
        public GameObject weaponOnTheGround;
        public GameObject weapon;
        public GameObject weaponContainer;
        public int weaponSlot;
        public string unlockString;
        WeaponManager weaponManager;
        WeaponManagerMultiplayer weaponManagerMP;
        public bool firstTimeInteracting = false;

        private void Start()
        {
            weaponManager = FindObjectOfType<WeaponManager>();
            weaponManagerMP = FindObjectOfType<WeaponManagerMultiplayer>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerWeapon")
            {
                if (weaponManager != null)
                {
                    if (firstTimeInteracting == false)
                    {
                        weaponManager.SwitchToWeapon(weaponSlot);
                        weaponManager.UnlockNewWeapon(unlockString);
                        weapon.SetActive(true);
                        Destroy(weaponContainer.transform.GetChild(weaponSlot).gameObject);
                        weapon.transform.SetParent(weaponContainer.transform);
                        weapon.transform.SetSiblingIndex(weaponSlot);
                        weaponManager.SelectWeapon();
                        firstTimeInteracting = true;
                    }
                    else
                    {
                        //ADD AMMO PICKUP SYSTEM HERE.
                    }

                    StartCoroutine(RespawnWeapon());

                }
                /*
                else if (weaponManagerMP != null)
                {
                    weaponManagerMP.SwitchToWeapon(weaponSlot);
                    weaponManagerMP.UnlockNewWeapon(unlockString);
                    weapon.SetActive(true);
                    Destroy(weaponContainer.transform.GetChild(weaponSlot).gameObject);
                    weapon.transform.SetParent(weaponContainer.transform);
                    weapon.transform.SetSiblingIndex(weaponSlot);
                    weaponOnTheGround.SetActive(false);
                }
                */
            }
        }

        IEnumerator RespawnWeapon()
        {
            Collider collider = GetComponent<Collider>();

            collider.enabled = false;
            weaponOnTheGround.SetActive(false);
            yield return new WaitForSeconds(respawnTime);
            collider.enabled = true;
            weaponOnTheGround.SetActive(true);
        }
    }
}