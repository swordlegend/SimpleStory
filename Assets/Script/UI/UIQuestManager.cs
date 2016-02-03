﻿using UnityEngine;
using System.Collections.Generic;


public class UIQuestManager : MonoBehaviour
{
    #region para
    public GameObject NPC;

    private Quest selectQuest;

    public GameObject prefabQuestButton;
    public GameObject prefabStepButton;

    private PlayerQuest playerQuest;


    private List<GameObject> questShowList;
    private List<GameObject> stepShowList;

    private NPCManager npcManager;
    private EnemyManager enemyManager;
    #region UI
    UIController mainControllerUI;

    GameObject containQuestGrid;
    GameObject containStepGrid;

    #endregion
    #endregion


    #region start
    // Use this for initialization
    void Start()
    {

        questShowList = new List<GameObject>();
        stepShowList = new List<GameObject>();
        playerQuest = PlayerState.GamePlayerState.GetPlayerQuest();

        npcManager = NPC.GetComponent<NPCManager>();
        enemyManager = GameObject.FindGameObjectWithTag(Tags.enemyManager).GetComponent<EnemyManager>();

        mainControllerUI = GameObject.FindGameObjectWithTag(Tags.UIRoot).GetComponent<UIController>();


        containQuestGrid = transform.Find("QuestBG").Find("Scroll View").Find("Items").gameObject;
        containStepGrid = transform.Find("StepBG").Find("Scroll View").Find("Items").gameObject;
        this.gameObject.SetActive(false);
    }
    #endregion
    #region UpdateQuest/UpdateStep
    GameObject goQuest;

    void UpdateQuest()
    {
        selectQuest = null;
        playerQuest = PlayerState.GamePlayerState.GetPlayerQuest();
        for (int i = 0; i < questShowList.Count; i++)
        {
            NGUITools.Destroy(questShowList[i]);
        }
        questShowList.Clear();

        foreach (KeyValuePair<int, Quest> item in playerQuest.GetAcceptQuestList())
        {
            if (selectQuest == null)
            {
                selectQuest = item.Value;
            }
            if (!item.Value.isOver)
            {
                goQuest = NGUITools.AddChild(containQuestGrid, prefabQuestButton);
                goQuest.transform.Find("Title").GetComponent<UILabel>().text = string.Format("Quest:{0}", item.Value.info.name);
                goQuest.transform.Find("Doing").GetComponent<UILabel>().text = string.Format("Doing : Step {0}", item.Value.stepNow);
                goQuest.name = item.Key.ToString();
                questShowList.Add(goQuest);
            }
        }

        containQuestGrid.GetComponent<UIGrid>().enabled = true;

        UpdateStep();
    }
    GameObject goStep;
    void UpdateStep()
    {
        for (int i = 0; i < stepShowList.Count; i++)
        {
            NGUITools.Destroy(stepShowList[i]);
        }
        stepShowList.Clear();

        if (selectQuest != null)
        {
            for (int i = 0; i < selectQuest.info.step.Count; i++)
            {
                goStep = NGUITools.AddChild(containStepGrid, prefabStepButton);
                switch (selectQuest.info.step[i].questType)
                {
                    case QuestType.findNPC:
                        goStep.transform.Find("Tween").Find("LabelDescription").GetComponent<UILabel>().text = string.Format("任務説明：\n {0} \n\n 任務進捗：\n {1}と話してください。", selectQuest.info.step[i].description, npcManager.GetNPCInfo(selectQuest.info.step[i].targetID).name);
                        break;
                    case QuestType.findItem:
                        goStep.transform.Find("Tween").Find("LabelDescription").GetComponent<UILabel>().text = string.Format("任務説明：\n {0} \n\n 任務進捗：\n {1}  :  {2}/{3}", selectQuest.info.step[i].description,ItemList.getItem(selectQuest.info.step[i].targetID).name,PlayerState.GamePlayerState.GetPlayerBag().GetItemCount(selectQuest.info.step[i].targetID), selectQuest.count);
                        break;
                    case QuestType.killEnemy:
                        goStep.transform.Find("Tween").Find("LabelDescription").GetComponent<UILabel>().text = string.Format("任務説明：\n {0} \n\n 任務進捗：\n {1} :  {2}/{3}", selectQuest.info.step[i].description, enemyManager.getEnemyName(selectQuest.info.step[i].targetID),selectQuest.count, selectQuest.info.step[i].count);
                        break;
                    default:
                        break;
                }
                goStep.transform.Find("Title").GetComponent<UILabel>().text = string.Format("Step : {0}", i + 1);
                goStep.name = i.ToString();
                stepShowList.Add(goStep);
            }
            containStepGrid.GetComponent<UITable>().repositionNow = true;
        }
    }
    #endregion
    #region 外部API
    public void SetSelectQuest(Quest quest)
    {
        this.selectQuest = quest;
    }
    #endregion
    #region UI Event
    public void OnCloseButtonClick()
    {

        gameObject.SetActive(false);
    }
    public void OnOpenButtonClick()
    {
        mainControllerUI.CloseAllWindows();
        UpdateQuest();
        gameObject.SetActive(true);
    }
    public void OnTollgleButtonClick()
    {
        if (!gameObject.activeSelf)
        {
            UpdateQuest();
            mainControllerUI.CloseAllWindows();
        }

        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void OnQuestButtonClick()
    {
        UpdateStep();
    }
    #endregion
}
