using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour 
{
    #region Fields
    public Image icon;
    public Button removeButton;
    public Inventory inventory;
    #endregion


    #region Private Data
    private Item _item;
    #endregion


    #region Methods
    public void SetItem(Item newItem) 
    {
        _item = newItem;
        icon.sprite = _item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() 
    {
        _item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() 
    {
        inventory.DropItem(_item);
    }

    public void UseItem() 
    {
        if (_item != null) inventory.UseItem(_item);
    }
    #endregion
}
