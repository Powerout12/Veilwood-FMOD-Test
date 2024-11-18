using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour
{
    public static bool isController;
    public InputActionReference useHeldItem, interactWithItem, interactWithoutItem, 
    movement, sprint, look, moreInfo, hotbarScroll, hotbarUp, hotbarDown, showGrid;
    string currentDevice;
    public static bool isGamepad;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentDevice = GetComponent<PlayerInput>().currentControlScheme;
        print(currentDevice);
        if(currentDevice == "Gamepad")
        {
            isGamepad = true;
        }
        else
        {
            isGamepad = false;
        }
    }
}
