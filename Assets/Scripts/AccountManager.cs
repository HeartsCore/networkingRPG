using System.Collections.Generic;
using UnityEngine.Networking;


public static class AccountManager
{
    #region Private Data
    static List<UserAccount> accounts = new List<UserAccount>();
    #endregion


    #region Methods
    public static bool AddAccount(UserAccount account)
    {
        if (accounts.Find(acc => acc.Login == account.Login) == null)
        {
            accounts.Add(account);
            return true;
        }
        return false;
    }

    public static void RemoveAccount(UserAccount account)
    {
        accounts.Remove(account);
    }

    public static UserAccount GetAccount(NetworkConnection conn)
    {
        return accounts.Find(acc => acc.Connection == conn);
    }
    #endregion
}
