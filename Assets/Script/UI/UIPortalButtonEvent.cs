﻿using UnityEngine;
using System.Collections;

public class UIPortalButtonEvent : MonoBehaviour
{
    public int floorNumber;
    UISceneManager LoadingBar;

    // Use this for initialization
    void Awake()
    {
        LoadingBar = UISceneManager._instance;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnButtonClick()
    {

        if (floorNumber == -1)
        {
            LoadingBar.Show(Application.LoadLevelAsync(3));
        }
        if (floorNumber == -100)
        {
            LoadingBar.Show(Application.LoadLevelAsync(4));
        }
        GameController._instance.SetLastChangeSceneType(EntryType.Portal);
        Debug.Log("GO TO FLOOR " + floorNumber);
    }

}
