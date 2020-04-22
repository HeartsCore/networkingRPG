using System;
using UnityEngine.Networking;


public class UpgradeableSkill : Skill
{
    #region Fields
    public event Action<UpgradeableSkill, int> OnSetLevel;

    [SyncVar(hook = "LevelHook")] int _level = 1;
    #endregion


    #region Properties
    public virtual int Level
    {
        get 
        { 
            return _level; 
        }
        set
        {
            _level = value;
            if (OnSetLevel != null)
            {
                OnSetLevel.Invoke(this, Level);
            }
        }
    }
    #endregion


    #region Methods
    void LevelHook(int newLevel)
    {
        Level = newLevel;
    }
    #endregion
}
