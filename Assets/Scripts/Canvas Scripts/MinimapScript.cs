using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{

    public Transform player;
    private GameObject crosshair;

    [SerializeField] GameObject ndhuMap;

    // Minimap script in canvas because
    // we cant activate a script form initially disabled GameObject

    public static bool isMapOpen = false;

    void Awake()
    {
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
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
