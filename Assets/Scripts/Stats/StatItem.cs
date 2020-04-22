using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour 
{
    #region Private Data
    [SerializeField] private Text _value;
    [SerializeField] private Button _upgradeButton;
    #endregion


    #region Methods
    public void ChangeStat(int stat) 
    {
        _value.text = stat.ToString();
    }

    public void SetUpgradable(bool upgradable) 
    {
        _upgradeButton.gameObject.SetActive(upgradable);
    }
    #endregion
}
