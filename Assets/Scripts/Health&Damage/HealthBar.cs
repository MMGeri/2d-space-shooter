using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private float scale;
    public GameObject player;
    public float ydisplaceAmount = 10;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            return;
        float shrinkAmount = (float)player.GetComponent<Health>().currentHealth/ (float)player.GetComponent<Health>().maximumHealth;
        transform.localScale = new Vector3(scale*shrinkAmount, transform.localScale.y,1);
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y - ydisplaceAmount, player.transform.position.z);
        
    }
}
