using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static bool isController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print("Is Controller: " + isController);
        //print(Input.GetAxis("RightTrigger"));

        if(Input.GetAxis("LeftJoyY") != 0 
        || Input.GetAxis("LeftJoyX") != 0
        || Input.GetAxis("RightJoyX") != 0
        || Input.GetAxis("RightJoyY") != 0
        || Input.GetButton("ControllerTab")
        || Input.GetButtonDown("LeftBumper")
        || Input.GetButtonDown("RightBumper")
        || Input.GetAxis("RightTrigger") != 0
        || Input.GetAxis("LeftTrigger") != 0
        || Input.GetButtonDown("ControllerA")
        || Input.GetButtonDown("ControllerB")
        || Input.GetButtonDown("ControllerX")
        || Input.GetButtonDown("ControllerY"))
        {
            isController = true;
            //print("vert!!!");
        }

        else if(Input.GetAxis("Horizontal") != 0 
        || Input.GetAxis("Vertical") != 0
        || Input.GetAxisRaw("Mouse X") != 0
        || Input.GetAxisRaw("Mouse Y") != 0
        || Input.GetButton("Tab")
        || Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            isController = false;
        }
    }
}
