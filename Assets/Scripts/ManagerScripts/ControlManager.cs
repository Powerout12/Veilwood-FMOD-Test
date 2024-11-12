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
        if(Input.GetAxis("LeftJoyY") != 0 || Input.GetAxis("LeftJoyX") != 0)
        {
            isController = true;
            //print("vert!!!");
        }

        else if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isController = false;
        }
    }
}
