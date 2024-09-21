using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay chestPanel;
    public DynamicInventoryDisplay playerBackpackPanel;

    private bool inventoryDisplayed;

    private void Awake()
    {
        inventoryDisplayed = false;
        chestPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
    }

    void Update()
    {
        /* if (PlayerMovement.accesingInventory && !inventoryDisplayed)
         {

         }*/
        /* if (inventoryPanel.gameObject.activeInHierarchy && !PlayerMovement.accesingInventory)
         {
             inventoryPanel.gameObject.SetActive(false);
             inventoryDisplayed = false;
         }*/
        if (chestPanel.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.T))
        {
            chestPanel.gameObject.SetActive(false);
            playerBackpackPanel.gameObject.SetActive(false);
            inventoryDisplayed = false;
            PlayerMovement.accesingInventory = false;
        }
        if (playerBackpackPanel.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.T) && PlayerMovement.accesingInventory)
        {
            Debug.Log("Running This");
            playerBackpackPanel.gameObject.SetActive(false);
            inventoryDisplayed = false;
            PlayerMovement.accesingInventory = false;
        }
    }

    void DisplayInventory(InventorySystem invToDisplay)
    {
       PlayerMovement.accesingInventory = true;
        chestPanel.gameObject.SetActive(true);
        playerBackpackPanel.gameObject.SetActive(true);
        chestPanel.RefreshDynamicInventory(invToDisplay);
    }

    void DisplayPlayerBackpack(InventorySystem invToDisplay)
    {
        PlayerMovement.accesingInventory = true;
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invToDisplay);
    }
}
