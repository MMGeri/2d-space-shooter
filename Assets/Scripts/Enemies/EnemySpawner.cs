using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which spawns enemies in an area around it.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("GameObject References")]
    [Tooltip("The enemy prefab to use when spawning enemies")]
    public GameObject enemyPrefab = null;
    [Tooltip("The target of the spwaned enemies")]
    public Transform target = null;

    [Header("Spawn Position")]
    [Tooltip("The distance within which enemies can spawn in the X direction")]
    [Min(0)]
    public float spawnRangeX = 50f;
    [Tooltip("The distance within which enemies can spawn in the Y direction")]
    [Min(0)]
    public float spawnRangeY = 50f;
    public float despawnRange = 50f;

    [Header("Spawn Variables")]
    [Tooltip("The maximum number of enemies that can be spawned from this spawner")]
    public int maxSpawn = 20;
    [Tooltip("Ignores the max spawn limit if true")]
    public bool spawnInfinite = true;

    // The number of enemies that have been spawned
    private int currentlySpawned = 0;

    [Tooltip("The time delay between spawning enemies")]
    public float spawnDelay = 2.5f;

    // The most recent spawn time
    private float lastSpawnTime = Mathf.NegativeInfinity;

    [Tooltip("The object to make projectiles child objects of.")]
    public Transform projectileHolder = null;

    [HideInInspector]
    public int bonusHealth = 0;
    [HideInInspector]
    public int bonusDamage = 0;

    /// <summary>
    /// Description:
    /// Standard Unity function called every frame
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void Update()
    {
        CheckSpawnTimer();
        DespawnFarEnemies();
    }

    /// <summary>
    /// Description:
    /// Checks if it is time to spawn an enemy
    /// Spawns an enemy if it is time
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void CheckSpawnTimer()
    {   
        // If it is time for an enemy to be spawned
        if (Time.timeSinceLevelLoad > lastSpawnTime + spawnDelay && (currentlySpawned < maxSpawn || spawnInfinite))
        {
            // Determine spawn location
            Vector3 spawnLocation = GetSpawnLocation();

            // Spawn an enemy
            SpawnEnemy(spawnLocation);
        }
    }

    /// <summary>
    /// Description:
    /// Spawn and set up an instance of the enemy prefab
    /// Inputs: 
    /// Vector3 spawnLocation
    /// Returns: 
    /// void (no return)
    /// </summary>
    /// <param name="spawnLocation">The location to spawn an enmy at</param>
    private void SpawnEnemy(Vector3 spawnLocation)
    {
        // Make sure the prefab is valid
        if (enemyPrefab != null)
        {
            // Create the enemy gameobject
            GameObject enemyGameObject = Instantiate(enemyPrefab, spawnLocation, enemyPrefab.transform.rotation, transform);
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            ShootingController[] shootingControllers = enemyGameObject.GetComponentsInChildren<ShootingController>();

            // Setup the enemy if necessary
            if (enemy != null)
            {
                enemy.followTarget = target;
                enemy.GetComponent<Health>().currentHealth = enemy.GetComponent<Health>().currentHealth + bonusHealth;
                enemy.GetComponent<Health>().maximumHealth = enemy.GetComponent<Health>().maximumHealth + bonusHealth;
            }
            foreach (ShootingController gun in shootingControllers)
            {
                gun.projectileHolder = projectileHolder;
                gun.GetComponent<ShootingController>().projectilePrefab.GetComponent<Damage>().damageAmount = gun.GetComponent<ShootingController>().projectilePrefab.GetComponent<Damage>().damageAmount + bonusDamage;
            }

            // Incremment the spawn count
            currentlySpawned++;
            lastSpawnTime = Time.timeSinceLevelLoad;
        }
    }

    private void DespawnFarEnemies()
    {
        if (target == null)
            return;
        foreach (Transform child in transform)
        {
            if (Vector3.Distance(child.position, target.transform.position) > despawnRange)
            {
                Destroy(child.gameObject);
                currentlySpawned--;
            }
        }
    }

    /// <summary>
    /// Description:
    /// Returns a generated spawn location for an enemy
    /// Inputs: 
    /// none
    /// Returns: 
    /// Vector3
    /// </summary>
    /// <returns>Vector3: The spawn location as determined by the function</returns>
    protected virtual Vector3 GetSpawnLocation()
    {
        if (target == null)
            return new Vector3();
        // Get random coordinates
        float x = Random.Range(-spawnRangeX / 2f, spawnRangeY / 2f);
        float y = Random.Range(-spawnRangeX / 2f, spawnRangeY / 2f);
        // Return the coordinates as a vector
        Vector3 playerPosition = target.transform.position;
        Vector3 spawnPosition = new Vector3(x, y, 0f) + playerPosition;
        spawnPosition = playerPosition + (spawnPosition - playerPosition).normalized * spawnRangeX;
        return spawnPosition;
    }
}
