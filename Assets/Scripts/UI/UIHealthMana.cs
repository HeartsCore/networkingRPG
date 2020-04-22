using UnityEngine;
using UnityEngine.UI;


public class UIHealthMana : MonoBehaviour
{
    #region Singleton
    public static UIHealthMana instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of StatsUI found!");
            return;
        }
        instance = this;
    }
    #endregion


    #region Fields
    public GameObject Panel;
    public Slider HealthSlider;
    public Text HealthStatus;
    public Slider ExperienceSlider;
    public Text ExperienceStatus;
    #endregion


    #region Private Data
    private StatsManager _manager;
    private int _currentHealth;
    private float _currentExperience;
    #endregion


    #region Unity Methods
    private void Start()
    {
        Panel.SetActive(true);
    }

    private void Update()
    {
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
        
        if (_currentHealth != _manager.Health)
        {
            _currentHealth = _manager.Health;
            HealthSlider.value = HealthPercent();
            HealthStatus.text = _manager.Health + " / " + _manager.MaxHealth;

        }
        if (_currentExperience != _manager.Experience)
        {
            _currentExperience = _manager.Experience;
            ExperienceSlider.value = ExperiencePercent();
            ExperienceStatus.text = _manager.Experience + " / " + _manager.NextLevelExperience;

        }

    }

    private float HealthPercent()
    {
        return (_manager.Health != 0 && _manager.MaxHealth != 0) ? (float)_manager.Health / (float)_manager.MaxHealth : 0;
    }

    private float ExperiencePercent()
    {
        return (_manager.Experience != 0 && _manager.NextLevelExperience != 0) ? (float)_manager.Experience / (float)_manager.NextLevelExperience : 0;
    }
    #endregion
}
