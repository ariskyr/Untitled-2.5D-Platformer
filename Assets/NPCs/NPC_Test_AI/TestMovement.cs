using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    private Vector2 inputDirection;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        Animate();
    }

    private void ProcessInputs()
    {
        inputDirection = InputManager.Instance.GetMovePressed().normalized;
    }

    void Animate()
    {
        animator.SetFloat("DirectionX", inputDirection.x);
        animator.SetFloat("DirectionY", inputDirection.y);
    }
}
