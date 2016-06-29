using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChallengeSix : HackingChallengeTemplate
{

    protected override void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode)
    {
        switch (subCode)
        {
            case SubCommand.Code.Boss:
                character.SetEndPosition(playerOutbox.playerPos);
                break;
            case SubCommand.Code.Distrust:
                character.SetEndPosition(distrustOutbox.playerPos);
                break;
            case SubCommand.Code.Zero:
                character.SetEndPosition(memoryBar.getPickupPos(0));
                break;
            case SubCommand.Code.One:
                character.SetEndPosition(memoryBar.getPickupPos(1));
                break;
        }
    }


    protected override bool FinishWithoutSucceed()
    {
        if (distrustCMDNo == enumPan.transform.childCount)
        {
            if (hasSolved == 0)
            {
                FailFeedback("You can steal data from cells.", playerFeedback);
            }
            else if (hasSolved == 1)
            {
                FailFeedback("Half-way there! Don't forget to \"Give\" me the secret message.", playerFeedback);
            }

            return true;
        }
        return false;
    }

    protected Vector2[] InitialMemoryPickupPos()
    {
        Vector2[] initialPickupPos = new Vector2[2];
        initialPickupPos[0] = new Vector2(-204f, 43f);
        initialPickupPos[1] = new Vector2(-116f, 43f);
        return initialPickupPos;
    }

    void Start()
    {
        distrustTopCode = new TopCommand.Code[6];
        distrustSubCode = new SubCommand.Code[6];
        distrustTopCode[0] = TopCommand.Code.Inbox;
        distrustSubCode[0] = SubCommand.Code.NoAction;
        distrustTopCode[1] = TopCommand.Code.Store;
        distrustSubCode[1] = SubCommand.Code.One;
        distrustTopCode[2] = TopCommand.Code.Inbox;
        distrustSubCode[2] = SubCommand.Code.NoAction;
        distrustTopCode[3] = TopCommand.Code.Outbox;
        distrustSubCode[3] = SubCommand.Code.Distrust;
        distrustTopCode[4] = TopCommand.Code.Load;
        distrustSubCode[4] = SubCommand.Code.One;
        distrustTopCode[5] = TopCommand.Code.Outbox;
        distrustSubCode[5] = SubCommand.Code.Distrust;

        playerInbox.Initialise(new Vector2(-344f, 251f), InitialInboxGenerator());
        playerOutbox.Initialise(new Vector2(-83f, -171f));
        distrustInboxPos = new Vector2(-436f, -111f);
        distrustOutbox.Initialise(new Vector2(-83f, 132f));
        memoryBar.Initialise(InitialMemoryPickupPos());
        debugPan.debugButtons[(int)ButtonCode.Run].onClick.AddListener(() => StartRunning());
        debugPan.debugButtons[(int)ButtonCode.Stop].onClick.AddListener(() => Reset());
        linkMenuEntry();
        Reset();
    }

    void Update()
    {
        if (ClickHandler.checkUpdate == 2)
        {
            if (ClickHandler.isUpdated == 0)
            {
                if (ClickHandler.subCommandToBeChanged == null)
                {
                    foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot>())
                    {
                        s.ActivateEventTrigger(true);
                    }
                }
                else
                {
                    ExecuteEvents.Execute<IUpdateSubCMDChoice>(ClickHandler.subCommandToBeChanged.gameObject, null, (x, y) => x.FinaliseSubCMDChoice(ClickHandler.subCommandToBeChanged.subCommandRef.myCode));
                    ClickHandler.subCommandToBeChanged = null;

                }

            }
            ClickHandler.checkUpdate -= 1;
        }

        if (Input.GetMouseButtonUp(0) && ClickHandler.subCommandToBeChanged)
        {
            ClickHandler.checkUpdate += 1;
        }


        Data d;
        if ((playerState != RunningState.Inactive) && (playerOldCounter != player.counter))
        {
            playerOldCounter = player.counter;
            TopCommand runTopCommand = instructionPan.GetTopCommandAt(playerCMDNo);
            switch (runTopCommand.myCode)
            {
                case TopCommand.Code.NoAction:
                    playerCMDNo += 1;
                    playerState = RunningState.Ready;
                    Invoke("RunPlayerCommand", delaySec);
                    break;
                case TopCommand.Code.Inbox:
                    d = playerInbox.sendFirstData();
                    if (d)
                    {
                        player.PickupData(d);
                        playerCMDNo += 1;
                        playerState = RunningState.Ready;
                        Invoke("RunPlayerCommand", delaySec);
                    }
                    else
                    {
						FailFeedback("There's no data to \"Take\" off your line.", playerFeedback);
                    }
                    break;
                case TopCommand.Code.Outbox:
                    d = player.SendData();
                    if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Boss)
                    {
                        if (d)
                        {
                            playerOutbox.AcceptData(d);
                            hasSolved = 2;
                            SucceedFeedback("Well done!", playerFeedback);
                        }
                        else
                        {
                            FailFeedback("You're hands are empty!", playerFeedback);
                        }
                    }
                    else if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Distrust)
                    {
                        if (d)
                        {
                            distrustOutbox.AcceptData(d);
                            FailFeedback("I was not expecting to receive \"Y\" twice.", distrustFeedback);
                        }
                        else
                        {
                            FailFeedback("You're not holding anything.", distrustFeedback);
                        }
                    }
                    break;
                case TopCommand.Code.Load:
                    d = memoryBar.CloneDataAt((int)runTopCommand.subCommandRef.myCode);
                    if (d)
                    {
                        d.dataStr = "Y";
                        player.PickupData(d);
                        hasSolved = 1;
                        playerCMDNo += 1;
                        playerState = RunningState.Ready;
                        Invoke("RunPlayerCommand", delaySec);
                    }
                    else
                    {
                        FailFeedback("Nothing to load.", playerFeedback);
                    }
                    break;
                case TopCommand.Code.Store:
                    d = player.CloneData();
                    if (d)
                    {
                        memoryBar.AcceptDataAt((int)runTopCommand.subCommandRef.myCode, d);
                        playerCMDNo += 1;
                        playerState = RunningState.Ready;
                        Invoke("RunPlayerCommand", delaySec);
                    }
                    else
                    {
                        FailFeedback("Nothing to store.", playerFeedback);
                    }
                    break;
            }
        }

        if ((distrustPlayerState != RunningState.Inactive) && (distrustOldCounter != distrust.counter))
        {
            distrustOldCounter = distrust.counter;
            switch (distrustTopCode[distrustCMDNo])
            {
                case TopCommand.Code.NoAction:
                    break;
                case TopCommand.Code.Inbox:
                    d = Instantiate(Resources.Load("DataBoard", typeof(Data))) as Data;
                    d.dataStr = "?";
                    distrust.PickupData(d);
                    break;
                case TopCommand.Code.Outbox:
                    d = distrust.SendData();
                    if (distrustSubCode[distrustCMDNo] == SubCommand.Code.Boss)
                    {
                        playerOutbox.AcceptData(d);
                    }
                    else if (distrustSubCode[distrustCMDNo] == SubCommand.Code.Distrust)
                    {
                        distrustOutbox.AcceptData(d);
                    }
                    break;
                case TopCommand.Code.Load:
                    distrust.PickupData(memoryBar.CloneDataAt((int)distrustSubCode[distrustCMDNo]));
                    break;
                case TopCommand.Code.Store:
                    memoryBar.AcceptDataAt((int)distrustSubCode[distrustCMDNo], distrust.CloneData());
                    break;
            }
            distrustCMDNo += 1;
            distrustPlayerState = RunningState.Ready;
            Invoke("RunPlayerCommand", delaySec);
        }

    }
}
