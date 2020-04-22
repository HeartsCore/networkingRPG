using UnityEngine;
using UnityEngine.UI;


public class SkillTreeItem : MonoBehaviour
{
    #region Private Data
    [SerializeField] private Image _icon;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject _holder;
    #endregion


    #region Methods
    public void SetSkill(UpgradeableSkill skill)
    {
        if (skill != null)
        {
            _icon.sprite = skill.Icon;
            skill.OnSetLevel += ChangeLevel;
            ChangeLevel(skill, skill.Level);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetUpgradable(bool active)
    {
        _holder.SetActive(active);
    }

    private void ChangeLevel(UpgradeableSkill skill, int newLevel)
    {
        _levelText.text = newLevel.ToString();
    }
    #endregion
}
