using UnityEngine;
using UnityEngine.Networking;


public class NetUnitSetup : NetworkBehaviour 
{
    #region Private Data
    [SerializeField] private MonoBehaviour[] _disableBehaviours;
    #endregion


    #region Unity Methods
    private void Awake() 
    {
        for (int i = 0; i < _disableBehaviours.Length; i++) {
            _disableBehaviours[i].enabled = false;
        }
	}
    #endregion


    #region Network Methods
    public override void OnStartServer() 
    {
        for (int i = 0; i < _disableBehaviours.Length; i++) {
            _disableBehaviours[i].enabled = true;
        }
    }
    #endregion
}
