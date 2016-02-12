﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 主人公の動作、動作だけ！
/// </summary>
public class PlayerController : MonoBehaviour
{

    #region para
    public static PlayerController _instance;
    Rigidbody playerRigidbody;
    PlayerState playerState;
    NavMeshAgent playerAgent;
    private Animator anim;
    private HashIDs hash;
    private float timerEnergy = 0f;
    private float timerHP = 0f;
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.3f;

    public float velocity = 5.5f;
    public float runSpeed = 5.5f;
    public float walkSpeed = 3f;

    #endregion
    #region start
    // Use this for initialization
    void Start()
    {
        _instance = this;
        playerState = PlayerState.GamePlayerState;
        playerRigidbody = GetComponent<Rigidbody>();
        playerAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hash = this.GetComponent<HashIDs>();
        anim.SetLayerWeight(1, 1f);

        //test
        playerState.PlayerStateChanged(PlayerStateChangeType.all);
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {

        if (playerState.PlayerAliveNow)
        {
            playerState.playerTransform = transform;
            timerEnergy += Time.deltaTime;
            timerHP += Time.deltaTime;
            if (timerEnergy > playerState.energyUP)
            {
                timerEnergy = 0f;
                EnergyUp();
            }
            if (timerHP > 1f)
            {
                timerHP = 0f;
                Health();
            }
            if (Input.GetButtonDown("Walk"))
            {
                WalkChange();
            }
        }
    }
    void FixedUpdate()
    {
        if (playerState.GetActionInfoNow() == PlayerState.PlayerAction.Free)
        {
            float h = Input.GetAxis("Horizontal") == 0f ? ETCInput.GetAxis("Horizontal") : Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical") == 0f ? ETCInput.GetAxis("Vertical") : Input.GetAxis("Vertical");
            MovementManagement(h, v);
        }
        else if (playerState.GetActionInfoNow() == PlayerState.PlayerAction.AutoMoving)
        {
            AutoMovementManagement();
        }
        else
        {
            anim.SetFloat(hash.speedFloat, 0f);
        }
    }
    #endregion
    #region 状態変更（歩く）
    /// <summary>
    /// 歩くと走るの切り替え　ｚボタン
    /// </summary>
    void WalkChange()
    {
        if (playerState.isWalk)
        {
            playerState.isWalk = false;
            velocity = runSpeed;
        }
        else
        {
            playerState.isWalk = true;
            velocity = walkSpeed;
        }

    }
    #endregion

    #region 移動操作
    void AutoMovementManagement()
    {
        anim.SetFloat(hash.speedFloat,7f);
        if (playerAgent.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(playerAgent.velocity);
        }
    }

    /// <summary>
    /// 主人公の移動
    /// </summary>
    /// <param name="horizontal">水平移動値</param>
    /// <param name="vertical">垂直移動値</param>
    /// <param name="attack">攻撃ボタン</param>
    void MovementManagement(float horizontal, float vertical)
    {
        //攻撃の時、位置固定
        //Debug.Log(anim.GetCurrentAnimatorStateInfo(1).IsName("Attack Layer.Empty"));
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack Layer.Empty"))
        {
            Vector3 nowVel = playerRigidbody.velocity;
            if (Mathf.Abs(horizontal) > 0.05f || Mathf.Abs(vertical) > 0.05f)
            {
                Rotating(horizontal, vertical);


                playerRigidbody.velocity = new Vector3(velocity * horizontal,nowVel.y, vertical * velocity);
                anim.SetFloat(hash.speedFloat, playerRigidbody.velocity.magnitude);
                transform.LookAt(new Vector3(horizontal, 0, vertical) + transform.position);

            }
            else
            {
                anim.SetFloat(hash.speedFloat, 0f);
                playerRigidbody.velocity = new Vector3(0, nowVel.y, 0);
            }
        }
        else
        {

            playerRigidbody.velocity = new Vector3(0f, 0f, 0f);
        }

    }
    /// <summary>
    /// 主人公移動中転向
    /// </summary>
    /// <param name="horizontal">水平移動値</param>
    /// <param name="vertical">垂直移動値</param>
    void Rotating(float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(playerRigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        playerRigidbody.MoveRotation(newRotation);
    }
    #endregion

    #region 自然回復
    void EnergyUp()
    {
        if (playerState.energy < playerState.energyMax)
        {
            playerState.energy += 1;
            playerState.PlayerStateChanged(PlayerStateChangeType.energy);
        }
    }
    void Health()
    {
        if (playerState.HP < playerState.HPMax)
        {
            if (playerState.HP + playerState.health > playerState.HPMax)
            {
                playerState.HP = playerState.HPMax;
            }
            else
            {
                playerState.HP += (int)playerState.health;
            }
            playerState.PlayerStateChanged(PlayerStateChangeType.HP);

        }
    }
    #endregion
    #region 被攻撃
    /// <summary>
    /// 被攻撃
    /// </summary>
    /// <param name="enemyATK"></param>
    /// <param name="ACC"></param>
    public void Hit(int enemyATK, int ACC)
    {
        //TODO:攻撃のダメージ算出
        anim.SetTrigger(hash.hitTrigger);
        int damege = enemyATK - playerState.DEF;
        if (damege <= 0)
        {
            damege = 1;
        }
        playerState.HP -= damege;
        if (playerState.HP <= 0)
        {
            playerState.HP = 0;
            die();
        }
        playerState.PlayerStateChanged(PlayerStateChangeType.HP);
    }
    void die()
    {
        playerState.PlayerAliveNow = false;
        anim.SetTrigger(hash.dieTrigger);
    }
    #endregion

}
