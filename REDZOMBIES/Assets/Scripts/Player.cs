using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed; //value set in inspector window
    int playerMaxHealth = 3;
    public int playerHealth;
    public int playerPoints = 0;
    float shootTimer; //Differing values are set in Shoot()
    public bool playerDeath = false;
    public int weaponSelected; //Holds the value of the weapon currently assigned

    public GameObject weaponSprite; //References weapon object
    public GameObject bulletObject; //References bullet object
    Rigidbody2D rb;

    void Start() //Initialisation, called on first frame
    {
        playerHealth = playerMaxHealth;
        weaponSelected = 1; //Assign pistol as starting weapon
        rb = gameObject.GetComponent<Rigidbody2D>(); //Reference rigidbody component
    }

    void Update() //Called every frame
    {
        Movement();
        FaceCursor();
        Shoot();
    }

    void Movement() //Responsible for movement of this object
    {
        float x = Input.GetAxis("Horizontal"); //Gets input from horizontal axis
        float y = Input.GetAxis("Vertical"); //Gets input from vertical axis

        Vector2 inputVector = new Vector2(x, y); //Constructs new vector2 with given input
        inputVector.Normalize(); //Set magnitude to 1
        rb.velocity = inputVector * movementSpeed; //Objects velocity updated accordingly to specified direction

        //Debug.Log(inputVector); //Prints the values inside this vector2 in console
    }

    void FaceCursor() //Responsible for object facing users cursor
    {

    }

    void Shoot() //Responsible for firing a bullet
    {
        shootTimer -= Time.deltaTime; //Reduce the shooting cooldown every second
        if (Input.GetMouseButton(0) && shootTimer <= 0) //If left mouse button is held down and theres no shoot cooldown
        {
            float randomRotation = Random.Range(-8f, 8f);
            if (weaponSelected == 1) //1 is pistol
            {
                Instantiate(bulletObject, weaponSprite.transform.position + new Vector3(0, 0.1f, 0), transform.rotation * Quaternion.Euler(0, 0, randomRotation));
                //Create instance of bullet at 0.1 pixels infront of weapon sprites position with an manipulated rotation from the players rotation
                shootTimer = 0.4f; //Sets a cooldown to how fast the weapon can be fired
            }

            else if (weaponSelected == 2) //2 is assualt rifle
            {
                Instantiate(bulletObject, weaponSprite.transform.position + new Vector3(0, 0.1f, 0), transform.rotation * Quaternion.Euler(0, 0, randomRotation));
                shootTimer = 0.2f; 
            }

            else if (weaponSelected == 3) //3 is shotgun
            {
                Instantiate(bulletObject, weaponSprite.transform.position + new Vector3(-0.2f, 0.1f, 0), transform.rotation * Quaternion.Euler(0, 0, 8f));
                Instantiate(bulletObject, weaponSprite.transform.position + new Vector3(0, 0.1f, 0), transform.rotation);
                Instantiate(bulletObject, weaponSprite.transform.position + new Vector3(0.2f, 0.1f, 0), transform.rotation * Quaternion.Euler(0, 0, -8f));
                shootTimer = 0.5f; 
            }
            
            Debug.Log("left mouse button inputted\nFiring bullet!");
        }
    }

    public void OnCollisionStay2D(Collision2D body)//Called when a collisions been made
    {
        
    }
}