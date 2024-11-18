using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay chestPanel;
    public DynamicInventoryDisplay playerBackpackPanel;

    PlayerInventoryHolder inventoryHolder;

    private bool isBackpackOpen = false;  
 
    public bool readyToPress;
    ControlManager controlManager;

    private void Awake()
    {
        readyToPress = true;
        chestPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);

        inventoryHolder = FindObjectOfType<PlayerInventoryHolder>();
        
        controlManager = FindFirstObjectByType<ControlManager>();
    }

    void Start()
    {
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested?.Invoke(inventoryHolder.secondaryInventorySystem);
        CloseBackpack();
        readyToPress = true;
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
        controlManager.openInventory.action.started += OpenInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
        controlManager.openInventory.action.started -= OpenInventory;
    }

    void Update()
    {
        
        
    }

    private void OpenInventory(InputAction.CallbackContext obj)
    {
       
        if(!PlayerMovement.accessingInventory)
        {
            PlayerInventoryHolder.OnPlayerBackpackDisplayRequested?.Invoke(inventoryHolder.secondaryInventorySystem);
            return;
        }
        
        if (chestPanel.gameObject.activeInHierarchy)
        {
            CloseInventory();
        }
        else if (isBackpackOpen)
        {
            CloseBackpack();
            print("Closing backpack");
        }
    }

    void DisplayInventory(InventorySystem invToDisplay)
    {
        //Chest Inventory
        PlayerMovement.accessingInventory = true;
        chestPanel.gameObject.SetActive(true);
        playerBackpackPanel.gameObject.SetActive(true);
        chestPanel.RefreshDynamicInventory(invToDisplay);
       
        isBackpackOpen = true;

    }

    void DisplayPlayerBackpack(InventorySystem invToDisplay)
    {
        if (!isBackpackOpen)
        {
            print("Opening");
            PlayerMovement.accessingInventory = true;
            playerBackpackPanel.gameObject.SetActive(true);
            playerBackpackPanel.RefreshDynamicInventory(invToDisplay);
            isBackpackOpen = true; 
            readyToPress = false;
        }
    }

    void CloseInventory()
    {
        //Close Chest
        chestPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);
        PlayerMovement.accessingInventory = false;

        isBackpackOpen = false; 
    }

    void CloseBackpack()
    {
        print("Closing");
        //HandItemManager.Instance.CheckSlotForTool();
        playerBackpackPanel.gameObject.SetActive(false);
        PlayerMovement.accessingInventory = false;
        isBackpackOpen = false; 
    }
}
