using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
public class Enemy : Unit 
{
    #region Private Data
    [Header("Movement")]
    [SerializeField] private float _moveRadius = 10f;
    [SerializeField] private float _minMoveDelay = 4f;
    [SerializeField] private float _maxMoveDelay = 12f;
    private Vector3 _startPosition;
    private Vector3 _curDistanation;
    private float _changePosTime;

    [Header("Behavior")]
    [SerializeField] private bool _aggressive;
    [SerializeField] private  float _rewardExp;
    [SerializeField] private float _viewDistance = 5f;
    [SerializeField] private float _reviveDelay = 5f;
    //[SerializeField] private float _agroDistance = 5f;

    private float _reviveTime;
    private List<Character> _enemies = new List<Character>();
    private Collider[] _bufferColliders = new Collider[64];
    private int _targetColliders;
    #endregion


    #region Unity Methods
    private void Start () 
    {
        _startPosition = transform.position;
        _changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        _reviveTime = _reviveDelay;
    }

    private void Update() 
    {
        OnUpdate();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);
    }
    #endregion


    #region Mrthods
    protected override void OnDieUpdate()
    {
        base.OnDieUpdate();
        if (_reviveTime > 0) 
        {
            _reviveTime -= Time.deltaTime;
        } 
        else 
        {
            _reviveTime = _reviveDelay;
            Revive();
        }
    }

    protected override void OnLiveUpdate() 
    {
        base.OnLiveUpdate();
        if (_focus == null) 
        {
            // блуждание
            Wandering(Time.deltaTime);
            // поиск цели если монстр агресивный
            if (_aggressive) FindEnemy();
        } 
        else 
        {
            float distance = Vector3.Distance(_focus.InteractionTransform.position, transform.position);
            if (distance > _viewDistance || !_focus.HasInteract) 
            {
                // если цель далеко перестаём приследовать
                RemoveFocus();
            } 
            else if (distance <= _focus.Radius) 
            {
                // действие если цель взоне взаимодействия
                if (!_focus.Interact(gameObject)) RemoveFocus();
            }
        }
    }
    
    protected override void Revive() 
    {
        base.Revive();
        transform.position = _startPosition;
        if (isServer) 
        {
            _motor.MoveToPoint(_startPosition);
        }
    }

    protected override void Die() 
    {
        base.Die();
        if (isServer) 
        {
            for (int i = 0; i < _enemies.Count; i++) 
            {
                _enemies[i].Player.Progress.AddExp(_rewardExp / _enemies.Count);
            }
            _enemies.Clear();
        }
    }

    private void FindEnemy() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _viewDistance, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; i++) 
        {
            Interactable interactable = _bufferColliders[i].GetComponent<Interactable>();
            if (interactable != null && interactable.HasInteract) 
            {
                SetFocus(interactable);
                break;
            }
        }
    }

    private void Wandering(float deltaTime) 
    {
        _changePosTime -= deltaTime;
        if (_changePosTime <= 0) 
        {
            RandomMove();
            _changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        }
    }

    private void RandomMove() 
    {
        _curDistanation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(_moveRadius, 0, 0) + _startPosition;
        _motor.MoveToPoint(_curDistanation);
    }

    public override bool Interact(GameObject user) 
    {
        if (base.Interact(user))
        {
            SetFocus(user.GetComponent<Interactable>());
            return true;
        }
        return false;
    }

    protected override void DamageWithCombat(GameObject user) 
    {
        base.DamageWithCombat(user);
        Character character = user.GetComponent<Character>();
        if (character != null && !_enemies.Contains(character)) _enemies.Add(character); 
    }
    #endregion
}
