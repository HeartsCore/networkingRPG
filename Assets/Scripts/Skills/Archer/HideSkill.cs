using UnityEngine;


public class HideSkill : UpgradeableSkill 
{
    #region Private Data
    [SerializeField] private float _initialCastTime = 10f;
    [SerializeField] private float _castTimeLimit = 1f;
    [SerializeField] private ParticleSystem _hideEffect;
    #endregion


    #region Properties
    public override int Level
    {
        set
        {
            base.Level = value;
            _castTime = Mathf.Clamp(_initialCastTime - Level, _castTimeLimit, _initialCastTime);       
        }
    }
    #endregion


    #region Methods
    protected override void OnUse() 
    {
        if (isServer) 
        {
            _unit.RemoveFocus();
            _unit.HasInteract = false;
        } 
        else
        {
            _hideEffect.Play();
        }
        base.OnUse();
    }

    protected override void OnCastComplete() 
    {
        if (isServer) 
        {
            _unit.HasInteract = true;
        } 
        else
        {
            _hideEffect.Stop();
        }
        base.OnCastComplete();
    }
    #endregion
}
