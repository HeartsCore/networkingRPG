using System;
using System.Collections.Generic;


[Serializable]
public struct SerializableUserObjectList
{
    public List<SerializableUserObject> Users;
}

[Serializable]
public struct SerializableUserObject
{
    #region Fields
    public string UserName;
    public string Password;
    public string Data;
    #endregion


    #region Methods
    public void SetData (string newData)
    {
        Data = newData;
    }
    public override string ToString()
    {
        return $"UserName = {UserName}; Password = {Password}; Data = {Data};";
    }
    #endregion
}
