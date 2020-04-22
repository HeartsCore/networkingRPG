using UnityEngine;


public class HealSkill : UpgradeableSkill
{
    #region Private Data
    [SerializeField] private int _baseHealAmount = 10;
    [SerializeField] private int _healAmountByLevel = 1;
    [SerializeField] private ParticleSystem _particle;
    private StatsManager _manager;
    private int _healAmount;
    #endregion


    #region Properties
    public override int Level
    {
        set
        {
            base.Level = value;
            _healAmount = _baseHealAmount + _healAmountByLevel * Level;
        }
    }
    #endregion


    #region Methods
    protected override void OnCastComplete()
    {
        if (isServer)
        {
            _unit.Stats.AddHealth(_healAmount);
            _manager.Player.Character.Stats.OnHealthChanged?.Invoke(_healAmount);
        }
        else _particle.Play();
        base.OnCastComplete();
    }
    #endregion
}
