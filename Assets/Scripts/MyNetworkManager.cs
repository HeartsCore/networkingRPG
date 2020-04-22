using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;


public class MyNetworkManager : NetworkManager
{
    public Action<string> LoginResponse;
    public Action<string> RegisterResponse;
    public Action ServerRegisterHandler;
    public Action<NetworkClient> ClientRegisterHandler;

    public bool ServerMode;
    private UserDataRepository _repository;
    private void Start()
    {
        if (ServerMode)
        {
            StartServer();
            _repository = UserDataRepository.Instance;
            NetworkServer.UnregisterHandler(MsgType.Connect);
            NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnectCustom);
            NetworkServer.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.Login, OnServerLogin);
            NetworkServer.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.Register, OnServerRegister);
            if (ServerRegisterHandler != null)
            {
                ServerRegisterHandler.Invoke();
            }
        }

    }
    private void OnServerLogin(NetworkMessage netMsg)
    {
        StartCoroutine(LoginUser(netMsg));
    }

    private void OnServerRegister(NetworkMessage netMsg)
    {
        StartCoroutine(RegisterUser(netMsg));
    }

    private void OnClientLogin(NetworkMessage netMsg)
    {
        LoginResponse.Invoke(netMsg.reader.ReadString());
    }

    private void OnClientRegister(NetworkMessage netMsg)
    {
        RegisterResponse.Invoke(netMsg.reader.ReadString());
    }

    private void OnServerConnectCustom(NetworkMessage netMsg)
    {
        if (LogFilter.logDebug) 
        { 
            Debug.Log("NetworkManager:OnServerConnectCustom"); 
        }
        netMsg.conn.SetMaxDelay(maxDelay);
        OnServerConnect(netMsg.conn);
    }

    public void Login(string login, string pass)
    {
        ClientConnect();
        StartCoroutine(SendLogin(login, pass));
    }

    public void Register(string login, string pass)
    {
        ClientConnect();
        StartCoroutine(SendRegister(login, pass));
    }
    public void AccountEnter(UserAccount account)
    {
        account.Connection.Send(MsgType.Scene, new StringMessage(onlineScene));
    }
    private IEnumerator SendLogin(string login, string pass)
    {
        while (!client.isConnected)
        {
            yield return null;
        }
        Debug.Log("client login");
        client.connection.Send(MsgType.Highest + 1 + (short)NetMsgType.Login, new UserMessage(login, pass));
    }

    private IEnumerator SendRegister(string login, string pass)
    {
        while (!client.isConnected)
        {
            yield return null;
        }
        Debug.Log("client register");
        client.connection.Send(MsgType.Highest + 1 + (short)NetMsgType.Register, new UserMessage(login, pass));
    }
    private IEnumerator LoginUser(NetworkMessage netMsg)
    {
        UserAccount account = new UserAccount(netMsg.conn);
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        //IEnumerator e = DCF.Login(msg.login, msg.pass);
        IEnumerator e = account.LoginUser(msg.login, msg.pass);
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;

        if (response == "Success")
        {
            if (account.Data.CharacterHash.IsValid())
            {
                AccountEnter(account);
            }
            else
            {
                netMsg.conn.Send(MsgType.Highest + 1 + (short)NetMsgType.Login, new StringMessage("CharacterNotSelect"));
                netMsg.conn.Send(MsgType.Highest + 1 + (short)NetMsgType.SelectCharacter, new EmptyMessage());
            }
        }
        else
        {
            netMsg.conn.Send(MsgType.Highest + 1 + (short)NetMsgType.Login, new StringMessage(response));
        }
    }

    private IEnumerator RegisterUser(NetworkMessage netMsg)
    {
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        //IEnumerator e = DCF.RegisterUser(msg.login, msg.pass, "");
        IEnumerator e = _repository.RegisterUser(msg.login, msg.pass, ""); 
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;

        Debug.Log("server register done");
        netMsg.conn.Send(MsgType.Highest + 1 + (short)NetMsgType.Register, new StringMessage(response));
    }
    private void ClientConnect()
    {
        NetworkClient client = this.client;
        if (client == null)
        {
            client = StartClient();
            client.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.Login, OnClientLogin);
            client.RegisterHandler(MsgType.Highest + 1 + (short)NetMsgType.Register, OnClientRegister);
            if (ClientRegisterHandler != null)
            {
                ClientRegisterHandler.Invoke(client);
            }
        }
    }
}
