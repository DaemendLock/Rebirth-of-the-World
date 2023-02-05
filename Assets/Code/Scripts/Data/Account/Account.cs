using AccountsData;
using Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CurrencyType : byte {
    GOLD = 0,
    GUILD_TOKENS = 1
}

[CreateAssetMenu(fileName = "TestAccount", menuName = "New Test Account", order = 53)]
sealed public class Account : ScriptableObject {
    public int UID = 0;
    public AccountData data;

    private bool serverUpdated = false;

    public Account() {
        data = new AccountData();
    }

    public Account(int id) {
        data = new AccountData();
        UID = id;
    }

    public static Account RequestServerAccount() {
        Account account = new Account();
        
        return account;
    }

    public string GetName() {
        return name;
    }
}
[Serializable]
public class AccountData {

    public string Nickname = "";
    
    public ushort iconId = 0;
    public ushort backgroudId = 0;

    public ushort titleId;
    public byte Lvl;

    public Currencies Currencies = new Currencies();

    public List<LobbyQuest> ActiveQuests = new List<LobbyQuest>();
    public bool[] ComplitedQuests = new bool[1];
    public List<ushort> ComplitedScenario = new List<ushort>();
    public Dictionary<ushort, int> Inventory = new Dictionary<ushort, int>();
    public List<UnitPreview> OwnerCharacters = new List<UnitPreview>();

    public void HandleData(ResponsableRequest request) {
        switch ((DataType) request.Data[6]) {
            case DataType.ACCOUNT_CUSTOMIZATION:
                switch (request.Data[7]) {
                    case 0:
                        byte len = request.Response.Data[2];
                        Nickname = System.Text.Encoding.Unicode.GetString(new ArraySegment<byte>(request.Response.Data, 3, len).ToArray());
                        Lvl = request.Response.Data[len + 3];
                        break;
                }
                break;
        }
    }

    
    

}

[Serializable]
public class Currencies {
    public Dictionary<CurrencyType, int> currencies = new() {
        [CurrencyType.GOLD] = 99999,
        [CurrencyType.GUILD_TOKENS] = 99999
    };

}

class ItemData {
    public readonly ushort id;
    public readonly ushort ammount;
}

