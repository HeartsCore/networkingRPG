using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject 
{
    #region Fields
    new public string name = "New Item";
    public Sprite icon = null;
    public ItemPickup pickupPrefab;
    #endregion

    public virtual void Use(Player player) { }
}
