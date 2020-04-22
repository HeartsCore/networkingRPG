using UnityEngine.Networking;


public class UserMessage : MessageBase
{
    #region Fields
    public string login;
    public string pass;
    #endregion


    #region Class LifeCycle
    public UserMessage()
    {
    }

    public UserMessage(string login, string pass)
    {
        this.login = login;
        this.pass = pass;
    }
    #endregion


    #region Methods
    public override void Deserialize(NetworkReader reader)
    {
        login = reader.ReadString();
        pass = reader.ReadString();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(login);
        writer.Write(pass);
    }
    #endregion
}
