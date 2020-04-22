using UnityEngine;
using UnityEngine.UI;


public class StatsUI : MonoBehaviour {

    #region Singleton
    public static StatsUI instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one instance of StatsUI found!");
            return;
        }
        instance = this;
    }
    #endregion


    #region Private Data
    [SerializeField] private GameObject _statsUI;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _statPointsText;
    [SerializeField] private StatItem _damageStat;
    [SerializeField] private StatItem _armorStat;
    [SerializeField] private StatItem _moveSpeedStat;

    private StatsManager _manager;
    private int _currentDamage;
    private int _currentArmor;
    private int _currentMoveSpeed;
    private int _currentLevel;
    private int _currentStatPoints;
    private float _currentExp;
    private float _nextLevelExp;
    #endregion


    #region Unity Methods
    private void Start()
    {
        _statsUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Stats"))
        {
            _statsUI.SetActive(!_statsUI.activeSelf);
        }
        if (_manager != null)
        {
            CheckManagerChanges();
        }
    }
    #endregion


    #region Methods
    public void SetManager(StatsManager statsManager)
    {
        _manager = statsManager;
        CheckManagerChanges();
    }

    private void CheckManagerChanges()
    {
        // stat changes
        if (_currentDamage != _manager.Damage)
        {
            _currentDamage = _manager.Damage;
            _damageStat.ChangeStat(_currentDamage);
        }
        if (_currentArmor != _manager.Armor)
        {
            _currentArmor = _manager.Armor;
            _armorStat.ChangeStat(_currentArmor);
        }
        if (_currentMoveSpeed != _manager.MoveSpeed)
        {
            _currentMoveSpeed = _manager.MoveSpeed;
            _moveSpeedStat.ChangeStat(_currentMoveSpeed);
        }
        // progress changes
        if (_currentLevel != _manager.Level)
        {
            _currentLevel = _manager.Level;
            _levelText.text = _currentLevel.ToString();
        }
        if (_currentExp != _manager.Experience)
        {
            _currentExp = _manager.Experience;
        }
        if (_nextLevelExp != _manager.NextLevelExperience)
        {
            _nextLevelExp = _manager.NextLevelExperience;
        }
        if (_currentStatPoints != _manager.StatPoints)
        {
            _currentStatPoints = _manager.StatPoints;
            _statPointsText.text = _currentStatPoints.ToString();
            if (_currentStatPoints > 0) SetUpgradableStats(true);
            else SetUpgradableStats(false);
        }
    }

    private void SetUpgradableStats(bool active)
    {
        _damageStat.SetUpgradable(active);
        _armorStat.SetUpgradable(active);
        _moveSpeedStat.SetUpgradable(active);
    }

    public void UpgradeStat(StatItem stat)
    {
        if (stat == _damageStat) _manager.CmdUpgradeStat((int)StatType.Damage);
        else if (stat == _armorStat) _manager.CmdUpgradeStat((int)StatType.Armor);
        else if (stat == _moveSpeedStat) _manager.CmdUpgradeStat((int)StatType.MoveSpeed);
    }
    #endregion
}
