using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCam;

    public GameObject testStructure;

    public StructureManager structManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;

            if(Physics.Raycast(mainCam.transform.position, fwd, out hit, 10, 1 << 7))
            {
                Vector3 pos = structManager.CheckTile(hit.point);
                if(pos != new Vector3(0,0,0)) structManager.SpawnStructure(testStructure, pos);
                else print("Tile is not free");
            }
        }

    }

    
}
