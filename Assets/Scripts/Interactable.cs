using UnityEngine;
using UnityEngine.Networking;


public class Interactable : NetworkBehaviour
{
    #region Fields
    public Transform InteractionTransform;
    public float Radius = 2f;
    #endregion


    #region Private Data
    private bool _hasInteract = true;
    #endregion


    #region Properties
    public bool HasInteract
    {
        get { return _hasInteract; }
        set { _hasInteract = value; }
    }
    #endregion


    #region Unity Methods
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(InteractionTransform.position, Radius);
    }
    #endregion


    #region Methods
    public virtual bool Interact(GameObject user)
    {
        // override interaction
        return false;
    }
    #endregion
}
