﻿using UnityEngine;


public class InventoryUI : MonoBehaviour {

    #region Singleton
    public static InventoryUI instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one instance of InventoryUI found!");
            return;
        }
        instance = this;
    }
    #endregion


    #region Private Data
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private InventorySlot _slotPrefab;

    private InventorySlot[] _slots;
    private Inventory _inventory;
    #endregion
    

    #region Unity Methods
    private void Start() 
    {
        _inventoryUI.SetActive(false);
    }

    private void Update() {
        if (Input.GetButtonDown("Inventory")) {
            _inventoryUI.SetActive(!_inventoryUI.activeSelf);
        }
    }
    #endregion


    #region Methods
    public void SetInventory(Inventory newInventory) 
    {
        _inventory = newInventory;
        _inventory.OnItemChanged += ItemChanged;
        InventorySlot[] childs = _itemsParent.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < childs.Length; i++) Destroy(childs[i].gameObject);
        _slots = new InventorySlot[_inventory.Space];
        for (int i = 0; i < _inventory.Space; i++) 
        {
            _slots[i] = Instantiate(_slotPrefab, _itemsParent);
            _slots[i].inventory = _inventory;
            if (i < _inventory.Items.Count) _slots[i].SetItem(_inventory.Items[i]);
            else _slots[i].ClearSlot();
        }
    }

    private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex) 
    {
        for (int i = 0; i < _slots.Length; i++) 
        {
            if (i < _inventory.Items.Count) _slots[i].SetItem(_inventory.Items[i]);
            else _slots[i].ClearSlot();
        }
    }
    #endregion
}
