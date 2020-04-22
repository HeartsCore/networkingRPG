using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stat
{
    #region Fields
    public event Action<int> OnStatChanged;
    #endregion


    #region Private Data
    [SerializeField] private int _baseValue;
    private List<int> _modifiers = new List<int>();
    #endregion


    #region Properties
    public int BaseValue {
        get { return _baseValue; }
        set {
            _baseValue = value;
            if (OnStatChanged != null) OnStatChanged(GetValue());
        }
    }
    #endregion


    #region Methods
    public int GetValue() 
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier( int modifier) 
    {
        if (modifier != 0) {
            _modifiers.Add(modifier);
            OnStatChanged?.Invoke(GetValue());
        }
    }

    public void RemoveModifier(int modifier) 
    {
        if (modifier != 0) {
            _modifiers.Remove(modifier);
            OnStatChanged?.Invoke(GetValue());
        }
    }
    #endregion
}
