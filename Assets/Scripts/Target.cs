using UnityEngine;

public class Target : MonoBehaviour
{
    public float radius = 3f;
    bool isFocused = false;
    Transform enemy;
    public Transform playerTransform;
    bool hasTouched = false;

    private void Update()
    {
        if (isFocused && !hasTouched)
        {
            float distance = Vector3.Distance(enemy.position, playerTransform.position);
            if (distance <= radius)
            {
                // Her jeg finder ud af hvad de skal gøre når player fanges
                Interact();
                hasTouched = true;
            }
        }
    }
    public void OnFococused(Transform playerTransform)
    {
        isFocused = true;
        enemy = playerTransform;
        hasTouched = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, radius);
    }


    public virtual void Interact()
    {
        //this method is meat to be overridden

        Debug.Log("Wasted");
    }


}
