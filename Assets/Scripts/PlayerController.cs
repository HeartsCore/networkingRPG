using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


public class PlayerController : NetworkBehaviour 
{
    #region Private Data
    [SerializeField] private LayerMask _movementMask;

    private Character _character;
    private Camera _camera;
    #endregion


    #region Unity Methods
    private void Awake() 
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (_character != null && !EventSystem.current.IsPointerOverGameObject())
            {
                // при нажатии на правую кнопку мыши пересещаемся в указанную точку
                if (Input.GetMouseButtonDown(1))
                {
                    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, _movementMask))
                    {
                        CmdSetMovePoint(hit.point);
                    }
                }
                // при нажатии на левую кнопку мыши взаимодйствуем с объектами
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, ~(1 << LayerMask.NameToLayer("Player"))))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            CmdSetFocus(interactable.GetComponent<NetworkIdentity>());
                        }
                    }
                }
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    if (Input.GetButtonDown("Skill1"))
                    {
                        CmdUseSkill(0);
                    }

                    if (Input.GetButtonDown("Skill2"))
                    {
                        CmdUseSkill(1);
                    }

                    if (Input.GetButtonDown("Skill3"))
                    {
                        CmdUseSkill(2);
                    }
                }
            }
        }
    }
    #endregion


    #region Methods
    public void SetCharacter(Character character, bool isLocalPlayer) 
    {
        this._character = character;
        if (isLocalPlayer) _camera.GetComponent<CameraController>().Target = character.transform;
    }
    #endregion


    #region Network Methods
    [Command]
    public void CmdSetMovePoint(Vector3 point) 
    {
        _character.SetMovePoint(point);
    }

    [Command]
    public void CmdSetFocus(NetworkIdentity newFocus) 
    {
        _character.SetNewFocus(newFocus.GetComponent<Interactable>());
    }
    [Command]
    void CmdUseSkill(int skillNum)
    {
        if (!_character.UnitSkills.InCast)
        {
            _character.UseSkill(skillNum);
        }
    }

    private void OnDestroy()
    {
        if (_character != null) Destroy(_character.gameObject);
    }
    #endregion
}
