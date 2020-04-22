using UnityEngine;
using UnityEngine.AI;


public class UnitAnimation : MonoBehaviour 
{
    #region Private Data
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    private static readonly int _moving = Animator.StringToHash("Move");
    #endregion


    #region Unity Methods
    private void FixedUpdate() 
    {
        if (agent.velocity.magnitude == 0) 
        {
            animator.SetBool(_moving, false);
        } 
        else 
        {
            animator.SetBool(_moving, true);
        }
    }
    #endregion


    #region Methods

    //Placeholder functions for Animation events
    private void Hit() {
    }

    private void FootR() {
    }

    private void FootL() {
    }
    #endregion
}
