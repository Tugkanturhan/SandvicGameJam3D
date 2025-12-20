using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ItemSO testItem;
    public ItemSO testItem2;

    public GameObject hotbarObject;
    public GameObject inventorySlotParent;
    public GameObject Container;

    public Image dragIcon;

    public float pickupRange = 3f;
    private Item lookedAtItem = null;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lookAtRenderer = null;

    private int equippedHotbarIndex = 0; //0-5
    public float equippedOpacity = 0.9f;
    public float normalOpacity = 0.58f;

    public Transform Hand;
    private GameObject currentHandItem;


    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private Slot draggedSlot = null;
    private bool isDragging = false;

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange( hotbarObject.GetComponentsInChildren<Slot>());
        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Q)) {
        //   AddItem(testItem, 2);
        //}
        //else if (Input.GetKeyDown(KeyCode.E)) {
        //    AddItem(testItem2, 3);
        //}

        

        if (Input.GetKeyDown(KeyCode.Tab)){

            Container.SetActive(!Container.activeInHierarchy);
            //Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
            PlayerCam.Instance.updatingRotation=!PlayerCam.Instance.updatingRotation;
           if (Cursor.visible)
            {
               Cursor.lockState = CursorLockMode.None; // İmleci serbest bırak
            }
            else
            {
               Cursor.lockState = CursorLockMode.Locked; // İmleci merkeze kilitle (FPS oyunları için)
            }
        }

        DetectLookAtItem();
        Pickup();

        StartDrag();
        UpdateDragItemPosition();
        EndDrag();

        UpdateHotbarOpacity();
        HandleHotBarSelection();
        HandleDropEquippedItem();

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

    private void StartDrag() {

        if (Input.GetMouseButtonDown(0))
        {
            Slot hovered = GetHoveredSlot();

            if (hovered != null && hovered.HasItem()) {
                draggedSlot = hovered;
                isDragging = true;

                //Show drag item
                dragIcon.sprite = hovered.GetItem().Icon;
                dragIcon.color = new Color (1,1,1,0.5f);
                dragIcon.enabled = true;
            }
        }
    }

    private void EndDrag() {
        if (Input.GetMouseButtonUp(0) && isDragging) {
            Slot hovered = GetHoveredSlot();

            if (hovered != null) {
                HandleDrop(draggedSlot,hovered);

                dragIcon.enabled = false;
                draggedSlot = null;
                isDragging = false;
            }
        }
    }

    private Slot GetHoveredSlot() {
        foreach(Slot s in allSlots) {
            if (s.hovering)
            return s;
        }

        return null;
    }

    private void HandleDrop(Slot from, Slot to) {
        if (from == to) return;


        //Stacking
        if (to.HasItem() && to.GetItem() == from.GetItem()) {
            int max = to.GetItem().maxStackSize;
            int space = max - to.GetItemAmount();

            if (space >0) {

                int move = Mathf.Min(space,from.GetItemAmount());

                to.SetItem(to.GetItem(),to.GetItemAmount() + move);
                from.SetItem(from.GetItem(),from.GetItemAmount() - move);

                if(from.GetItemAmount() <= 0)
                    from.ClearSlot();

                return;
            }
        }

        //Different Item
        if (to.HasItem()) {

            ItemSO tempItem =to.GetItem();
            int tempAmount = to.GetItemAmount();

            to.SetItem(from.GetItem(),from.GetItemAmount());
            from.SetItem(tempItem,tempAmount);
            return;
        }

        //Empty Slot
        to.SetItem(from.GetItem(),from.GetItemAmount());
        from.ClearSlot();
    }

    private void UpdateDragItemPosition() {
        if (isDragging) {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    private void Pickup() 
    {
        if (lookAtRenderer != null && Input.GetKeyDown(KeyCode.F))
        {
            Item item = lookAtRenderer.GetComponent<Item>();
            if(item !=null)
            {
                AddItem(item.item, item.amount);    
                Destroy(item.gameObject);   
                EquipHandItem();
            }
        }
    }

    private void DetectLookAtItem() {
        if (lookAtRenderer !=null) {
            lookAtRenderer.material = originalMaterial;
            lookAtRenderer = null;
            originalMaterial = null;
        }

        Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange)){
            Item item = hit.collider.GetComponent<Item>();
            if (item !=null) {
                Renderer rend = item.GetComponent<Renderer>();
                if (rend !=null) {
                    originalMaterial = rend.material;
                    rend.material = highlightMaterial;
                    lookAtRenderer = rend;
                }
            }
        }
    }

    private void UpdateHotbarOpacity()
    {
        for(int i = 0; i < hotbarSlots.Count; i++)
        {
            Image icon = hotbarSlots[i].GetComponent<Image>();
            if(icon != null)
            {
                icon.color = (i == equippedHotbarIndex) ? new Color(1, 1, 1, equippedOpacity) : new Color(1, 1, 1, normalOpacity);
            }
        }
    }

    private void HandleHotBarSelection()
    {
        for(int i = 0; i < 6; i++)
        {
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                equippedHotbarIndex = i;
                UpdateHotbarOpacity();
                EquipHandItem();
            }
        }
    }

    private void HandleDropEquippedItem()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];

        if (!equippedSlot.HasItem()) return;

        ItemSO itemSO = equippedSlot.GetItem();
        GameObject prefab = itemSO.itemPrefab;

        if (prefab == null) return;

        // Eşyayı kameranın biraz önünde oluşturur
        GameObject dropped = Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);

        Item item = dropped.GetComponent<Item>();
        item.item = itemSO;
        item.amount = equippedSlot.GetItemAmount();

        equippedSlot.ClearSlot();
        EquipHandItem();
    }

    private void EquipHandItem()
    {
        if (currentHandItem != null) Destroy(currentHandItem);

        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];
        if (!equippedSlot.HasItem()) return;

        ItemSO item = equippedSlot.GetItem();
        if (item.handItemPrefab == null) return;

        currentHandItem = Instantiate(item.handItemPrefab, Hand);
        currentHandItem.transform.localPosition = Vector3.zero;
        currentHandItem.transform.localRotation = Quaternion.identity;
    }
}
