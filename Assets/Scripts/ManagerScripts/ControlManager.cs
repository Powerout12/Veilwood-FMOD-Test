using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour
{
    public static bool isController;
    public InputActionReference useHeldItem, interactWithItem, interactWithoutItem, 
    movement, sprint, look, moreInfo, hotbarScroll, hotbarUp, hotbarDown, showGrid;
    public string currentDevice;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentDevice = GetComponent<PlayerInput>().currentControlScheme;
        //print(currentDevice);
    }
}
