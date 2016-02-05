﻿using UnityEngine;
using System.Collections;


/// <summary>
/// 主人公の数値、状態など　
/// </summary>
public enum PlayerStateChangeType
{
    EXP, LEVEL, STATE, HP, energy, money, all, bag, equep
}

public class PlayerState
{
    public enum PlayerAction
    {
        Free, Died, Shopping, Talking
    }
    //シングルトン
    private static PlayerState _instans;
    public static PlayerState GamePlayerState
    {
        get
        {
            if (_instans == null)
            {
                _instans = new PlayerState();
                return _instans;
            }
            else
            {
                return _instans;
            }
        }
        set
        {
            _instans = value;
        }
    }
    #region para
    public Transform playerTransform;
    private PlayerAction playerActionNow;
    public bool PlayerAliveNow
    {
        get
        {
            if (playerActionNow == PlayerAction.Free)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        set
        {
            if (value)
            {
                playerActionNow = PlayerAction.Free;
            }
            else
            {
                playerActionNow = PlayerAction.Died;
            }

        }
    }
    public bool isWalk;

    public int level;//レベル
    public int HP;//HP
    public int HPMax;//HP上限
    public int MP;//マナ
    public int energy;//体力
    public int energyMax;//体力上限
    public int money;//持ち金

    private float attackDis = 0;


    public int baseSTR;//力
    public int baseDEX;//素早さ
    public int baseINT;//知恵
    public int baseCON;//体力
    public int baseLUK;//運

    public int equepSTR;//力
    public int equepDEX;//素早さ
    public int equepINT;//知恵
    public int equepCON;//体力
    public int equepLUK;//運

    public int EXP;//経験値
    public int STR;//力
    public int DEX;//素早さ
    public int INT;//知恵
    public int CON;//体力
    public int LUK;//運


    public int ATK;//攻撃力
    public int DEF;//防御力
    public float energyUP;//体力回復、毎energyUP秒回復1点
    public float health;//HP回復、毎秒回復health点



    public delegate void OnPlayerStateChangedEvent(PlayerStateChangeType type);
    public event OnPlayerStateChangedEvent OnPlayerStateChanged;

    private PlayerBag bag;
    private PlayerEquep equep;
    private PlayerQuest quest;
    #endregion
    #region 初期化・データ更新
    private PlayerState()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;
        HP = 100;
        LoadData();
        Init();
    }
    //基礎statusを導入
    public void LoadData()
    {
        //TODO：　ファイルから数値を読み取る

        #region Tempデータ

        EXP = 740;
        baseSTR = 50;
        baseDEX = 50;
        baseINT = 50;
        baseCON = 50;
        baseLUK = 20;
        money = 10000;
        playerActionNow = PlayerAction.Free;
        #endregion
        this.bag = PlayerBag.nowPlayerBag;
        this.equep = PlayerEquep.nowPlayerEquep;
        this.quest = PlayerQuest.nowPlayerQuest;
        isWalk = false;
    }
    //status算出
    private void Init()
    {
        InitEquep();
        //TODO：　計算式はまだ完成していません

        level = (int)(0.5f + Mathf.Sqrt(EXP / 25f + 0.25f));

        STR = level * 3 + baseSTR + equepSTR;
        DEX = level * 2 + baseDEX + equepDEX;
        INT = level * 2 + baseINT + equepINT;
        CON = level * 3 + baseCON + equepCON;
        LUK = (int)(level * 0.25f + baseLUK + equepLUK);

        ATK = (int)(STR * 1.25f);
        DEF = (int)(DEX * 0.4f + CON * 0.4f);
        energyUP = 0.1f - DEX * 0.001f;
        energyMax = 100;
        energy = 100;
        HPMax = level * 100 + STR * 10 + CON * 25;
        HP = HPMax > HP ? HP : HPMax;
        health = 1 + level * 0.3f + CON * 0.2f;
        attackDis = 5;
    }
    /// <summary>
    /// 装備数値の初期化
    /// </summary>
    public void InitEquep()
    {
        equepSTR = 0;
        equepDEX = 0;
        equepINT = 0;
        equepCON = 0;
        equepLUK = 0;
        if (equep.dictionaryEquep[ItemType.head] != null)
        {
            equepSTR += equep.dictionaryEquep[ItemType.head].info.STR;
            equepDEX += equep.dictionaryEquep[ItemType.head].info.DEX;
            equepINT += equep.dictionaryEquep[ItemType.head].info.INT;
            equepCON += equep.dictionaryEquep[ItemType.head].info.CON;
            equepLUK += equep.dictionaryEquep[ItemType.head].info.LUK;
        }
        if (equep.dictionaryEquep[ItemType.necklace] != null)
        {
            equepSTR += equep.dictionaryEquep[ItemType.necklace].info.STR;
            equepDEX += equep.dictionaryEquep[ItemType.necklace].info.DEX;
            equepINT += equep.dictionaryEquep[ItemType.necklace].info.INT;
            equepCON += equep.dictionaryEquep[ItemType.necklace].info.CON;
            equepLUK += equep.dictionaryEquep[ItemType.necklace].info.LUK;
        }
        if (equep.dictionaryEquep[ItemType.body] != null)
        {
            equepSTR += equep.dictionaryEquep[ItemType.body].info.STR;
            equepDEX += equep.dictionaryEquep[ItemType.body].info.DEX;
            equepINT += equep.dictionaryEquep[ItemType.body].info.INT;
            equepCON += equep.dictionaryEquep[ItemType.body].info.CON;
            equepLUK += equep.dictionaryEquep[ItemType.body].info.LUK;
        }
        if (equep.dictionaryEquep[ItemType.foot] != null)
        {
            equepSTR += equep.dictionaryEquep[ItemType.foot].info.STR;
            equepDEX += equep.dictionaryEquep[ItemType.foot].info.DEX;
            equepINT += equep.dictionaryEquep[ItemType.foot].info.INT;
            equepCON += equep.dictionaryEquep[ItemType.foot].info.CON;
            equepLUK += equep.dictionaryEquep[ItemType.foot].info.LUK;
        }
        if (equep.dictionaryEquep[ItemType.bracelet] != null)
        {
            equepSTR += equep.dictionaryEquep[ItemType.bracelet].info.STR;
            equepDEX += equep.dictionaryEquep[ItemType.bracelet].info.DEX;
            equepINT += equep.dictionaryEquep[ItemType.bracelet].info.INT;
            equepCON += equep.dictionaryEquep[ItemType.bracelet].info.CON;
            equepLUK += equep.dictionaryEquep[ItemType.bracelet].info.LUK;
        }
        if (equep.dictionaryEquep[ItemType.ring] != null)
        {
            equepSTR += equep.dictionaryEquep[ItemType.ring].info.STR;
            equepDEX += equep.dictionaryEquep[ItemType.ring].info.DEX;
            equepINT += equep.dictionaryEquep[ItemType.ring].info.INT;
            equepCON += equep.dictionaryEquep[ItemType.ring].info.CON;
            equepLUK += equep.dictionaryEquep[ItemType.ring].info.LUK;
        }
        if (equep.dictionaryEquep[ItemType.wing] != null)
        {
            equepSTR += equep.dictionaryEquep[ItemType.wing].info.STR;
            equepDEX += equep.dictionaryEquep[ItemType.wing].info.DEX;
            equepINT += equep.dictionaryEquep[ItemType.wing].info.INT;
            equepCON += equep.dictionaryEquep[ItemType.wing].info.CON;
            equepLUK += equep.dictionaryEquep[ItemType.wing].info.LUK;
        }
        if (equep.dictionaryEquep[ItemType.weapon] != null)
        {
            equepSTR = equep.dictionaryEquep[ItemType.weapon].info.STR;
            equepDEX = equep.dictionaryEquep[ItemType.weapon].info.DEX;
            equepINT = equep.dictionaryEquep[ItemType.weapon].info.INT;
            equepCON = equep.dictionaryEquep[ItemType.weapon].info.CON;
            equepLUK = equep.dictionaryEquep[ItemType.weapon].info.LUK;
        }

    }
    #endregion
    #region delegate
    /// <summary>
    /// Playerの数値変更する時
    /// </summary>
    /// <param name="type"></param>
    public void PlayerStateChanged(PlayerStateChangeType type)
    {
        OnPlayerStateChanged(type);
    }
    #endregion
    #region 外部API
    public float GetAttackDis()
    {
        return attackDis;
    }
    public void KillEnemy(int enemyID, int EXP, int level)
    {
        ExpUp(EXP);
        DoQuest(QuestType.killEnemy, enemyID);
    }
    private void ExpUp(int exp)
    {
        EXP += exp;
        if (EXP > (level * 50))
        {
            level++;
            Init();
            PlayerStateChanged(PlayerStateChangeType.LEVEL);
            PlayerStateChanged(PlayerStateChangeType.STATE);
        }
        PlayerStateChanged(PlayerStateChangeType.EXP);
    }
    public PlayerBag GetPlayerBag()
    {
        return bag;
    }
    public PlayerEquep GetPlayerEquep()
    {
        return equep;
    }
    public PlayerQuest GetPlayerQuest()
    {
        return quest;
    }
    public PlayerAction GetActionInfoNow()
    {
        return playerActionNow;
    }
    public void ChangeAction(PlayerAction action)
    {
        this.playerActionNow = action;
    }
    #endregion

