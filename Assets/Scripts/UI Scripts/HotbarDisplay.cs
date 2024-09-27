using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarDisplay : StaticInventoryDisplay
{
    private int maxIndexSize = 8;
    private int currentIndex = 0;

    private void Awake()
    {

    }

    protected override void Start()
    {
        base.Start();

        currentIndex = 0;
        maxIndexSize = slots.Length - 1;

        slots[currentIndex].ToggleHighlight();
    }

    protected override void OnEnable()
    {
        base.OnEnable();


    }

    protected override void OnDisable()
    {
        base.OnDisable();


    }

    private void Update()
    {
        //Mousewheel implementation
    }

    private void UseItem()
    {
        if (slots[currentIndex].AssignedInventorySlot != null) { slots[currentIndex].AssignedInventorySlot.ItemData.UseItem(); }
    }

    private void ChangeIndex(int direction)
    {
        slots[currentIndex].ToggleHighlight();
        currentIndex += direction;

        if (currentIndex > maxIndexSize) currentIndex = 0;
        if (currentIndex < 0) currentIndex = maxIndexSize;

        slots[currentIndex].ToggleHighlight();
    }

    private void SetIndex(int newIndex)
    {
        slots[currentIndex].ToggleHighlight();
        if (newIndex < 0) newIndex = 0;
        if (newIndex > maxIndexSize) newIndex = maxIndexSize;

        currentIndex = newIndex;

        slots[currentIndex].ToggleHighlight();
    }
}
