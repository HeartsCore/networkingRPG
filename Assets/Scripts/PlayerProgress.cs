using UnityEngine;


public class PlayerProgress : MonoBehaviour 
{
    #region Private Data
    private int _level = 1;
    private int _statPoints;
    private float _exp;
    private float _nextLevelExp = 100;
    private float _experiencePoints = 100.0f;
    private int _numberOfPoints = 3;

    private UserData _data;
    private int _skillPoints;
    private StatsManager _manager;
    #endregion


    #region Properties
    public StatsManager Manager {
        set {
            _manager = value;
            _manager.Experience = _exp;
            _manager.NextLevelExperience = _nextLevelExp;
            _manager.Level = _level;
            _manager.StatPoints = _statPoints;
            _manager.SkillPoints = _skillPoints;

        }
    }
    #endregion


    #region Methods
    public void Load(UserData data)
    {
        _data = data;
        if (_data.Level > 0)
        {
            _level = _data.Level;
        }

        _statPoints = _data.StatPoints;
        _skillPoints = _data.SkillPoints;
        _exp = _data.Exp;
        if (_data.NextLevelExp > 0)
        {
            _nextLevelExp = _data.NextLevelExp;
        }
    }

    public bool RemoveStatPoint() {
        if (_statPoints > 0)
        {
            _data.StatPoints = --_statPoints;
            if (_manager != null) _manager.StatPoints = _statPoints;
            return true;
        }
        return false;
    }

    public void AddExp(float addExp) {
        _data.Exp = _exp += addExp;
        while (_exp >= _nextLevelExp)
        {
            _data.Exp = _exp -= _nextLevelExp;
            LevelUP();
        }
        if (_manager != null)
        {
            _manager.Experience = _exp;
            _manager.Level = _level;
            _manager.NextLevelExperience = _nextLevelExp;
            _manager.StatPoints = _statPoints;
            _manager.SkillPoints = _skillPoints;
        }
    }

    private void LevelUP() {
        _data.Level = ++_level;
        _data.NextLevelExp = _nextLevelExp += _experiencePoints;
        _data.StatPoints = _statPoints += _numberOfPoints;
        _data.CurHealth = _manager.Player.Character.Stats.CurHealth = _manager.Player.Character.Stats.HealthMax;
        _manager.Player.Character.Stats.OnHealthChanged?.Invoke(100);
        _data.SkillPoints = _skillPoints += 1;
    }

    public bool RemoveSkillPoint()
    {
        if (_skillPoints > 0)
        {
            _data.SkillPoints = --_skillPoints;
            if (_manager != null)
            {
                _manager.SkillPoints = _skillPoints;
            }

            return true;
        }
        return false;
    }
    #endregion
}
