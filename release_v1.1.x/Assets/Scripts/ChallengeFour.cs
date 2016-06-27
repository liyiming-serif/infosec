using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeFour : CodingChallengeTemplate
{

    [SerializeField]
    protected GameObject distrustFeedback;
    [SerializeField]
    protected Outbox distrustOutbox;
    [SerializeField]
    protected MemoryBar memoryBar;

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
            if (hasSolved == 0)
            {
                FailFeedback("You can \"Store\" \"K\" in a cell. Then \"Give\" \"O\" to me.", playerFeedback);
            }
            else if (hasSolved == 1)
            {
                FailFeedback("You are half-way the goal! \"Load\" \"K\" from the cell storing \"K\".", playerFeedback);
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

    void Start()
    {
        playerInbox.Initialise(new Vector2(-344f, 251f), InitialInboxGenerator());
        playerOutbox.Initialise(new Vector2(-83f, -171f));
        distrustOutbox.Initialise(new Vector2(-83f, 132f));
        memoryBar.Initialise(InitialMemoryPickupPos());
        debugButtons[(int)ButtonCode.Run].onClick.AddListener(() => StartRunning());
        debugButtons[(int)ButtonCode.Stop].onClick.AddListener(() => Reset());
        debugButtons[(int)ButtonCode.Step].onClick.AddListener(() => StartStepping());
        Reset();
    }

    void Update()
    {
        Data d;
        if (playerOldCounter != player.counter)
        {
            playerOldCounter = player.counter;
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
                                playerCMDNo += 1;
                                ExecuteNextIfNotPaused();
                            }
                            else if (hasSolved == 0 && d.dataStr == "K")
                            {
                                FailFeedback("I was expecting to receive \"O\" first. You can \"Store\" \"K\" in a cell first.", playerFeedback);
                            }
                            else if (hasSolved == 1 && d.dataStr == "K")
                            {
                                hasSolved = 2;
                                SucceedFeedback("Well done!", playerFeedback);
                            }
                            else if (hasSolved == 1 && d.dataStr == "O")
                            {
                                FailFeedback("I was not expecting to receive \"O\" twice.", playerFeedback);
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
                            FailFeedback("I was not expecting to receive any data.", distrustFeedback);
                        }
                        else
                        {
                            FailFeedback("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
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
                        playerCMDNo += 1;
                        ExecuteNextIfNotPaused();
                    }
                    else
                    {
                        FailFeedback("No data can be loaded from it.", playerFeedback);
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
                        playerCMDNo += 1;
                        ExecuteNextIfNotPaused();
                    }
                    else
                    {
                        FailFeedback("No data can you store on the ground.", playerFeedback);
                    }
                    break;

            }
        }

    }
}
