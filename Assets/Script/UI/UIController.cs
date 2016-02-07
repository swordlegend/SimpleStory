﻿using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region para
    private PlayerState playerState;
    private PlayerController player;

    bool isToolbarShow;

    private Transform containerPlayerState;
    private Transform containerMap;
    private Transform containerToolBar;
    private Transform containerEquepMenu;
    private Transform spriteAllow;
    private Transform containerSkill;
    private Transform containerMove;

    private UISlider sliderEnergy;
    private UILabel labelEnergy;
    private UISlider sliderHP;
    private UILabel labelHP;
    private UILabel labelLevel;
    private UISlider sliderExp;
    private UIBagManager bagManagerUI;
    private UIQuestManager questManagerUI;
    private UINPCQuestManager npcQuestManagerUI;
    private UICommunicationManager communicationPanelUI;
    private UISkillManager skillManagerUI;
    #endregion
    #region Start/Update
    // Use this for initialization
    void Start()
    {


        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerController>();
        playerState = PlayerState.GamePlayerState;

        isToolbarShow = false;

        containerToolBar = transform.Find("ToolBar");
        containerPlayerState = transform.Find("PlayerState");
        containerMap = transform.Find("GameMap");
        containerEquepMenu = transform.Find("EquepMenu");
        containerSkill = transform.Find("SkillContainer");
        containerMove = transform.Find("MoveContainer");
        spriteAllow = containerMap.Find("miniMap/spriteAllow");
        sliderEnergy = containerPlayerState.Find("sliderEnergy").GetComponent<UISlider>();
        labelEnergy = sliderEnergy.gameObject.transform.Find("labelEnergy").GetComponent<UILabel>();
        sliderHP = containerPlayerState.Find("sliderHP").GetComponent<UISlider>();
        sliderExp = containerToolBar.Find("ExpBar").GetComponent<UISlider>();
        labelHP = sliderHP.transform.Find("labelHP").GetComponent<UILabel>();
        labelLevel = containerPlayerState.Find("labelLevel").GetComponent<UILabel>();
        playerState.OnPlayerStateChanged += OnStateChanged;

        bagManagerUI = containerEquepMenu.GetComponent<UIBagManager>();
        questManagerUI = transform.Find("QuestMenu").GetComponent<UIQuestManager>();
        npcQuestManagerUI = transform.Find("NPCQuestPanel").GetComponent<UINPCQuestManager>();
        communicationPanelUI = transform.Find("CommunicationPanel").GetComponent<UICommunicationManager>();
        skillManagerUI = transform.Find("SkillPanel").GetComponent<UISkillManager>();

        ToolBarInit();
    }

    // Update is called once per frame
    void Update()
    {
        AllowRotion();
        if (Input.GetButtonDown("Map"))
        {
            MapSwitch();
        }
    }
    #endregion
    #region miniMap
    void MapSwitch()
    {
        NGUITools.SetActive(containerMap.gameObject, !containerMap.gameObject.activeSelf);
    }
    void AllowRotion()
    {
        const float miniMapScaleRatio = 0 / 2000f;
        spriteAllow.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.y);
        spriteAllow.localPosition = new Vector3()
        {
            x = -player.transform.position.x * miniMapScaleRatio,
            y = -player.transform.position.z * miniMapScaleRatio,
            z = 0,
        };

    }
    #endregion
    #region delegate/UI update
    void OnStateChanged(PlayerStateChangeType type)
    {
        switch (type)
        {
            case PlayerStateChangeType.all:
                {
                    OnEnergyChanged();
                    OnHPChanged();
                    OnLevelChange();
                    OnExpChanged();
                    break;
                }
            case PlayerStateChangeType.energy:
                {
                    OnEnergyChanged();
                    break;
                }
            case PlayerStateChangeType.HP:
                {
                    OnHPChanged();
                    break;
                }
            case PlayerStateChangeType.LEVEL:
                {
                    OnLevelChange();
                    break;
                }
            case PlayerStateChangeType.EXP:
                {
                    OnExpChanged();
                    break;
                }
            case PlayerStateChangeType.bag:
                {
                    break;
                }
            case PlayerStateChangeType.equep:
                {
                    break;
                }
            case PlayerStateChangeType.STATE:
                {
                    break;
                }
        }

    }

    void OnHPChanged()
    {
        sliderHP.value = playerState.HP / (float)playerState.HPMax;
        labelHP.text = string.Format("{0}/{1}", playerState.HP, playerState.HPMax);
    }

    void OnEnergyChanged()
    {
        sliderEnergy.value = playerState.energy / 100f;
        labelEnergy.text =
            string.Format("{0}/{1}", playerState.energy, playerState.energyMax);
    }
    void OnLevelChange()
    {
        labelLevel.text = playerState.level.ToString();
    }
    void OnExpChanged()
    {

        sliderExp.value = (float)(playerState.EXP - 50 * (playerState.level * (playerState.level - 1)) / 2) / (playerState.level * 50);
    }

    #endregion
    #region Toolbar

    private void ToolBarInit()
    {
    }
    public void ToolBarToggle()
    {
        if(isToolbarShow)
        {
            containerToolBar.GetComponent<UITweener>().PlayForward();
            containerSkill.GetComponent<UITweener>().PlayForward();
            containerMove.GetComponent<UITweener>().PlayForward();
        }
        else
        {
            containerToolBar.GetComponent<UITweener>().PlayReverse();
            containerSkill.GetComponent<UITweener>().PlayReverse();
            containerMove.GetComponent<UITweener>().PlayReverse();


        }
        isToolbarShow = !isToolbarShow;

    }
    
    public void OnSkillButtonClicked()
    {
        skillManagerUI.OnToggleButtonClick();
    }
    public void OnBagButtonClicked()
    {
        bagManagerUI.OpenPanelEquep();
    }
    public void OnQuestButtonClicked()
    {
        questManagerUI.OnTollgleButtonClick();
    }

    #endregion
    #region 外部API
    /// <summary>
    /// 全部のウィンドウを閉じる
    /// </summary>
    public void CloseAllWindows()
    {
        bagManagerUI.OnCloseButtonClick();
        questManagerUI.OnCloseButtonClick();
        npcQuestManagerUI.OnCloseButtonClick();
        communicationPanelUI.OnCloseButtonClick();
        skillManagerUI.OnCloseButtonClick();
    }
    #endregion
}
