using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField]
    private Renderer backgroundRenderer;
    public GameObject player;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 movementVector = new Vector3(InputManager.instance.horizontalMoveAxis, InputManager.instance.verticalMoveAxis, 0);
            backgroundRenderer.material.mainTextureOffset += new Vector2(movementVector.x * Time.deltaTime * moveSpeed, movementVector.y * Time.deltaTime * moveSpeed);
            backgroundRenderer.transform.position = player.transform.position;
        }
        else
        {
            backgroundRenderer.material.mainTextureOffset += new Vector2(1 * moveSpeed* Time.deltaTime, 1);
        }
    }
}
