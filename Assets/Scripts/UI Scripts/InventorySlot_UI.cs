using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Security.Principal;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] public GameObject slotHighlight;
    [SerializeField] private InventorySlot assignedInventorySlot;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }
    ControlManager controlManager;
    bool isSelected;

    private void Awake()
    {
        controlManager = FindFirstObjectByType<ControlManager>();
        ClearSlot();

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();

        AddEventTriggers();
    }

     private void OnEnable()
    {
        controlManager.select.action.started += Select;
        controlManager.split.action.started += Split;
    }
    private void OnDisable()
    {
        controlManager.select.action.started -= Select;
        controlManager.split.action.started -= Split;
    }

    void Update()
    {
        if(PlayerMovement.accessingInventory){slotHighlight.SetActive(isSelected);}
    }

    public void TestPrint()
    {
        print("Test");
    }


    // Add EventTrigger component and setup event listeners for highlight detection and clicks
    private void AddEventTriggers()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

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

        // PointerClick (detect left and right mouse clicks)
        EventTrigger.Entry pointerClick = new EventTrigger.Entry();
        pointerClick.eventID = EventTriggerType.PointerClick;
        //pointerClick.callback.AddListener((eventData) => OnPointerClick((PointerEventData)eventData));
        trigger.triggers.Add(pointerClick);
    }

     private void Select(InputAction.CallbackContext obj)
    {
        if(PlayerMovement.accessingInventory == true)
        {
           OnLeftUISlotClick();
        }     
    }
    private void Split(InputAction.CallbackContext obj)
    {
        if(PlayerMovement.accessingInventory == true)
        {
            OnRightUISlotClick();
        }
    }  

    /* void OnPointerClick(PointerEventData eventData)
    {
        if(!ControlManager.isController)
        {
            //print("HELP");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // Handle left-click
                OnLeftUISlotClick();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // Handle right-click
                OnRightUISlotClick();
            }
        }
        else
        {
            print("Heyguys");
        }  
    } */

    public void Selected()
    {
        isSelected = true;
        //slotHighlight.SetActive(true);
    }

    public void Deselected()
    {
        isSelected = false;
        //slotHighlight.SetActive(false);
    }
        

    public void OnLeftUISlotClick()
    {
        // Handle left-click behavior
        if(isSelected){ParentDisplay?.HandleSlotLeftClick(this);}
    }

    public void OnRightUISlotClick()
    {
        // Handle right-click behavior
        if(isSelected){ParentDisplay?.HandleSlotRightClick(this);}
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
