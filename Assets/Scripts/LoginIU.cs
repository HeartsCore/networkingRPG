using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LoginIU : MonoBehaviour 
{
    #region Private Data
    [SerializeField] private GameObject curPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private InputField loginLogin;
    [SerializeField] private InputField loginPass;
    [SerializeField] private InputField registerLogin;
    [SerializeField] private InputField registerPass;
    [SerializeField] private InputField registerConfirm;

    private MyNetworkManager _mgr;
    #endregion


    #region Unity Methods
    private void Start () 
    {
        _mgr = NetworkManager.singleton as MyNetworkManager;
        if (_mgr.ServerMode) {
            loginPanel.SetActive(false);
        } else {
            _mgr.LoginResponse = LoginResponse;
            _mgr.RegisterResponse = RegisterResponse;
        }
    }
    #endregion


    #region Methods
    void ClearInputs() 
    {
        loginLogin.text = "";
        loginPass.text = "";
        registerLogin.text = "";
        registerPass.text = "";
        registerConfirm.text = "";
    }

    public void Login() 
    {
        _mgr.Login(loginLogin.text, loginPass.text);
        curPanel.SetActive(false);
        loadingPanel.SetActive(true);
    }

    public void Register() 
    {
        if (registerPass.text != "" && registerPass.text == registerConfirm.text) 
        {
            _mgr.Register(registerLogin.text, registerPass.text);
            curPanel.SetActive(false);
            loadingPanel.SetActive(true);
        } 
        else 
        {
            Debug.Log("Error: Password Incorrect");
            ClearInputs();
        }
    }

    public void LoginResponse(string response) 
    {
        switch (response) 
        {
            case "UserError": Debug.Log("Error: Username not Found"); break;
            case "PassError": Debug.Log("Error: Password Incorrect"); break;
            default: Debug.Log("Error: Unknown Error. Please try again later."); break;
        }
        loadingPanel.SetActive(false);
        curPanel.SetActive(true);
        ClearInputs();
    }

    public void RegisterResponse(string response) 
    {
        switch (response) 
        {
            case "Success": Debug.Log("User registered"); break;
            case "UserError": Debug.Log("Error: Username Already Taken"); break;
            default: Debug.Log("Error: Unknown Error. Please try again later."); break;
        }
        loadingPanel.SetActive(false);
        curPanel.SetActive(true);
        ClearInputs();
    }

    public void SetPanel(GameObject panel) 
    {
        curPanel.SetActive(false);
        curPanel = panel;
        curPanel.SetActive(true);
        ClearInputs();
    }
    #endregion
}
