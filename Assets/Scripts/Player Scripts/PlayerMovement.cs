using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character_Controller;
    public Animator anim;
    private Vector3 move_Direction;

    [SerializeField]
    public float speed = 5f;

    [SerializeField]
    private float gravity = 20f;

    [SerializeField]
    public float jump_Force = 10f;
    
    private float vertical_Velocity;

    void Awake()
    {
        character_Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        float verticalAxis = Input.GetAxis(Axis.VERTICAL);
        float horizontalAxis = Input.GetAxis(Axis.HORIZONTAL);

        anim.SetFloat("vertical", verticalAxis);
        anim.SetFloat("horizontal", horizontalAxis);

        move_Direction = new Vector3(horizontalAxis, 0f, 
                                     verticalAxis);

        move_Direction = transform.TransformDirection(move_Direction);
        move_Direction *= speed * Time.deltaTime;

        ApplyGravity();

        character_Controller.Move(move_Direction);
    }

    void ApplyGravity()
    {
        vertical_Velocity -= gravity * Time.deltaTime;
        PlayerJump();
        move_Direction.y = vertical_Velocity * Time.deltaTime;
    }
    void PlayerJump()
    {
        if(character_Controller.isGrounded && Input.GetKeyDown(KeyCode.Space))  //also gtkeyup
        {
            vertical_Velocity = jump_Force;
            anim.SetBool("jump", true);
        }
        else { anim.SetBool("jump", false); }
    }
}
