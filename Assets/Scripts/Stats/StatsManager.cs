using UnityEngine.Networking;


public class StatsManager : NetworkBehaviour 
{
    #region Fields
    [SyncVar] public int Damage;
    [SyncVar] public int Armor;
    [SyncVar] public int MoveSpeed;
    [SyncVar] public int Health;
    [SyncVar] public int MaxHealth;

    [SyncVar] public int Level;
    [SyncVar] public int StatPoints;
    [SyncVar] public float Experience;
    [SyncVar] public float NextLevelExperience;
    [SyncVar] public int SkillPoints;

    public Player Player;
    #endregion


    #region Network Methods
    [Command]
    public void CmdUpgradeStat(int stat)
    {
        if (Player.Progress.RemoveStatPoint())
        {
            switch (stat)
            {
                case (int)StatType.Damage: Player.Character.Stats.Damage.BaseValue++; break;
                case (int)StatType.Armor: Player.Character.Stats.Armor.BaseValue++; break;
                case (int)StatType.MoveSpeed: Player.Character.Stats.MoveSpeed.BaseValue++; break;
            }
        }
    }

    [Command]
    public void CmdUpgradeSkill(int index)
    {
        if (Player.Progress.RemoveSkillPoint())
        {
            UpgradeableSkill skill = Player.Character.UnitSkills[index] as UpgradeableSkill;
            if (skill != null)
            {
                skill.Level++;
            }
        }
    }
    #endregion
}