using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour 
{
    #region Private Data
    private NavMeshAgent _agent;
    private Transform _target;
    #endregion


    #region Unity Methods
    private void Awake () 
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update() 
    {
        if (_target != null) 
        {
            if (_agent.velocity.magnitude == 0) FaceTarget();
            _agent.SetDestination(_target.position);
        }
    }
    #endregion


    #region Methods

    public void MoveToPoint(Vector3 point) 
    {
        _agent.SetDestination(point);
    }

    private void FaceTarget() 
    {
        Vector3 direction = _target.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void FollowTarget(Interactable newTarget) 
    {
        _agent.stoppingDistance = newTarget.Radius;
        _target = newTarget.InteractionTransform;
    }

    public void StopFollowingTarget() 
    {
        _agent.stoppingDistance = 0f;
        _agent.ResetPath();
        _target = null;
    }

    public void SetMoveSpeed(int speed) 
    {
        _agent.speed = speed;
    }
    #endregion
}
