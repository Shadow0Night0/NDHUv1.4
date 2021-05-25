using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    private bool isMapOpen = false;
    public Transform player;
    public RectTransform playerIndicator;
    private GameObject crosshair;

    [SerializeField] private GameObject ndhuMap;

    Vector3 lastPosition;



    // Minimap script in canvas because
    // we cant activate a script form initially disabled GameObject


    Vector2 newIndicatorPos;
    Vector2 playerIndicatorOffset = new Vector2(-41.1f, -122.35f);

    float InGameToMap_Scalar_x = 3.5f, InGameToMap_Scalar_y = 3.5f;

    void Awake()
    {
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
        lastPosition = player.position;
    }

    private void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isMapOpen)
            {

                CloseMap();
                crosshair.SetActive(true);
            }
            else
            {

                OpenMap();
                crosshair.SetActive(false);
            }
        }

    }

    private void LateUpdate()
    {

        // moving minimap camera with player
        Vector3 newPosition = player.position;



        // Player Indicator Movement in the Big Map 
        newIndicatorPos.x += (lastPosition.x - newPosition.x) / InGameToMap_Scalar_x;
        newIndicatorPos.y += (lastPosition.z - newPosition.z) / InGameToMap_Scalar_y;



        //Debug.Log((newIndicatorPos));
        playerIndicator.anchoredPosition = (newIndicatorPos + playerIndicatorOffset);
        // Player Indicator Movement in the Big Map 


        lastPosition = newPosition;
        newPosition.y = transform.position.y;
        transform.position = newPosition;


        // camera rotate with player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);




    }

    // Open map

    public void OpenMap()
    {

        ndhuMap.SetActive(true);
        isMapOpen = true;



    }
    public void CloseMap()
    {
        ndhuMap.SetActive(false);
        isMapOpen = false;


    }
}
