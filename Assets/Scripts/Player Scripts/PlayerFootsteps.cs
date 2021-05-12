using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource footstep_Sound;

    [SerializeField]
    private AudioClip[] footsteps_Clip;

    [SerializeField]
    private CharacterController character_Controller;

    [HideInInspector]
    public float volume_Min, volume_Max;

    public float accumulated_Distance;

    [HideInInspector]
    public float step_Distance;


    // Start is called before the first frame update
    void Awake ()
    {
        footstep_Sound = GetComponent<AudioSource>();

        character_Controller = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckToPlayFootstepSound();
    }

    private void CheckToPlayFootstepSound()
    {
        if (!character_Controller.isGrounded)
            return;

        if(character_Controller.velocity.sqrMagnitude > 0)
        {
            // accumulated distance is value indicating how far we go 
            // e.g. make a step, sprint, or move while crouching
            // until the footstep sound sounds start playing
            accumulated_Distance += Time.deltaTime;

            if(accumulated_Distance > step_Distance)
            {
                footstep_Sound.volume = Random.Range(volume_Min, volume_Max);
                footstep_Sound.clip = footsteps_Clip[Random.Range(0, footsteps_Clip.Length)];
                footstep_Sound.Play();

                accumulated_Distance = 0f;
            }
        }
        else
        {
            accumulated_Distance = 0f;
        }


    }















}

























