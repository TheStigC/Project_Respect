using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class Interactable : MonoBehaviour {

    public bool isEPressed = false;
    private Transform target = null;

    //Initiating animator
    Animator myAnimator;

    void Start()
    {
        //Tell the script that's an animator attached to it!
        myAnimator = gameObject.GetComponent<Animator>();
    }

    //Following two methods detects the player within the collider with a trigger
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (CrossPlatformInputManager.GetButtonDown("Use1") && isEPressed == false)
            { 
                Debug.Log("HALLO DET VIRKER");
                isEPressed = true;
                if(myAnimator != null)
                {
                    myAnimator.SetTrigger("Interact");
                    Invoke("ResetAnimation", 5f);
                }
            }
        }      
    }

    void ResetAnimation()
    {
        myAnimator.SetTrigger("Stop");
        isEPressed = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") target = null;
    }
}
