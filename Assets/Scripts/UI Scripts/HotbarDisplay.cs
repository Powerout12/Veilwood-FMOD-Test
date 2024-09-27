using UnityEngine;

public class HotbarDisplay : MonoBehaviour
{
    public InputManager inputManager;  // Reference to the InputManager
    public InventorySlot_UI[] hotbarSlots;   // Array of hotbar slots (InventorySlot_UI)
    public static InventorySlot_UI currentSlot;

    private void OnEnable()
    {
        // Subscribe to the input manager's OnNumberPressed event
        if (inputManager != null)
        {
            inputManager.OnNumberPressed += HandleNumberPressed;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        if (inputManager != null)
        {
            inputManager.OnNumberPressed -= HandleNumberPressed;
        }
    }

    // Method to handle number presses
    private void HandleNumberPressed(int number)
    {
        // Assuming hotbarSlots array has exactly 9 slots, 1-based (1 to 9)
        if (number > 0 && number <= hotbarSlots.Length)
        {
            SelectHotbarSlot(number - 1);  // Hotbar slots are 0-indexed
        }
    }

    private void SelectHotbarSlot(int slotIndex)
    {
        // Example logic for selecting the item in the hotbar slot
        currentSlot = hotbarSlots[slotIndex];

        if (currentSlot.AssignedInventorySlot != null && currentSlot.AssignedInventorySlot.ItemData != null)
        {
            //Debug.Log($"Selected item from hotbar slot {slotIndex + 1}: {currentSlot.AssignedInventorySlot.ItemData.displayName}");

            //This is where you can assign the currently selected hotbar
          
            currentSlot.AssignedInventorySlot.ItemData.UseItem();
        }
        else
        {
            Debug.Log($"No item in hotbar slot {slotIndex + 1}");
        }
    }
}
