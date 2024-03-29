﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrepareController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleGo;

    [SerializeField]
    private GameObject guideGo;

    [SerializeField]
    private AvatarController[] avatarCtrls;

    private PrepareState state = PrepareState.None;
    public PrepareState State => state;
    
	void Start () {
		Debug.Assert(titleGo != null);
        Debug.Assert(guideGo != null);
        Debug.Assert(avatarCtrls != null && avatarCtrls.Length > 0);

        GameController.Instance.prepareCtrl = this;
    }

    private void Update()
    {
        switch (state)
        {
            case PrepareState.Title:
                if (Input.anyKeyDown)
                {
                    SoundManager.Instance.Play("Effect_ControllerConfirm");
                    ChangeState(PrepareState.Guide);
                }
                break;
            case PrepareState.Guide:
                if (avatarCtrls.All(avatar => avatar.state == AvatarState.Ready))
                {
                    GameController.Instance.StartGame();
                }
                break;
        }
    }

    public void Init(PrepareState initState = PrepareState.Title)
    {
        titleGo.SetActive(initState == PrepareState.Title);
        guideGo.SetActive(initState == PrepareState.Guide);

        foreach (var avatar in avatarCtrls)
        {
            avatar.Reset();
        }

        ChangeState(initState);

        SoundManager.Instance.Stop("Music_MemoCute");
        SoundManager.Instance.Play("Music_HappyVictorious");
    }

    public void Exit()
    {
        //
    }

    public void ChangeState(PrepareState nextState)
    {
        switch (state)
        {
            case PrepareState.Title:
                titleGo.SetActive(false);
                break;
            case PrepareState.Guide:
                guideGo.SetActive(false);
                break;
        }

        state = nextState;

        switch (nextState)
        {
            case PrepareState.Title:
                titleGo.SetActive(true);
                break;
            case PrepareState.Guide:
                guideGo.SetActive(true);
                break;
        }
    }
}

public enum PrepareState
{
    None = 0,
    Title = 1,
    Guide = 2
}