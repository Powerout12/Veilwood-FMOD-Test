using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    public LayerMask InteractionLayer;
    public float raycastDistance = 5f; 
    public bool isInteracting { get; private set; }
    public Camera playerCamera; 

    private void Start()
    {
        isInteracting = false;
    }

    //TODO: Put this in with player inputs that cam already made

    private void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);  // Raycast from the camera
        RaycastHit hit;

      
        if (Physics.Raycast(ray, out hit, raycastDistance, InteractionLayer))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            GameObject interactObject = hit.collider.GameObject(); //Added this for dialogue stuff sowwy!!!

            // If right-click is pressed and an interactable object is detected
            if (Input.GetMouseButtonDown(1)) 
            {
                if (interactable != null) 
                {
                    if (interactObject.tag ==  "NPC") // Also this teehee!!!
                    {
                        StartInteraction(interactable);
                        Debug.Log("NPC Interacted");
                    }  
                    else
                    {
                        StartInteraction(interactable);
                        PlayerMovement.accessingInventory = true;  // Assuming this controls the inventory UI
                        Debug.Log("Opened Inventory of Interactable Object");
                    }
                }
            }
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
}
