using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ChallengeOne : CodingChallengeTemplate
{

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

    protected override void UndoCommand()
    {
        if (playerCMDNo < instructionPan.GetLength())
        {
            Data d;
            TopCommand runTopCommand = instructionPan.GetTopCommandAt(playerCMDNo);
            switch (runTopCommand.myCode)
            {
                case TopCommand.Code.Inbox:
                    d = playerInbox.sendFirstData();
                    if (d)
                    {
                        player.PickupData(d);
                        playerCMDNo += 1;
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
                                playerCMDNo += 1;
                                ExecuteNextIfNotPaused();
                            }
                            else if (hasSolved == 0 && d.dataStr == "K")
                            {
                                FailFeedback("You have dropped letter \"O\". Please send both letters to me.", playerFeedback);
                            }
                            else if (hasSolved == 1 && d.dataStr == "K")
                            {
                                hasSolved += 1;
                                SucceedFeedback("Thanks for giving me \"OK\".", playerFeedback);
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
        playerCMDNo -= 1;
        enumPan.SetRunningState(playerCMDNo, EnumPanel.Status.Executing);
    }

    protected override void ExecuteCommand()
    {
        Data d;
        TopCommand runTopCommand = instructionPan.GetTopCommandAt(playerCMDNo);
        switch (runTopCommand.myCode)
        {
            case TopCommand.Code.Inbox:
                d = playerInbox.sendFirstData();
                if (d)
                {
                    player.PickupData(d);
                    playerCMDNo += 1;
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
                            playerCMDNo += 1;
                            ExecuteNextIfNotPaused();
                        }
                        else if (hasSolved == 0 && d.dataStr == "K")
                        {
                            FailFeedback("You have dropped letter \"O\". Please send both letters to me.", playerFeedback);
                        }
                        else if (hasSolved == 1 && d.dataStr == "K")
                        {
                            hasSolved += 1;
                            SucceedFeedback("Thanks for giving me \"OK\".", playerFeedback);
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
    void Start()
    {
        playerInbox.Initialise(new Vector2(-344f, 251f), InitialInboxGenerator());
        playerOutbox.Initialise(new Vector2(-83f, -171f));
        AddButtonListener();
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

    }
}
