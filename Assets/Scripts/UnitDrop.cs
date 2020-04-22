using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Unit))]
public class UnitDrop : NetworkBehaviour 
{
    #region Private Data
    [SerializeField] private DropItem[] _dropItems = new DropItem[0];
    #endregion


    #region Network Methods
    public override void OnStartServer() 
    {
        GetComponent<Unit>().EventOnDie += Drop;
    }
    #endregion


    #region Methods
    private void Drop() 
    {
        for (int i = 0; i < _dropItems.Length; i++) 
        {
            if (Random.Range(0, 100f) <= _dropItems[i].rate) 
            {
                ItemPickup pickupItem = Instantiate(_dropItems[i].item.pickupPrefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
                pickupItem.item = _dropItems[i].item;
                NetworkServer.Spawn(pickupItem.gameObject);
            }
        }
    }
    #endregion

    [System.Serializable]
    struct DropItem 
    {
        public Item item;
        [Range(0, 100f)]
        public float rate;
    }
}
