using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{

    public Transform player;
    private GameObject crosshair;

    [SerializeField] private GameObject ndhuMap;
    [SerializeField] private GameObject playerIndicator;

    Vector3 lastPosition;

    //varaible used in vector3.lerp function to interpolate between two variables
    private float INTERPOLANT = 0.6f;

    // Minimap script in canvas because
    // we cant activate a script form initially disabled GameObject

    public static bool isMapOpen = false;

    void Awake()
    {
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
    }

    private void Start()
    {
        lastPosition = player.position;
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
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        // camera rotate with player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        // Player Indicator Movement in the Big Map 

        playerIndicator.transform.position = Vector3.Lerp(lastPosition, newPosition, INTERPOLANT);
        lastPosition = newPosition;
        


    }
    
    // Open map

    void OpenMap()
    {
        ndhuMap.SetActive(true);

        isMapOpen = true;
    }
    void CloseMap()
    {
        ndhuMap.SetActive(false);

        isMapOpen = false;

    }
}
