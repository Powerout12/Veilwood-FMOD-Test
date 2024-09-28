using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCam;

    public GameObject testStructure;
    public InventoryItemData testItem;

    StructureManager structManager;

    public bool isInteracting { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if(!mainCam) mainCam = FindObjectOfType<Camera>();
        structManager = FindObjectOfType<StructureManager>();
    }



    // Update is called once per frame
    void Update()
    {
        //LEFT CLICK USES THE ITEM CURRENTLY IN THE HAND
        if(Input.GetMouseButtonDown(0) && !PlayerMovement.accessingInventory)
        {
            //TestSpawnStruct();
            UseHotBarItem();
        }

        //RIGHT CLICK USES AN ITEM ON A STRUCTURE, EX: PLANTING A SEED IN FARMLAND
        if(Input.GetMouseButtonDown(1) && !PlayerMovement.accessingInventory)
        {
            StructureInteractionWithItem();
        }

        //SPACE INTERACTS WITH A STRUCTURE WITHOUT USING AN ITEM, EX: HARVESTING A CROP
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InteractWithObject();
        }

        if(Input.GetKeyDown("f"))
        {
            //TO TEST CLEARING A STRUCTURE
            DestroyStruct();
        }

    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        isInteracting = true;
    }

    void EndInteraction()
    {
        isInteracting = false;
    }

    void DestroyStruct()
    {
        Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if(Physics.Raycast(mainCam.transform.position, fwd, out hit, 10, 1 << 6))
        {
            Destroy(hit.collider.gameObject);
        }
    }

    void StructureInteractionWithItem()
    {
        Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;


        if (Physics.Raycast(mainCam.transform.position, fwd, out hit, 10, 1 << 6))
        {
            var structure = hit.collider.GetComponent<StructureBehaviorScript>();
            if (structure != null)
            {
                structure.ItemInteraction(HotbarDisplay.currentSlot.AssignedInventorySlot.ItemData);
                Debug.Log("Interacted with item");
                return;
            }
        }
    }

    void InteractWithObject()
    {
        Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;


        if (Physics.Raycast(mainCam.transform.position, fwd, out hit, 10, 1 << 6))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            GameObject hitObject = hit.collider.gameObject;
            if (interactable != null)
            {
                StartInteraction(interactable);
                
                if (hitObject.tag ==  "NPC") // Also this teehee!!! //Lauren?????
                {
                    Debug.Log("NPC Interacted");
                    return;
                }  


                PlayerMovement.accessingInventory = true;  // Assuming this controls the inventory UI
                Debug.Log("Opened Inventory of Interactable Object");
                return;
            }

            var structure = hit.collider.GetComponent<StructureBehaviorScript>();
            if (structure != null)
            {
                structure.StructureInteraction();
                Debug.Log("Interacting with a structure");
            }
        }
    }

    void TestSpawnStruct()
    {
        Vector3 fwd = mainCam.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if(Physics.Raycast(mainCam.transform.position, fwd, out hit, 10, 1 << 7))
        {
            Vector3 pos = structManager.CheckTile(hit.point);
            if(pos != new Vector3(0,0,0)) structManager.SpawnStructure(testStructure, pos);
        }
    }

    void UseHotBarItem()
    {
        InventoryItemData item = HotbarDisplay.currentSlot.AssignedInventorySlot.ItemData;

        //Is it a placeable item?
        PlaceableItem p_item = item as PlaceableItem;
        if(p_item) p_item.PlaceStructure(mainCam.transform);
    }
    
}
