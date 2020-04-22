using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(StatsManager), typeof(PlayerProgress), typeof(NetworkIdentity))]
public class Player : MonoBehaviour 
{
    #region Private Data
    [SerializeField] private Character _character;
    [SerializeField] private PlayerProgress _progress;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Equipment _equipment;
    [SerializeField] StatsManager _statsManager;
    private NetworkConnection _connection;
    #endregion


    #region Properties
    public Character Character { get { return _character; } }
    public PlayerProgress Progress { get { return _progress; } }
    public Inventory Inventory { get { return _inventory; } }
    public Equipment Equipment { get { return _equipment; } }
    public NetworkConnection Connection //player vis
    { 
        get 
        {
            if (_connection == null)
            {
                _connection = GetComponent<NetworkIdentity>().connectionToClient;
            }
            return _connection;
        }
    }
    #endregion


    #region Methods
    public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer) //UIHealthMana uIHealthMana,
    {
        _progress = GetComponent<PlayerProgress>();
        _statsManager = GetComponent<StatsManager>();
        _character = character;
        _inventory = inventory;
        _equipment = equipment;
        
        _character.Player = this;
        _inventory.Player = this;
        _equipment.Player = this;
        _statsManager.Player = this;

        if (GetComponent<NetworkIdentity>().isServer)
        {
            UserAccount account = AccountManager.GetAccount(GetComponent<NetworkIdentity>().connectionToClient);
            _character.Stats.Load(account.Data);
            _character.UnitSkills.Load(account.Data);
            _progress.Load(account.Data);
            _inventory.Load(account.Data);
            _equipment.Load(account.Data);
            _character.Stats.manager = _statsManager;
            _progress.Manager = _statsManager;
        }

        if (isLocalPlayer) {
            InventoryUI.instance.SetInventory(_inventory);
            EquipmentUI.instance.SetEquipment(_equipment);
            StatsUI.instance.SetManager(_statsManager);
            SkillsPanel.Instance.SetSkills(character.UnitSkills);
            SkillTree.Instance.SetCharacter(character);
            SkillTree.Instance.SetManager(_statsManager);
            UIHealthMana.instance.SetManager(_statsManager);

            //PlayerChat playerChat = GetComponent<PlayerChat>();
            //if (playerChat != null)
            //{
            //    if (GlobalChatChannel.Instance != null)
            //    {
            //        playerChat.RegisterChannel(GlobalChatChannel.Instance);
            //    }
            //    ChatChannel localChannel = _character.GetComponent<ChatChannel>();
            //    if (localChannel != null)
            //    {
            //        playerChat.RegisterChannel(localChannel);
            //    }

            //    ChatUI.Instance.SetPlayerChat(playerChat);
            //}
        }
    }
    #endregion
}
