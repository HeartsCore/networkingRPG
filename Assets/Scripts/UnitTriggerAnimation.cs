using UnityEngine;


public class UnitTriggerAnimation : MonoBehaviour 
{
    #region Private Data
    [SerializeField] private Animator animator;
    [SerializeField] private Unit unit;
    [SerializeField] private Combat combat;
    #endregion


    #region Unity Methods
    private void Start() 
    {
        unit.EventOnDamage += Damage;
        unit.EventOnDie += Die;
        unit.EventOnRevive += Revive;
        combat.EventOnAttack += Attack;
    }
    #endregion


    #region Methods
    private void Damage() 
    {
        animator.SetTrigger("Damage");
    }

    private void Die() 
    {
        animator.SetTrigger("Die");
    }

    private void Revive() 
    {
        animator.ResetTrigger("Damage");
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Revive");
    }

    private void Attack() 
    {
        animator.SetTrigger("Attack");
    }
    #endregion
}
