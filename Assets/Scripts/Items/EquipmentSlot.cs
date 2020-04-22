using UnityEngine;
using UnityEngine.UI;


public class EquipmentSlot : MonoBehaviour 
{
    #region Fields
    public Image icon;
    public Button unequipButton;
    public Equipment equipment;
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
        unequipButton.interactable = true;
    }

    public void ClearSlot() 
    {
        _item = null;
        icon.sprite = null;
        icon.enabled = false;
        unequipButton.interactable = false;
    }

    public void Unequip() 
    {
        equipment.UnequipItem(_item);
    }
    #endregion
}
