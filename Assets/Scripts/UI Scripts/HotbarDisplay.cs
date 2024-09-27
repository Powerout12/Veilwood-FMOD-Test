using UnityEngine;

public class HotbarDisplay : MonoBehaviour
{
    public InputManager inputManager;  // Reference to the InputManager
    public InventorySlot_UI[] hotbarSlots;   // Array of hotbar slots (InventorySlot_UI)

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
        InventorySlot_UI selectedSlot = hotbarSlots[slotIndex];

        if (selectedSlot.AssignedInventorySlot != null && selectedSlot.AssignedInventorySlot.ItemData != null)
        {
            //Debug.Log($"Selected item from hotbar slot {slotIndex + 1}: {selectedSlot.AssignedInventorySlot.ItemData.displayName}");

          
            selectedSlot.AssignedInventorySlot.ItemData.UseItem();
        }
        else
        {
            Debug.Log($"No item in hotbar slot {slotIndex + 1}");
        }
    }
}
