﻿public class PlayerStats : UnitStats 
{
    #region Private Data
    private UserData _data;

    private StatsManager _manager;
    #endregion


    #region Properties
    public override int CurHealth
    {
        get
        {
            return base.CurHealth;
        }
        set
        {
            base.CurHealth = value;
            _data.CurHealth = CurHealth;
        }
    }
    
    public StatsManager manager {
        set {
            _manager = value;
            _manager.Health = base.CurHealth;
            _manager.MaxHealth = HealthMax;
            _manager.Damage = Damage.GetValue();
            _manager.Armor = Armor.GetValue();
            _manager.MoveSpeed = MoveSpeed.GetValue();
        }
    }
    #endregion


    #region Networking Methods
    public override void OnStartServer()
    {
        base.OnStartServer();
        OnHealthChanged += HealthChanged;
        //playerProgress.OnLevelUp += Changed;
        Damage.OnStatChanged += DamageChanged;
        Armor.OnStatChanged += ArmorChanged;
        MoveSpeed.OnStatChanged += MoveSpeedChanged;
    }
    #endregion


    #region Methods
    public void Load(UserData data)
    {
        _data = data;
        CurHealth = _data.CurHealth;
        if (_data.StatDamage > 0)
        {
            Damage.BaseValue = _data.StatDamage;
        }

        if (_data.StatArmor > 0)
        {
            Armor.BaseValue = _data.StatArmor;
        }

        if (_data.StatMoveSpeed > 0)
        {
            MoveSpeed.BaseValue = _data.StatMoveSpeed;
        }
    }
    

    private void DamageChanged(int value)
    {
        if (Damage.BaseValue != _data.StatDamage)
        {
            _data.StatDamage = Damage.BaseValue;
        }

        if (_manager != null)
        {

            _manager.Damage = value;
        }
    }

    private void ArmorChanged(int value)
    {
        if (Armor.BaseValue != _data.StatArmor)
        {
            _data.StatArmor = Armor.BaseValue;
        }

        if (_manager != null)
        {
            _manager.Armor = value;
        }
    }

    private void MoveSpeedChanged(int value)
    {
        if (MoveSpeed.BaseValue != _data.StatMoveSpeed)
        {
            _data.StatMoveSpeed = MoveSpeed.BaseValue;
        }

        if (_manager != null)
        {
            _manager.MoveSpeed = value;
        }
    }

    private void HealthChanged(int value) 
    {
        if (base.CurHealth != _data.CurHealth)
        {
            _data.CurHealth = base.CurHealth;
        }
        if (_manager != null) _manager.Health = value;
    }
    #endregion
}
