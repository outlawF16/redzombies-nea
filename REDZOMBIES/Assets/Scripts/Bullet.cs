using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed; //Value set in inspector
    float deathTimer = 4f; //Value responsible for how long object will exist
    Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); //Reference objects rigidbody
    }

    void Update()
    {
        rb.velocity = transform.up * bulletSpeed; //Move object forward
        deathTimer -= Time.deltaTime; //Reduce time each second 
        if (deathTimer <= 0) 
        {
            Destroy(gameObject); //Delete this object from the scene
        }
    }

    public void OnTriggerEnter2D(Collider2D body)//Called once the moment a collisions been made
    {
        Collider2D hasCollider = body.gameObject.GetComponent<Collider2D>();
        //Attempt to fetch the collider component attatched to the object collided with
        if (hasCollider != null) //If the collided object does have a collider
        {
            Destroy(gameObject); 
        }
    }
}
