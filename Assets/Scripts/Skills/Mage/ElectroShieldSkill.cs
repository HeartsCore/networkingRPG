using UnityEngine;


public class ElectroShieldSkill : UpgradeableSkill 
{
    #region Private data
    [SerializeField] private int _baseDamage = 25;
    [SerializeField] private int _damageByLevel = 5;
    [SerializeField] private float _radius = 2;
    private int _damage;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private ParticleSystem _electroEffect;

    private Collider[] _bufferColliders = new Collider[64];
    private int _targetColliders;
    #endregion


    #region Properties
    public override int Level
    {
        set
        {
            base.Level = value;
            _damage = _baseDamage + _damageByLevel * Level;
        }
    }
    #endregion


    #region Unity Methods
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
    #endregion


    #region Methods
    protected override void OnUse() 
    {
        if (isServer) 
        {
            _unit.Motor.StopFollowingTarget();
        }
        base.OnUse();
    }

    protected override void OnCastComplete() 
    {
        if (isServer) 
        {
            _targetColliders = Physics.OverlapSphereNonAlloc(transform.position, _radius,_bufferColliders, _enemyMask);
            for (int i = 0; i < _targetColliders; i++) 
            {
                Unit enemy = _bufferColliders[i].GetComponent<Unit>();
                if (enemy != null && enemy.HasInteract)
                {
                    enemy.TakeDamage(_unit.gameObject, _damage);
                }
            }
        } 
        else 
        {
            _electroEffect.Play();
        }
        base.OnCastComplete();
    }
    #endregion
}
