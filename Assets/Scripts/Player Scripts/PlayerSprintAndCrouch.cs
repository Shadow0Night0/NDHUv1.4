using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public Animator anim;

    public float sprint_Speed = 100f;
    public float crouch_Speed = 2f;
    public float move_Speed = 5f;

    private Transform look_Root;
    private float stand_Height = 1.6f;
    private float crouch_Height = 1f;

    private bool is_Crouching = false, is_Sprinting = false;

    private PlayerFootsteps player_Footsteps;

    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.6f;

    private float walk_Step_Distance = 0.4f;
    private float sprint_Step_Distance = 0.25f;
    private float crouch_Step_Distance = 0.5f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        look_Root = transform.GetChild(0);

        player_Footsteps = GetComponentInChildren<PlayerFootsteps>();
    }
        // Start is called before the first frame update
    void Start()
    {
        player_Footsteps.volume_Min = walk_Volume_Min;
        player_Footsteps.volume_Max = walk_Volume_Max;
        player_Footsteps.step_Distance = walk_Step_Distance;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
        {
            playerMovement.speed = sprint_Speed;

            is_Sprinting = true;
            anim.SetBool("sprint", true);

            player_Footsteps.step_Distance = sprint_Step_Distance;
            player_Footsteps.volume_Min = sprint_Volume;
            player_Footsteps.volume_Max = sprint_Volume;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !is_Crouching)
        {
            is_Sprinting = false;
            anim.SetBool("sprint", false);

            playerMovement.speed = move_Speed;

            player_Footsteps.volume_Min = walk_Volume_Min;
            player_Footsteps.volume_Max = walk_Volume_Max;
            player_Footsteps.step_Distance = walk_Step_Distance;
        }
    }
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //if we are crouching stand up
            if (is_Crouching)
            {
                anim.SetBool("crouch", false);

                look_Root.localPosition = new Vector3(0f, stand_Height, 0f); //has to be local position, otherwise, you clip into the ground
                playerMovement.speed = move_Speed;//local position is just putting this object relative to parent object

                player_Footsteps.volume_Min = walk_Volume_Min;
                player_Footsteps.volume_Max = walk_Volume_Max;
                player_Footsteps.step_Distance = walk_Step_Distance;

                is_Crouching = false;

                
            }
            //if we are not crouching crouch
            else if(!is_Crouching && !is_Sprinting)
            {
                anim.SetBool("crouch", true);


                look_Root.localPosition = new Vector3(0f, crouch_Height, 0f);
                playerMovement.speed = crouch_Speed;

                player_Footsteps.step_Distance = crouch_Step_Distance;
                player_Footsteps.volume_Min = crouch_Volume;
                player_Footsteps.volume_Max = crouch_Volume;

                is_Crouching = true;
            }
        }
    }
}