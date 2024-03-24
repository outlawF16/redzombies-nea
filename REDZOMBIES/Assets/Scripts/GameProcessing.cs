using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProcessing : MonoBehaviour
{
    public GameObject[] spawnLocations; //Holds all possible spawn locations for enemy
    public Player player; //Get reference to player class inside game scene
    public GameObject enemyObject; //Get enemy object prefab
    public GameObject fastEnemyObject;
    public GameObject exploderEnemyObject;
    public Pistol pistolObject;
    public Shotgun shotgunObject;
    public AssaultRifle assualtRifleObject;
    public GameObject waveDisplay; //Reference WaveText object
    Text waveText;
    public GameObject scoreDisplay; //Reference ScoreText object
    Text scoreText;
    public GameObject healthDisplay; //Reference HealthText object
    Text healthText;
    public GameObject bulletDisplay; //Reference bulletText object
    Text bulletText;
    public GameObject weaponDisplay; //Reference weaponText object
    Text weaponText;
    Enemy enemyClass;

    float enemyHealthScale = 0;
    float enemySpeedScale = 0;
    public int waveNumber = 0; //Current wave number
    int enemiesToSpawn = 0; //Num of enemies to spawn next round
    public int enemiesToKill = 0; // Num of enemies to be eliminated to start next round
    void Start()
    {
        waveNumber = 0; //Initialise wave to 0
        enemiesToSpawn = Random.Range(4, 9); //Spawn atleast 4 enemies
        enemyClass = enemyObject.GetComponent<Enemy>();
        NextWave(); //Start first wave

        scoreText = scoreDisplay.GetComponent<Text>(); //Grab text component
        weaponText = weaponDisplay.GetComponent<Text>();
        bulletText = bulletDisplay.GetComponent<Text>();
        waveText = waveDisplay.GetComponent<Text>();
        healthText = healthDisplay.GetComponent<Text>();
    }
    
    void Update()
    {
        if (player.weaponSelected == 1)
        {
            weaponText.text = "Weapon: Pistol";
        }
        else if (player.weaponSelected == 2)
        {
            weaponText.text = "Weapon: AssualtRifle";
        }
        else if (player.weaponSelected == 3)
        {
            weaponText.text = "Weapon: Shotgun";
        }
        bulletText.text = "Bullets: " + player.bulletCapacity.ToString(); //Output players bullet capacity
        healthText.text = "Health: " + player.playerHealth.ToString(); //Output players health
        waveText.text = "Wave: " + waveNumber.ToString();//Output wave number
        scoreText.text = "Points: " + player.playerPoints.ToString();//Output players in-game points
        if (player.playerHealth <= 0)
        {
            Debug.Log("Game ended!");
            SceneManager.LoadScene("GameOver"); //Load game over scene, ending the game
        }
        else if (enemiesToKill <= 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        waveNumber += 1; //Increment wave number

        if (waveNumber > 1)
        {
            ScaleDifficulty();
        }
        enemiesToKill = enemiesToSpawn;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomSpawn = Random.Range(0, 7); //Choose a random enemy spawn location
            int randomChance = Random.Range(1, 5); //Choose a number from 1 to 4
            if (randomChance == 4) // 25% chance to spawn a fast enemy
            {
                randomChance = Random.Range(1, 11);
                if (randomChance == 10) // 10% chance to spawn a exploder enemy
                {
                    Instantiate(exploderEnemyObject, spawnLocations[randomSpawn].transform.position, transform.rotation); //Spawn exploder enemy
                }
                else
                {
                    Instantiate(fastEnemyObject, spawnLocations[randomSpawn].transform.position, transform.rotation); //Spawn fast enemy
                }
            }

            else
            {
                GameObject newEnemy = Instantiate(enemyObject, spawnLocations[randomSpawn].transform.position, transform.rotation); //Spawn enemy
                enemyClass = newEnemy.GetComponent<Enemy>();
                enemyClass.enemyHealth += enemyHealthScale;
                enemyClass.movementSpeed += enemySpeedScale;
            }
        }
        Debug.Log("Next round started!\n" + enemiesToSpawn.ToString() + " enemies have spawned this round");
    }

    void ScaleDifficulty()
    {
        enemiesToSpawn += Random.Range(4, 9); //Spawn more enemies next round by a seemingly random amount
        int randomChance = Random.Range(1, 6 - player.playerHealth); //Generate number between 1 and (5 - current health)
        if (randomChance == 1 && player.playerHealth > 1) //Scale enemy stats
        {
            randomChance = Random.Range(1, 5);
            if (randomChance == 1) //25% to increase health 
            {
                enemyHealthScale += Random.Range(0.1f, player.playerHealth / 4f); //Add a random value 
                Debug.Log("Enemy health increased!!");
            }
            else if (randomChance == 2) //25% to increase speed
            {
                enemySpeedScale += Random.Range(0.1f, player.playerHealth / 8f);
                Debug.Log("Enemy movement increased!!");
            }
            else
            {
                pistolObject.cost += player.playerHealth * 30; //Increase purchase cost by (player health * 40)
                shotgunObject.cost += player.playerHealth * 35;
                assualtRifleObject.cost += player.playerHealth * 40;
            }
        }
        else
        {
            Debug.Log("enemy stats were not scaled!");
            if (player.playerHealth < 2) 
            {
                enemiesToSpawn -= Random.Range(3, 7); //Reduce number of enemies to spawn next wave 
            }
        }
        player.playerHealth = player.playerMaxHealth; //Restore player health
    }
}
