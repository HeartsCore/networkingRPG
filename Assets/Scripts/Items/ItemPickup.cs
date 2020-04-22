using UnityEngine;


public class ItemPickup : Interactable 
{
    #region Fields
    public Item item;
    public float lifetime;
    #endregion


    #region Unity Methods
    private void Update() 
    {
        if (isServer) {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0) Destroy(gameObject);
        }
    }
    #endregion


    #region Methods
    public override bool Interact(GameObject user) 
    {
        return PickUp(user);
    }

    public bool PickUp(GameObject user) 
    {
        Character character = user.GetComponent<Character>();
        if (character != null && character.Player.Inventory.AddItem(item)) 
        {
            Destroy(gameObject);
            return true;
        } 
        else return false;
    }
    #endregion
}