    #region アイテム関連
    public void GetMoney(int money)
    {
        this.money += money;
        PlayerStateChanged(PlayerStateChangeType.money);
    }

    /// <summary>
    /// アイテムget
    /// </summary>
    /// <param name="itemID">アイテムid</param>
    public void GetItem(int itemID)
    {
        bag.AddItem(itemID);
        PlayerStateChanged(PlayerStateChangeType.bag);
    }
    /// <summary>
    /// アイテム購入
    /// </summary>
    /// <param name="itemID"></param>
    public bool BuyItem(int itemID)
    {
        if (ItemList.getItem(itemID).money <= money)
        {
            money -= ItemList.getItem(itemID).money;
            bag.AddItem(itemID);
            PlayerStateChanged(PlayerStateChangeType.bag);
            PlayerStateChanged(PlayerStateChangeType.money);
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// アイテム売る
    /// </summary>
    /// <param name="itemBagID"></param>
    public void SellItem(int itemBagID)
    {
        if (bag.isEqueped(itemBagID))
        {
            SetdownEquep(itemBagID);
        }
        this.money += bag.dictionBag[itemBagID].info.money;
        bag.DeleteItem(itemBagID);
        PlayerStateChanged(PlayerStateChangeType.bag);
        PlayerStateChanged(PlayerStateChangeType.money);
    }
    /// <summary>
    /// アイテム使用
    /// </summary>
    /// <param name="itemBagID"></param>
    public void UseItem(int itemBagID)
    {
        HP += bag.dictionBag[itemBagID].info.HP;
        energy += bag.dictionBag[itemBagID].info.energy;
        bag.DeleteItem(itemBagID);
        PlayerStateChanged(PlayerStateChangeType.bag);
    }
    /// <summary>
    /// 装備交換
    /// </summary>
    /// <param name="id">Bag中のid</param>
    public void ChangeEquep(int id)
    {
        bag.ChangeEquep(id);
        Init();
        PlayerStateChanged(PlayerStateChangeType.equep);
        PlayerStateChanged(PlayerStateChangeType.STATE);
    }
    /// <summary>
    /// 装備撤下
    /// </summary>
    /// <param name="id">Bag中のid</param>
    public void SetdownEquep(int id)
    {
        bag.SetdownEquep(id);
        Init();
        PlayerStateChanged(PlayerStateChangeType.equep);
        PlayerStateChanged(PlayerStateChangeType.STATE);
    }
    #endregion

    #region クエスト関連
    public bool AcceptQuest(int questID)
    {
        return quest.AcceptQuest(questID);
    }
    public void CancelQuest()
    {

    }
    public void DoQuest(QuestType type, int ID)
    {
        quest.DoQuest(type, ID);
    }
    public bool OverQuest(int questID)
    {
        QuestAward(quest.GetAcceptQuestList()[questID].GetStepNow().award);
        return quest.OverQuest(questID);
    }
    public bool CanReport(int questID)
    {
        if (quest.GetAcceptQuestList()[questID].GetStepNow().questType == QuestType.findItem)
        {
            quest.GetAcceptQuestList()[questID].count = bag.GetItemCount(questID);
        }
        return quest.IsOverStep(questID);
    }
    public void QuestAward(QuestInfo.Award award)
    {
        if (award.itemID >= 0)
        {
            GetItem(award.itemID);
        }
        GetMoney(award.money);
        ExpUp(award.EXP);
    }

    #endregion
    #region Skill関連
    public bool SkillUp(int money)
    {
        if (this.money > money)
        {
            this.money -= money;
            return true;
        }
        else
        {
            return false;
        }

    }

    #endregion 
}
