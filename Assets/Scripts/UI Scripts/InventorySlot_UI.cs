using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private GameObject slotHighlight;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;
    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();

       
        AddEventTriggers();
    }

    // Add EventTrigger component and setup event listeners for highlight detection
    private void AddEventTriggers()
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // PointerEnter (highlighted)
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => { OnHighlight(true); });
        trigger.triggers.Add(pointerEnter);

        // PointerExit (no longer highlighted)
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => { OnHighlight(false); });
        trigger.triggers.Add(pointerExit);
    }

    private void OnHighlight(bool selected)
    {

        itemName.gameObject.SetActive(selected);
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.icon;
            itemSprite.color = Color.white;
            itemName.text = slot.ItemData.name;
            if (slot.StackSize > 1)
                itemCount.text = slot.StackSize.ToString();
            else
                itemCount.text = "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void ToggleHighlight()
    {
        slotHighlight.SetActive(!slotHighlight.activeInHierarchy);
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
        itemName.text = "";
        itemName.gameObject.SetActive(false);

    }
}
