using System;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(UnitStats))]
public class Combat : NetworkBehaviour 
{
    #region Fields
    public float AttackDistance = 0f;
    [SyncEvent] public event Action EventOnAttack;
    #endregion


    #region  Private Data
    [SerializeField] private  float attackSpeed = 1f;
    
    private UnitStats myStats;
    private float attackCooldown = 0f;
    #endregion


    #region  Unity Methods
    private void Start() 
    {
        myStats = GetComponent<UnitStats>();
	}

    private void Update() 
    {
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
    }
    #endregion


    #region  Methods
    public bool Attack(UnitStats targetStats) 
    {
        if (attackCooldown <= 0) 
        {
            targetStats.TakeDamage(myStats.Damage.GetValue());
            EventOnAttack();
            attackCooldown = 1f / attackSpeed;
            return true;
        }
        return false;
    }
    #endregion
}
