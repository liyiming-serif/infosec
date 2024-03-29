﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ChallengeFour : CodingChallengeTemplate
{

    [SerializeField]
    protected GameObject distrustFeedback;
    [SerializeField]
    protected Outbox distrustOutbox;
    [SerializeField]
    protected MemoryBar memoryBar;

    /*Execution Record*/
    protected List<string[]> distrustOutboxLog;
    protected List<string[]> memoryBarLog;

    override protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode)
    {
        if (subCode == SubCommand.Code.Boss)
        {
            character.SetEndPosition(playerOutbox.playerPos);
        }
        else if (subCode == SubCommand.Code.Distrust)
        {
            character.SetEndPosition(distrustOutbox.playerPos);
        }
        else if (subCode == SubCommand.Code.Zero)
        {
            character.SetEndPosition(memoryBar.getPickupPos(0));
        }
        else if (subCode == SubCommand.Code.One)
        {
            character.SetEndPosition(memoryBar.getPickupPos(1));
        }
    }

    protected override bool FinishWithoutSucceed()
    {
        if (playerCMDNo == instructionPan.GetLength())
        {
            playerCMDNo -= 1;
            if (hasSolved == 0)
            {
                FailFeedback("You can \"Store\" \"K\" in a cell. Then \"Give\" \"O\" to me.", playerFeedback);
            }
            else if (hasSolved == 1)
            {
				FailFeedback("Almost! \"Load\" \"K\" and \"Give\" it to me.", playerFeedback);
            }
            return true;
        }
        return false;
    }

    protected override void Reset()
    {
        base.Reset();
        memoryBar.EmptyMemoryBar();
        distrustOutbox.EmptyAllData();
        distrustFeedback.SetActive(false);
        playerInboxLog.Clear();
        playerOutboxLog.Clear();
        hasSolvedLog.Clear();
        playerHoldingLog.Clear();
        playerPosLog.Clear();
        distrustOutboxLog.Clear();
        memoryBarLog.Clear();
    }

    protected override Data[] InitialInboxGenerator()
    {
        Data[] initialInboxData = new Data[2];
        initialInboxData[0] = Instantiate(Resources.Load("DataBoard", typeof(Data))) as Data;
        initialInboxData[0].dataStr = "K";
        initialInboxData[1] = Instantiate(Resources.Load("DataBoard", typeof(Data))) as Data;
        initialInboxData[1].dataStr = "O";
        return initialInboxData;
    }

    protected Vector2[] InitialMemoryPickupPos()
    {
        Vector2[] initialPickupPos = new Vector2[2];
        initialPickupPos[0] = new Vector2(-204f, 43f);
        initialPickupPos[1] = new Vector2(-116f, 43f);
        return initialPickupPos;
    }

    protected override void Logging()
    {
        if (playerCMDNo >= playerPosLog.Count)
        {
            hasSolvedLog.Add(hasSolved);
            playerPosLog.Add(player.startPosition);
            playerHoldingLog.Add(player.GetData());
            playerInboxLog.Add(playerInbox.GetCurrentState());
            playerOutboxLog.Add(playerOutbox.GetCurrentState());
            distrustOutboxLog.Add(distrustOutbox.GetCurrentState());
            memoryBarLog.Add(memoryBar.GetCurrentState());
        }
    }

    void Start()
    {
        playerInbox.Initialise(new Vector2(-344f, 251f), InitialInboxGenerator());
        playerOutbox.Initialise(new Vector2(-83f, -171f));
        distrustOutbox.Initialise(new Vector2(-83f, 132f));
        memoryBar.Initialise(InitialMemoryPickupPos());
        AddButtonListener();
        playerInboxLog = new List<string[]>();
        playerOutboxLog = new List<string[]>();
        hasSolvedLog = new List<int>();
        playerHoldingLog = new List<string>();
        playerPosLog = new List<Vector2>();
        distrustOutboxLog = new List<string[]>();
        memoryBarLog = new List<string[]>();
        Reset();
        instructionPan.GetComponent<IHasFinalised>().HasFinalised();
        linkMenuEntry();
    }

    protected override void UndoCommand()
    {
        memoryBar.RestoreState(memoryBarLog[playerCMDNo]);
        if (distrustOutboxLog[playerCMDNo] == null)
        {
            distrustOutbox.ResetOutbox();
        }
        else
        {
            List<Data> tempList = new List<Data>();
            foreach (string s in distrustOutboxLog[playerCMDNo])
            {
                Data d = Instantiate(Resources.Load("DataBoard", typeof(Data))) as Data;
                d.dataStr = s;
                tempList.Add(d);
            }
            distrustOutbox.ResetOutbox(tempList.ToArray());
        }
        base.UndoCommand();
    }

    protected override void ExecuteCommand()
    {
        Logging();
        Data d;
        TopCommand runTopCommand = instructionPan.GetTopCommandAt(playerCMDNo);
        switch (runTopCommand.myCode)
        {
            case TopCommand.Code.Inbox:
                d = playerInbox.sendFirstData();
                if (d)
                {
                    player.PickupData(d);
                    ExecuteNextIfNotPaused();
                }
                else
                {
					FailFeedback("There's no more data to \"Take\" off your line.", playerFeedback);
                }
                break;
            case TopCommand.Code.Outbox:
                d = player.SendData();
                if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Boss)
                {
                    if (d)
                    {
                        playerOutbox.AcceptData(d);
                        if (hasSolved == 0 && d.dataStr == "O")
                        {
                            hasSolved = 1;
                            InformFeedback("Thanks! The next letter that I need is \"K\".", playerFeedback);
                            StartCoroutine(DiminishAfterSec(playerFeedback, 1f));
                            ExecuteNextIfNotPaused();
                        }
                        else if (hasSolved == 0 && d.dataStr == "K")
                        {
                            FailFeedback("I was expecting to receive \"O\" first. Try to \"Store\" \"K\" in a cell first.", playerFeedback);
                        }
                        else if (hasSolved == 1 && d.dataStr == "K")
                        {
                            hasSolved = 2;
                            SucceedFeedback("Well done!", playerFeedback, "Challenge5");
                        }
                        else if (hasSolved == 1 && d.dataStr == "O")
                        {
                            FailFeedback("I was not expecting to receive \"O\" twice.", playerFeedback);
                        }
                    }
                    else
                    {
						FailFeedback("You're not holding anything. Please \"Take\" or \"Load\" before \"Giving\" me data.", playerFeedback);
                    }
                }
                else if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Distrust)
                {
                    if (d)
                    {
                        distrustOutbox.AcceptData(d);
                        FailFeedback("I was not expecting to receive any data.", distrustFeedback);
                    }
                    else
                    {
                        FailFeedback("Fool. \"Take\" or \"Load\" before \"Giving\" me data.", distrustFeedback);
                    }
                }
                break;
            case TopCommand.Code.Load:
                if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Zero)
                {
                    d = memoryBar.CloneDataAt(0);
                }
                else
                {
                    d = memoryBar.CloneDataAt(1);
                }
                if (d)
                {
                    player.PickupData(d);
                    ExecuteNextIfNotPaused();
                }
                else
                {
                    FailFeedback("That cell is empty!", playerFeedback);
                }
                break;
            case TopCommand.Code.Store:
                d = player.CloneData();
                if (d)
                {
                    if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Zero)
                    {
                        memoryBar.AcceptDataAt(0, d);
                    }
                    else
                    {
                        memoryBar.AcceptDataAt(1, d);
                    }
                    ExecuteNextIfNotPaused();
                }
                else
                {
                    FailFeedback("You're not holding any data to store.", playerFeedback);
                }
                break;

        }

    }
    void Update()
    {

        if (playerOldCounter != player.counter)
        {
            playerOldCounter = player.counter;
            ExecuteCommand();
         }
       
        if (ClickHandler.checkUpdate == 2)
        {
            if (ClickHandler.isUpdated == 0)
            {
                ExecuteEvents.Execute<IUpdateSubCMDChoice>(ClickHandler.subCommandToBeChanged.gameObject, null, (x, y) => x.FinaliseSubCMDChoice(ClickHandler.subCommandToBeChanged.subCommandRef.myCode));
                ClickHandler.subCommandToBeChanged = null;
            }
            ClickHandler.checkUpdate -= 1;
        }

        if (Input.GetMouseButtonUp(0) && ClickHandler.subCommandToBeChanged)
        {
            ClickHandler.checkUpdate += 1;
        }

    }
}
