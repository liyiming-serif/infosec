using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChallengeTwo : CodingChallengeTemplate
{

    [SerializeField]
    protected GameObject distrustFeedback;
    [SerializeField]
    protected Outbox distrustOutbox;

    /*Execution Record*/
    protected List<string[]> distrustOutboxLog;

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
    }

    override protected Data[] InitialInboxGenerator()
    {
        Data[] initialInboxData = new Data[2];
        initialInboxData[0] = Instantiate(Resources.Load("DataBoard", typeof(Data))) as Data;
        initialInboxData[0].dataStr = "O";
        initialInboxData[1] = Instantiate(Resources.Load("DataBoard", typeof(Data))) as Data;
        initialInboxData[1].dataStr = "K";
        return initialInboxData;
    }

    protected override bool FinishWithoutSucceed()
    {
        if (playerCMDNo == instructionPan.GetLength())
        {
            playerCMDNo -= 1;
            switch (hasSolved)
            {
                case 0:
                    FailFeedback("You need to \"Take\" and \"Give\" for every letter. There are 2 sets to be completed.", playerFeedback);
                    break;
                case 1:
                    FailFeedback("You are half-way the goal. Maybe redo \"Take\" and \"Give\" again?", playerFeedback);
                    break;
            }
            return true;
        }
        return false;
    }

    protected override void Reset()
    {
        base.Reset();
        distrustOutbox.EmptyAllData();
        distrustFeedback.SetActive(false);
        playerInboxLog.Clear();
        playerOutboxLog.Clear();
        hasSolvedLog.Clear();
        playerHoldingLog.Clear();
        playerPosLog.Clear();
        distrustOutboxLog.Clear();
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
        }

    }

    protected override void UndoCommand()
    {
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
                    FailFeedback("There is no data on the line. Try \"Give\" what you had to me.", playerFeedback);
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
                            FailFeedback("I was not expecting to receive \"O\".", playerFeedback);
                        }
                        else if (hasSolved == 0 && d.dataStr == "K")
                        {
                            FailFeedback("You have dropped letter \"O\".", distrustFeedback);
                        }
                        else if (hasSolved == 1 && d.dataStr == "K")
                        {
                            hasSolved += 1;
                            SucceedFeedback("Well done!", playerFeedback);
                        }
                    }
                    else
                    {
                        FailFeedback("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
                    }
                }
                else if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Distrust)
                {
                    if (d)
                    {
                        distrustOutbox.AcceptData(d);
                        if (hasSolved == 0 && d.dataStr == "O")
                        {
                            hasSolved += 1;
                            InformFeedback("Thanks, I got what I need.", distrustFeedback);
                            StartCoroutine(DiminishAfterSec(distrustFeedback, 1f));
                            ExecuteNextIfNotPaused();
                        }
                        else if (hasSolved == 0 && d.dataStr == "K")
                        {
                            FailFeedback("You have dropped letter\"O\". I was not expecting to receive \"K\".", distrustFeedback);
                        }
                        else if (hasSolved == 1 && d.dataStr == "K")
                        {
                            FailFeedback("I hwas not expecting to receive all data.", distrustFeedback);
                        }
                    }
                    else
                    {
                        FailFeedback("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
                    }
                }
                break;
        }
    }

    void Start()
    {
        playerInbox.Initialise(new Vector2(-344f, 251f), InitialInboxGenerator());
        playerOutbox.Initialise(new Vector2(-83f, -171f));
        distrustOutbox.Initialise(new Vector2(-83f, 132f));
        AddButtonListener();
        playerInboxLog = new List<string[]>();
        playerOutboxLog = new List<string[]>();
        hasSolvedLog = new List<int>();
        playerHoldingLog = new List<string>();
        playerPosLog = new List<Vector2>();
        distrustOutboxLog = new List<string[]>();
        Reset();
        instructionPan.GetComponent<IHasFinalised>().HasFinalised();
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
