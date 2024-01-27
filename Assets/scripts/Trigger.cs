using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    private List<GameObject> availableObjects = new List<GameObject>();
    bool hasPackage = false;
    public float destroyDelay = 0.2f;
    [SerializeField] Color32 hasPackageColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 noPackageColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 noDeliveryColor = new Color32(1, 1, 1, 1);

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent < SpriteRenderer>();
        // Disable the object to respawn at the start
        foreach (var obj in objectsToSpawn)
        {
            obj.SetActive(false);
            availableObjects.Add(obj);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject != null)
        {
            if (collision.tag == "Package" && hasPackage == false)
            {
                if (availableObjects.Count > 0)
                {
                    // Choose a random index from the availableObjects list
                    int randomIndex = Random.Range(0, availableObjects.Count);
                    GameObject randomObject = availableObjects[randomIndex];

                    // Remove the chosen object from the availableObjects list
                    availableObjects.RemoveAt(randomIndex);

                    // Check if the collider's game object is still active before destroying
                    if (collision.gameObject.activeSelf)
                    {
                        // Destroy the collided object after a delay
                        Destroy(collision.gameObject, destroyDelay);
                    }

                    // Check if the randomObject is not null
                    if (randomObject != null)
                    {
                        // Activate the chosen object
                        randomObject.SetActive(true);
                        Debug.Log(randomObject);
                    }

                    spriteRenderer.color = hasPackageColor;

                    Debug.Log("Pack");
                    hasPackage = true;
                }
                else
                {
                    Debug.LogWarning("No available objects to spawn. Check the objectsToSpawn array.");
                }
            }
            else if (collision.tag == "Delivery" && hasPackage == true)
            {
                Debug.Log("Delivered");

                // Check if the collider's game object is still active before destroying
                if (collision.gameObject.activeSelf)
                {
                    // Destroy the collided object after a delay
                    Destroy(collision.gameObject, destroyDelay);
                }
                spriteRenderer.color = noPackageColor;

                hasPackage = false;
                if (availableObjects.Count == 0)
                {
                    spriteRenderer.color = noDeliveryColor;
                    Debug.Log("No more objects to deliver");
                    // Optionally, you can perform additional actions when there are no more objects to deliver
                }
            }
            
        }
    }
}
