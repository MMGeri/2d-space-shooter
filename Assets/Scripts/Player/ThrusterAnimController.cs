using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterAnimController : MonoBehaviour
{
    public Animator anim;
    public GameObject player;
    private string[] inputArray = { "w", "a", "s", "d" };
    private ISet<string> keysHeld = new HashSet<string>();
    private bool isOnTheMove = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        StartCoroutine(CheckMoving(player));
        if (this.isOnTheMove)
        {
            anim.Play("ThrustersAnimator");
        }
        else
        {
            anim.Play("Default");
        }
    }

    private IEnumerator CheckMoving(GameObject @object)
    {
        Vector3 startPos = @object.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 finalPos = @object.transform.position;

        if (startPos.x != finalPos.x || startPos.y != finalPos.y
            || startPos.z != finalPos.z)
            this.isOnTheMove = true;
        else {
            this.isOnTheMove = false;
         }
    }

    void Update()
    {

    }
}
