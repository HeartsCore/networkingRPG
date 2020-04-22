using System;
using UnityEngine;
using UnityEngine.Networking;


public class Unit : Interactable 
{
    #region Fields
    public UnitSkills UnitSkills;
    public event Action EventOnDamage;
    public event Action EventOnDie;
    public event Action EventOnRevive;
    #endregion


    #region Private data
    [SerializeField] protected UnitMotor _motor;
    [SerializeField] protected UnitStats _stats;
    
    protected Interactable _focus;
    
    protected bool _isDie;
    #endregion


    #region Properties
    public UnitStats Stats { get { return _stats; } }
    public UnitMotor Motor { get { return _motor; } }
    public Interactable Focus { get { return _focus; } }
    #endregion


    #region Unity Methods
    private void Update()
    {
        OnUpdate();
    }
    #endregion


    #region Network Methods
    public override void OnStartServer() 
    {
        _motor.SetMoveSpeed(_stats.MoveSpeed.GetValue());
        _stats.MoveSpeed.OnStatChanged += _motor.SetMoveSpeed;
    }
    [ClientRpc]
    void RpcRevive()
    {
        if (!isServer) Revive();
    }
    [ClientRpc]
    void RpcDie()
    {
        if (!isServer) Die();
    }
    #endregion


    #region Methods
    protected virtual void OnLiveUpdate() { }
    protected virtual void OnDieUpdate() { }

    protected void OnUpdate() 
    {
        if (isServer) 
        {
            if (!_isDie) 
            {
                if (_stats.CurHealth == 0) Die();
                else OnLiveUpdate();
            } 
            else 
            {
                OnDieUpdate();
            }
        }
    }

    public override bool Interact(GameObject user) 
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null) 
        {
            if (combat.Attack(_stats)) 
            {
                DamageWithCombat(user);
            }
            return true;
        }
        return base.Interact(user);
    }

    protected virtual void DamageWithCombat(GameObject user) 
    {
        EventOnDamage();
    }

    public virtual void SetFocus(Interactable newFocus) 
    {
        if (newFocus != _focus) 
        {
            _focus = newFocus;
            _motor.FollowTarget(newFocus);
        }
    }

    public virtual void RemoveFocus() 
    {
        _focus = null;
        _motor.StopFollowingTarget();
    }
    
    protected virtual void Die() 
    {
        _isDie = true;
        GetComponent<Collider>().enabled = false;
        EventOnDie();
        if (isServer) 
        {
            HasInteract = false;
            RemoveFocus();
            _motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }
    
    protected virtual void Revive() 
    {
        _isDie = false;
        GetComponent<Collider>().enabled = true;
        EventOnRevive();
        if (isServer) 
        {
            HasInteract = true;
            _stats.SetHealthRate(1);
            RpcRevive();
        }
    }
    public void TakeDamage(GameObject user, int damage)
    {
        _stats.TakeDamage(damage);
        DamageWithCombat(user);
    }
    public void UseSkill(int skillNum)
    {
        if (!_isDie && skillNum < UnitSkills.Count)
        {
            UnitSkills[skillNum].Use(this);
        }
    }
    #endregion
}
