using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] float steerSpeed = 1f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float boostSpeed = 1.5f;
    [SerializeField] float slowSpeed = 0.5f;

    float currentSpeed;
    float collisionTime;
    public float health = 100f;
    float damageFromBump;
    public int damageFromHouse = 35;
    public int damageFromStone = 50;
    public int damageFromTree = 25;
    public float armor = 50;
    bool isAlive = false;

    private void Start()
    {
        currentSpeed = moveSpeed;
        isAlive = true;
    }

    void Update()
    {
            isAlive = health >= 0;

            if ( !isAlive )
            {
                Destroy(gameObject);
            }
       
            float steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
            float moveAmount = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

            if (Input.GetKey("left shift") && Time.time - collisionTime > 2f)
            {
                currentSpeed = boostSpeed;
            }
            else if (currentSpeed != slowSpeed)
            {
                currentSpeed = moveSpeed;
            }

            transform.Rotate(0, 0, -steerAmount);
            transform.Translate(0, moveAmount, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "House":
                damageFromBump = damageFromHouse; break;
            case "Tree":
                damageFromBump = damageFromTree; break;
            case "Stone":
                damageFromBump = damageFromStone; break;
        }
        collisionTime = Time.time;
        currentSpeed = slowSpeed;
        Invoke("ResetSpeed", 2f);
        health -= damageFromBump * armor/100;
    }

    private void ResetSpeed()
    {
        currentSpeed = moveSpeed;
    }
}
