using UnityEngine;
using UnityEngine.Networking;


public class CharacterSelect : MonoBehaviour
{
    #region Singleton
    public static CharacterSelect Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        _manager.ServerRegisterHandler += RegisterServerHandler;
        _manager.ClientRegisterHandler += RegisterClientHandler;
    }
    #endregion


    #region Private Data
    [SerializeField] private MyNetworkManager _manager;
    #endregion


    #region Methods
    private void RegisterServerHandler()
    {
        NetworkServer.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnSelectCharacter);
    }

    private void RegisterClientHandler(NetworkClient client)
    {
        client.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, OnOpenSelectUI);
    }
    private void OnSelectCharacter(NetworkMessage netMsg)
    {
        NetworkHash128 hash = netMsg.reader.ReadNetworkHash128();
        if (hash.IsValid())
        {
            UserAccount account = AccountManager.GetAccount(netMsg.conn);
            account.Data.CharacterHash = hash;
            _manager.AccountEnter(account);
        }
    }

    private void OnOpenSelectUI(NetworkMessage netMsg)
    {
       CharacterSelectUI.Instance.OpenPanel();
    }

    public void SelectCharacter(NetworkHash128 characterHash)
    {
        if (characterHash.IsValid())
        {
            _manager.client.Send(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, new HashMessage(characterHash));
        }
    }
    #endregion
}
