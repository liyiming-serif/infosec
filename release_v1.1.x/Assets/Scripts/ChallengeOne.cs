using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ChallengeOne : CodingChallengeTemplate
{

    [SerializeField]
    GameObject tutorialTake;
    [SerializeField]
    GameObject tutorialArrow;

    [SerializeField]
    GameObject stopHint;
    [SerializeField]
    GameObject debugHint;


    protected override void FailFeedback(string message, GameObject feedback)
    {
        base.FailFeedback(message, feedback);
        stopHint.SetActive(true);
        debugHint.SetActive(true);
    }

    override protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode)
    {
        if (subCode == SubCommand.Code.Boss)
        {
            character.SetEndPosition(playerOutbox.playerPos);
        }
        else
        {
            Debug.Log("Should not reach here.");
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
                if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Boss)
                {
                    d = player.SendData();
                    if (d)
                    {
                        playerOutbox.AcceptData(d);
                        if (hasSolved == 0 && d.dataStr == "O")
                        {
                            hasSolved += 1;
                            ExecuteNextIfNotPaused();
                        }
                        else if (hasSolved == 0 && d.dataStr == "K")
                        {
                            FailFeedback("You have dropped letter \"O\". Please send both letters to me.", playerFeedback);
                        }
                        else if (hasSolved == 1 && d.dataStr == "K")
                        {
                            hasSolved += 1;
                            SucceedFeedback("Well done!", playerFeedback, "Challenge2");
                        }
                    }
                    else
                    {
                        FailFeedback("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
                    }
                }
                else
                {
                    Debug.Log("Should not reach here @ 1 Update, Challenge One.");
                }
                break;
            default:
                throw new System.Exception("An unexpected command: " + runTopCommand.myCode);
        }
    }

    protected override void Reset()
    {
        base.Reset();
        stopHint.SetActive(false);
        debugHint.SetActive(false);
    }

    protected override void StartBackStepping()
    {
        base.StartBackStepping();
        stopHint.SetActive(false);
        debugHint.SetActive(false);
    }
    void Start()
    {
        playerInbox.Initialise(new Vector2(-344f, 251f), InitialInboxGenerator());
        playerOutbox.Initialise(new Vector2(-83f, -171f));
        AddButtonListener();
        playerInboxLog = new List<string[]>();
        playerOutboxLog = new List<string[]>();
        hasSolvedLog = new List<int>();
        playerHoldingLog = new List<string>();
        playerPosLog = new List<Vector2>();
        Reset();
        instructionPan.GetComponent<IHasFinalised>().HasFinalised();
        linkMenuEntry();
    }

    void Update()
    {
        if(instructionPan.GetLength() == 0)
        {
            tutorialArrow.SetActive(true);
            tutorialTake.SetActive(true);
        }else
        {
            tutorialArrow.SetActive(false);
            tutorialTake.SetActive(false);
        }
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
