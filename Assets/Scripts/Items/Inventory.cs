using UnityEngine;
using UnityEngine.Networking;


public class Inventory : NetworkBehaviour 
{
    #region Fields
    public int Space = 20;
    public event SyncList<Item>.SyncListChanged OnItemChanged;

    public Player Player;
    public SyncListItem Items = new SyncListItem();
    #endregion


    #region Private Data
    private UserData _data;
    #endregion


    #region Unity Methods
    public override void OnStartLocalPlayer() 
    {
        Items.Callback += ItemChanged;
    }

    [Command]
    void CmdUseItem(int index)
    {
        if (Items[index] != null)
        {
            Items[index].Use(Player);
        }
    }
    [Command]
    void CmdDropItem(int index)
    {
        if (Items[index] != null)
        {
            Drop(Items[index]);
            RemoveItem(Items[index]);
            //items.RemoveAt(index);
        }
    }
    #endregion


    #region Methods
    public void Load(UserData data)
    {
        _data = data;
        for (int i = 0; i < data.Inventory.Count; i++)
        {
            Items.Add(ItemBase.GetItem(data.Inventory[i]));
        }
    }
    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex) 
    {
        OnItemChanged(op, itemIndex);
    }

    public bool AddItem(Item item) 
    {
        if (Items.Count < Space) 
        {
            Items.Add(item);
            _data.Inventory.Add(ItemBase.GetItemId(item));
            return true;
        } 
        else return false;
    }

    public void UseItem(Item item) 
    {
        CmdUseItem(Items.IndexOf(item));
    }
    
    public void DropItem(Item item) {
        CmdDropItem(Items.IndexOf(item));
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        _data.Inventory.Remove(ItemBase.GetItemId(item));
    }

    private void Drop(Item item)
    {
        ItemPickup pickupItem = Instantiate(item.pickupPrefab, Player.Character.transform.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
        pickupItem.item = item;
        NetworkServer.Spawn(pickupItem.gameObject);
    }
    #endregion
}