using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static bool isController;
    public static bool isDpadLeftPressed;
    public static bool isDpadRightPressed, isDpadRightReleased;
    public static bool isDpadUpPressed;
    public static bool isDpadDownPressed;

    bool upIsPressed = false;
    bool downIsPressed = false;
    bool isNeutral = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print("Is Controller: " + isController);
        //print(Input.GetAxis("RightTrigger"));
        ControllerCheck();

        //if(Input.GetAxis("DPadH") == 1 && !isDpadRightReleased) //Fix this later
        //{
         //   isDpadRightPressed = true;
        //    yield return new WaitForEndOfFrame();
        //    isDpadRightPressed = false;
        //}
    }

    
    
    public void ControllerCheck()
    {
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
        || Input.GetButtonDown("ControllerY")
        || Input.GetButtonDown("ControllerSprint")
        //|| Input.GetAxis("DPadH") != 0
        //|| Input.GetAxis("DPadV") != 0
        )
        {
            isController = true;
            //print("vert!!!");
            return;
        }

        else if(Input.GetAxisRaw("Horizontal") != 0 
        || Input.GetAxisRaw("Vertical") != 0
        || Input.GetAxisRaw("Mouse X") != 0
        || Input.GetAxisRaw("Mouse Y") != 0
        || Input.GetButton("Tab")
        || Input.GetAxis("Mouse ScrollWheel") != 0
        || Input.GetMouseButtonDown(0)
        || Input.GetMouseButtonDown(1)
        || Input.GetMouseButtonDown(2)
        || Input.GetButtonDown("Sprint"))
        {
            isController = false;
        }
        else
        {
            return;
        }
    }
}
