using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CometTileManager : MonoBehaviour
{
    public GameObject cometPrefab; // Reference to the comet prefab
    public int numberOfComets = 100; // Number of comets to be spawned
    public float tileWidth = 10f; // Width of the area where comets will be spawned
    public float tileHeight = 10f; // Height of the area where comets will be spawned
    public bool randomizeSize = true;

    public GameObject player;

    private int currentComets = 0;
    public float initialDistance = 0;
    public float distance = 50;
    public float despawnDistanceX = 50;
    public float despawnDistanceY = 50;


    void Start()
    {
        TilePlaneWithComets(initialDistance);
    }

    void TilePlaneWithComets(float offset)
    {
        for (int i = 0; i < numberOfComets - currentComets; i++)
        {
            float randomX = Random.Range(-tileWidth / 2f, tileWidth / 2f);
            float randomY = Random.Range(-tileHeight / 2f, tileHeight / 2f);
            Vector3 playerPosition = player.transform.position;


            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f) + playerPosition;
            if (offset != 0)
            {
                spawnPosition = playerPosition + (spawnPosition - playerPosition).normalized * offset;
            }

            float randomSize = 1;
            if (randomizeSize)
                randomSize = Random.Range(2f, 10f);

            float randomRotation = Random.Range(0f, 360f);

            cometPrefab.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

            Instantiate(cometPrefab, spawnPosition, Quaternion.Euler(0, 0, randomRotation), transform);
            currentComets++;
        }
    }

    private void Update()
    {
        if (player != null)
        TilePlaneWithComets(distance);
        RemoveFarComets();
    }

    void RemoveFarComets()
    {
        if (!player || !transform)
            return;
        foreach (Transform child in transform)
        {
            Bounds bounds = child.GetComponent<Collider2D>().bounds;

            // Calculate the rightmost and leftmost positions
            float rightmostX = bounds.center.x + bounds.extents.x;
            float leftmostX = bounds.center.x - bounds.extents.x;
            float topmostY = bounds.center.y + bounds.extents.y;
            float bottommostY = bounds.center.y - bounds.extents.y;

            float leftDistance = Mathf.Abs(leftmostX - player.transform.position.x);
            float rightDistance = Mathf.Abs(rightmostX - player.transform.position.x);
            float topDistance = Mathf.Abs(topmostY - player.transform.position.y);
            float bottommostDistance = Mathf.Abs(bottommostY - player.transform.position.y);

            // Check if both sides are within the range
            if (leftDistance > despawnDistanceX || rightDistance > despawnDistanceX || topDistance > despawnDistanceY || bottommostDistance > despawnDistanceY)
            {
                Destroy(child.gameObject);
                currentComets--;
            }
        }
    }
}