using UnityEngine.Networking;


public class HashMessage : MessageBase
{
    #region Fields
    public NetworkHash128 Hash;
    #endregion


    #region Class LifeCycle
    public HashMessage()
    {
    }
    public HashMessage(NetworkHash128 hash)
    {
        Hash = hash;
    }
    #endregion


    #region Methods
    public override void Deserialize(NetworkReader reader)
    {
        Hash = reader.ReadNetworkHash128();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(Hash);
    }
    #endregion
}