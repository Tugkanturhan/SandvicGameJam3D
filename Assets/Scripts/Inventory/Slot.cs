using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   public bool hovering;

   private ItemSO heldItem;
   private int itemAmount;

   private Image IconImage;
   private TextMeshProUGUI amountText;

   private void Awake()
   {
      IconImage = transform.GetChild(0).GetComponent<Image>();
      amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
   }

   public ItemSO GetItem()
   {
      return heldItem;
   }
   public int GetItemAmount()
   {
      return itemAmount;
   }
   public void SetItem(ItemSO item, int amount=1)
   {
      heldItem = item;
      itemAmount = amount;

      UpdateSlot();
   }
   public void UpdateSlot()
   {
      if (heldItem != null){
        IconImage.enabled = true;
        IconImage.sprite = heldItem.Icon;
        amountText.text = itemAmount.ToString();
      }
      else
      {
         IconImage.enabled = false;
         amountText.text = "";
      }
   }

   public int AddAmount(int amountToAdd)
   {
    itemAmount += amountToAdd;
    UpdateSlot();
    return itemAmount;
    }
    public int RemoveAmount(int amountToRemove)
    {
        itemAmount -= amountToRemove;
        if (itemAmount <= 0){
            ClearSlot();
        }
        else
        {
            UpdateSlot();
        }
        return itemAmount;
    }

    public void ClearSlot()
    {
        heldItem = null;
        itemAmount = 0;
        UpdateSlot();
    }

    public bool HasItem() {
        return heldItem != null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
