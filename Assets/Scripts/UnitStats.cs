using System;
using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour 
{
    #region Fields
    public Action <int> OnHealthChanged;
    
    public Stat Damage;
    public Stat Armor;
    public Stat MoveSpeed;

    public int HealthMax = 100;
    #endregion


    #region Private Data
    [SerializeField] protected int _maxHealth;
    [SyncVar] protected int _curHealth;
    #endregion


    #region Properties
    public virtual int CurHealth { get { return _curHealth; }  set { _curHealth = value; } }
    #endregion


    #region Unity Methods
    public override void OnStartServer() {
        _curHealth = _maxHealth;
    }
    #endregion


    #region Methods
    public virtual void TakeDamage(int damage)
    {
        damage -= Armor.GetValue();
        if (damage > 0)
        {
            CurHealth -= damage;
            OnHealthChanged?.Invoke(_curHealth);
            if (CurHealth <= 0)
            {
                CurHealth = 0;
            }
        }
    }

    public void AddHealth(int amount)
    {
        CurHealth += amount;
        if (CurHealth > _maxHealth)
        {
            CurHealth = _maxHealth;
        }
    }

    public void SetHealthRate(float rate) {

        CurHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
        OnHealthChanged?.Invoke(_curHealth);
    }
    #endregion


    #region Probs
    //#pragma warning disable CS0109 // member does not hide accessible member
    //    new public Collider collider;


    //    [Header("Target")]
    //    [SyncVar] GameObject _target;
    //    public UnitStats target
    //    {
    //        get { return _target != null ? _target.GetComponent<UnitStats>() : null; }
    //        set { _target = value != null ? value.gameObject : null; }
    //    }

    //    [Header("Level")]
    //    [SyncVar] public int level = 1;

    //    //[SerializeField] protected LinearInt _healthMax = new LinearInt { baseValue = 100 };

    //    [SyncVar] int _health = 1;
    //    public int health
    //    {
    //        get { return Mathf.Min(_health, healthMax); } // min in case hp>hpmax after buff ends etc.
    //        set { _health = Mathf.Clamp(value, 0, healthMax); }
    //    }
    //    [SyncVar] int _mana = 1;
    //    public float HealthPercent()
    //    {
    //        return (_curHealth != 0 && healthMax != 0) ? (float)_curHealth / (float)healthMax : 0;
    //    }

    //public int mana
    //{
    //    get { return Mathf.Min(_mana, manaMax); } // min in case hp>hpmax after buff ends etc.
    //    set { _mana = Mathf.Clamp(value, 0, manaMax); }
    //}
    //[SerializeField] protected LinearInt _manaMax = new LinearInt { baseValue = 100 };
    //public virtual int manaMax
    //{
    //    get
    //    {

    //        // base + passives + buffs
    //        return _manaMax.Get(level);// + passiveBonus + buffBonus;
    //    }
    //}
    //public virtual int healthMax
    //{
    //    get
    //    {

    //        // base + passives + buffs
    //        return _healthMax.Get(level);// + passiveBonus + buffBonus;
    //    }
    //}

    //public float HealthPercent()
    //{
    //    return (health != 0 && healthMax != 0) ? (float)health / (float)healthMax : 0;
    //}
    //public float ManaPercent()
    //{
    //    return (mana != 0 && manaMax != 0) ? (float)mana / (float)manaMax : 0;
    //}

    //public float ManaPercent()
    //{
    //    return (mana != 0 && manaMax != 0) ? (float)mana / (float)manaMax : 0;
    //}
    #endregion
}
