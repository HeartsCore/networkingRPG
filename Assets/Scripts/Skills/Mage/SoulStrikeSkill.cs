using UnityEngine;


public class SoulStrikeSkill : UpgradeableSkill 
{
    #region Private data
    [SerializeField] private float _baseRange = 7f;
    [SerializeField] private float _upgradedRange = 10f;
    [SerializeField] private int _levelToUpgrade = 3;
    [SerializeField] private int _baseDamage = 25;
    [SerializeField] private int _damagePerLevel = 5;
    private float _range;
    private int _damage;
    [SerializeField] private ParticleSystem _castEffect;
    [SerializeField] private ParticleSystem _soulStrikeEffect;
    #endregion


    #region Properties
    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = _baseDamage + _damagePerLevel * Level;
            _range = Level < _levelToUpgrade ? _baseRange : _upgradedRange;
        }
    }
    #endregion


    #region Unity Methods
    protected override void Start() 
    {
        base.Start();
        _soulStrikeEffect.transform.SetParent(null);
    }
    private void OnDestroy()
    {
        if (isServer)
        {
            Destroy(_soulStrikeEffect.gameObject);
        }
    }
    #endregion


    #region Methods
    protected override void OnUse() 
    {
        if (isServer) 
        {
            if (_target != null && _target.GetComponent<Unit>() != null) 
            {
                if (Vector3.Distance(_target.transform.position, _unit.transform.position) <= _range) 
                {
                    _unit.RemoveFocus();
                    base.OnUse();
                }
            }
        } 
        else 
        {
            _castEffect.Play();
            base.OnUse();
        }
    }

    protected override void OnCastComplete() 
    {
        Unit enemy = _target.GetComponent<Unit>();
        if (isServer) 
        {
            if (enemy.HasInteract) 
            {
                enemy.TakeDamage(_unit.gameObject, _damage);
                _unit.SetFocus(enemy);
            }
        } 
        else 
        {
            _castEffect.Stop();
            _soulStrikeEffect.transform.position = enemy.transform.position;
            _soulStrikeEffect.transform.rotation = Quaternion.LookRotation(enemy.transform.position - _unit.transform.position);
            _soulStrikeEffect.Play();
        }
        base.OnCastComplete();
    }
    #endregion
}