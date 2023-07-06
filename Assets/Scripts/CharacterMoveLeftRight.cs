using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveLeftRight : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float movespeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) == true || Input.GetKeyDown(KeyCode.RightArrow))
        {
            myRigidbody.velocity = Vector2.right * movespeed;
        }

        if (Input.GetKeyDown(KeyCode.A) == true || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            myRigidbody.velocity = Vector2.left * movespeed;
        }
    }
}
