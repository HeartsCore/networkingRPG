using UnityEngine;
using UnityEngine.Networking;


public class MenuUI : MonoBehaviour 
{
    #region Private Data
    [SerializeField] private GameObject _menuPanel;
    #endregion


    #region Unity Methods
    private void Start()
    {
        if ((NetworkManager.singleton as MyNetworkManager).ServerMode) _menuPanel.SetActive(false);
    }
    #endregion


    #region Methods
    public void Disconnect() 
    {
        if (NetworkManager.singleton.IsClientConnected()) 
        {
            NetworkManager.singleton.StopClient();
        }
    }
    #endregion
}
