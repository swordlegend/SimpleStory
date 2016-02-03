﻿using UnityEngine;
using System.Collections.Generic;

public enum CommunicationType
{
    Talk,Quest, Shop
}

public class NPCManager : MonoBehaviour {

    #region para
    private Dictionary<int, GameObject> NPCDictionary;
    #endregion
    #region test data
    public GameObject NPC1;
    public GameObject NPC2;
    #endregion

    #region Start
    // Use this for initialization
    void Start () {
        NPCDictionary = new Dictionary<int, GameObject>();

        Init();
    }
    #endregion
    #region 初期化
    void Init()
    {
        LoadData();
    }

    void LoadData()
    {
        NPC1.GetComponent<NPCInfomation>().SetNPCType(new Dictionary<CommunicationType, bool>() { { CommunicationType.Talk,true }, { CommunicationType.Shop,true }, { CommunicationType .Quest,true}  });
        NPC1.GetComponent<NPCInfomation>().SetQuest(new List<QuestInfo>() { QuestList.getQuest(1), QuestList.getQuest(2), QuestList.getQuest(3), QuestList.getQuest(4), QuestList.getQuest(5) });
        NPCDictionary.Add(NPC1.GetComponent<NPCInfomation>().NPCID, NPC1);

            
        NPC2.GetComponent<NPCInfomation>().SetNPCType(new Dictionary<CommunicationType, bool>() { { CommunicationType.Talk, true }, { CommunicationType.Shop, true }, { CommunicationType.Quest, true } });
        NPC2.GetComponent<NPCInfomation>().SetQuest(new List<QuestInfo>() { QuestList.getQuest(4) });

        NPCDictionary.Add(NPC2.GetComponent<NPCInfomation>().NPCID, NPC2);
    }
    #endregion

    #region 外部API
    public Dictionary<int, GameObject> GetNPCDctionary()
    {
        return NPCDictionary;
    }
    public NPCInfomation GetNPCInfo(int NPCID)
    {
        GameObject NPC;
        NPCDictionary.TryGetValue(NPCID, out NPC);
        return NPC.GetComponent<NPCInfomation>();
    }
    #endregion
}
