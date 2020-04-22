using UnityEngine;


[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    #region Private Data
    [SerializeField] private float _reviveDelay = 5f;
    //[SerializeField] private GameObject _gfx;

    private Vector3 _startPosition;
    private Vector3 _respawnPosition;

    private float _reviveTime;
    #endregion


    #region Fields
    public Player Player;
    #endregion


    #region Properties
    public Vector3 RespawnPosition {get {return _respawnPosition;} set { _respawnPosition = value;} }
    new public PlayerStats Stats { get { return base._stats as PlayerStats; } }
    #endregion


    #region Unity Methods
    private void Start()
    {
        _startPosition = transform.position;
        _reviveTime = _reviveDelay;
        if (Stats.CurHealth == 0)
        {
            transform.position = _startPosition;
            if (isServer)
            {
                Stats.SetHealthRate(1);
                _motor.MoveToPoint(_startPosition);
            }
        }
    }

    private void Update()
    {
        OnUpdate();
    }
    #endregion


    #region Methods
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
        if (_focus != null)
        {
            if (!_focus.HasInteract)
            {
                // если с объектом нельзя больше работать снимаем фокус
                RemoveFocus();
            }
            else
            {
                float distance = Vector3.Distance(_focus.InteractionTransform.position, transform.position);
                if (distance <= _focus.Radius)
                {
                    // действие если цель в зоне взаимодействия
                    if (!_focus.Interact(gameObject)) RemoveFocus();
                }
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        //gfx.SetActive(false);
    }

    protected override void Revive()
    {
        transform.position = _respawnPosition;
        base.Revive();
        //transform.position = startPosition;
        //_gfx.SetActive(true);
        if (isServer)
        {
            SetMovePoint(_respawnPosition);
            //motor.MoveToPoint(startPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!_isDie)
        {
            RemoveFocus();
            _motor.MoveToPoint(point);
        }
    }

    public void SetNewFocus(Interactable newFocus)
    {
        if (!_isDie)
        {
            if (newFocus.HasInteract) SetFocus(newFocus);
        }
    }

    public void SetRespawnPosition(Vector3 newPosition)
    {
        _respawnPosition = newPosition;
    }
    #endregion
}
