using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public ItemSO testItem;
    public ItemSO testItem2;

    public GameObject hotbarObject;
    public GameObject inventorySlotParent;

    public List<Slot> hotbarSlots = new List<Slot>();
    public List<Slot> inventorySlots = new List<Slot>();
    public List<Slot> allSlots = new List<Slot>();

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange( hotbarObject.GetComponentsInChildren<Slot>());
        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            AddItem(testItem, 2);
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            AddItem(testItem2, 3);
        }
    }
    public void AddItem(ItemSO itemToAdd, int amount) 
    {

        int remainingAmount = amount;
        foreach (Slot slot in allSlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetItemAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remainingAmount);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remainingAmount -= amountToAdd;
                    if (remainingAmount <= 0)
                    {
                        return;
                    }
                }
            }
        }

        foreach (Slot slot in allSlots) {
            if (!slot.HasItem()){
                int amountToPlace = Mathf.Min(remainingAmount, itemToAdd.maxStackSize);
                slot.SetItem(itemToAdd, amountToPlace);
                remainingAmount -= amountToPlace;
                if (remainingAmount <= 0)
                {
                    return;
                }
            }
        }

        if (remainingAmount > 0)
        {
            Debug.Log("Inventory is full, could not add " + itemToAdd.itemName + " x " + remainingAmount);
        }
    }
}
