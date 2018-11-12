using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class WeaponManagerMultiplayer : MonoBehaviourPunCallbacks, IPunObservable
    {

        public GameObject projectilePrefabHand, projectilePrefabBaseballBat, projectilePrefabPistol, projectilePrefabShotgun, projectilePrefabBazooka;
        public Transform firepointHand, firepointBaseballBat, firepointPistol, firepointShotgun;
        public GameObject handWeapon, baseballBat, pistol, shotgun, bazooka;
        public int previousWeapon, selectedWeapon = 0;
        public float projectileLifetime = 0.5f;
        private CameraShake camShake;
        public GameObject iconContainer;
        public Image activeWeaponIcon;
        public bool handsUnlocked = true, baseballBatUnlocked = false, pistolUnlocked = false, shotgunUnlocked = false, bazookaUnlocked = false;
        public List<Sprite> iconsList;
        bool isFiring;
        public static WeaponManagerMultiplayer Instance;

        
        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isFiring);
                stream.SendNext(projectileLifetime);
            }
            else
            {
                // Network player, receive data
                this.isFiring = (bool)stream.ReceiveNext();
                this.projectileLifetime = (float)stream.ReceiveNext();
            }
        }

        #endregion
        


        void Start()
        {
            Instance = this;
            camShake = Camera.main.GetComponent<CameraShake>();
            InvokeRepeating("FindUI", 0.25f, 0.25f);
        }

        void FindUI()
        {
            if (iconContainer == null)
            {
                iconContainer = GameObject.Find("ActiveWeaponImage");
                activeWeaponIcon = iconContainer.GetComponent<Image>();

                if (iconContainer != null)
                {
                    SelectWeapon();
                }
            }
            else
                CancelInvoke();
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
        }


        public void ShootWeapon()
        {
            isFiring = true;

            if (handWeapon.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = PhotonNetwork.InstantiateSceneObject(this.projectilePrefabHand.name, this.firepointHand.transform.position, this.firepointHand.transform.rotation) as GameObject;
                StartCoroutine(DestroyGameObject(projectileClone, projectileLifetime));

                //camShake.Shake(0.025f, 0.1f, Camera.main);
                isFiring = false;
            }
            else if (baseballBat.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = PhotonNetwork.InstantiateSceneObject(this.projectilePrefabBaseballBat.name, this.firepointBaseballBat.transform.position, this.firepointBaseballBat.transform.rotation) as GameObject;
                StartCoroutine(DestroyGameObject(projectileClone, projectileLifetime));

                //camShake.Shake(0.04f, 0.1f, Camera.main);
                isFiring = false;
            }
            else if (pistol.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = PhotonNetwork.InstantiateSceneObject(this.projectilePrefabPistol.name, this.firepointPistol.transform.position, this.firepointPistol.transform.rotation) as GameObject;
                StartCoroutine(DestroyGameObject(projectileClone, projectileLifetime));

                //camShake.Shake(0.04f, 0.1f, Camera.main);
                isFiring = false;
            }
            else if (shotgun.activeSelf == true)
            {
                GameObject projectileClone;
                projectileClone = PhotonNetwork.InstantiateSceneObject(this.projectilePrefabShotgun.name, this.firepointShotgun.transform.position, this.firepointShotgun.transform.rotation) as GameObject;
                StartCoroutine(DestroyGameObject(projectileClone, projectileLifetime));

                //camShake.Shake(0.08f, 0.1f, Camera.main);
                isFiring = false;
            }
        }



        public void SelectWeapon()
        {
            int i = 0;
            foreach (Transform weapon in transform)
            {

                if (i == selectedWeapon && !weapon.name.Contains("Locked"))
                {
                    weapon.gameObject.SetActive(true);
                    activeWeaponIcon.sprite = iconsList[i];
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



        IEnumerator DestroyGameObject(GameObject objectToDestroy, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            PhotonNetwork.Destroy(objectToDestroy);
        }
    }
}