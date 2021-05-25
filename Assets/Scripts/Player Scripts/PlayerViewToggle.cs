using UnityEngine;

public class PlayerViewToggle : MonoBehaviour
{

    public Camera FPSCam;
    public Camera TPSCam;
    private GameObject crosshair;

    private bool isInFirstPerson = true;

    void Start()
    {
        FPSCam.enabled = FPSCam;
        TPSCam.enabled = !FPSCam;
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        
    }

    // Update is called once per frame
    void Update()
    {
        //Jesus code --- basically switching whatever state we're in by abusing the ! operator
        if(Input.GetKeyDown(KeyCode.V))
        {
            isInFirstPerson = !isInFirstPerson;
            FPSCam.enabled = isInFirstPerson;
            TPSCam.enabled = !isInFirstPerson;
            crosshair.SetActive(isInFirstPerson);
        }
    }
}
