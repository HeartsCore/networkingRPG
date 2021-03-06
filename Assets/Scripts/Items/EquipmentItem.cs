﻿using UnityEngine;


[CreateAssetMenu(fileName = "New equipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : Item 
{
    #region Fields
    public EquipmentSlotType EquipSlot;

    public int DamageModifier;
    public int ArmorModifier;
    public int SpeedModifier;
    #endregion


    #region Methods
    public override void Use(Player player) 
    {
        player.Inventory.RemoveItem(this);
        EquipmentItem oldItem = player.Equipment.EquipItem(this);
        if (oldItem != null) player.Inventory.AddItem(oldItem);
        base.Use(player);
    }

    public virtual void Equip(Player player) 
    {
        if (player != null) 
        {
            UnitStats stats = player.Character.Stats;
            stats.Damage.AddModifier(DamageModifier);
            stats.Armor.AddModifier(ArmorModifier);
            stats.MoveSpeed.AddModifier(SpeedModifier);
        }
    }

    public virtual void Unequip(Player player) 
    {
        if (player != null) {
            UnitStats stats = player.Character.Stats;
            stats.Damage.RemoveModifier(DamageModifier);
            stats.Armor.RemoveModifier(ArmorModifier);
            stats.MoveSpeed.RemoveModifier(SpeedModifier);
        }
    }
    #endregion
}

public enum EquipmentSlotType { Head, Chest, Legs, RighHand, LeftHand }