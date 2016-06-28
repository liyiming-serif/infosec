using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum RunningState
{
    Inactive, NotReady, Ready, Pause, Back, Step
};

public class CodingChallengeTemplate : MonoBehaviour
{

    [SerializeField]
    protected AnimatorController player;
    [SerializeField]
    protected GameObject playerFeedback;

    [SerializeField]
    protected Inbox playerInbox;
    [SerializeField]
    protected Outbox playerOutbox;

    [SerializeField]
    protected InstructionPanel instructionPan;
    [SerializeField]
    protected GameObject CommandSelectPan;
    [SerializeField]
    protected EnumPanel enumPan;
    [SerializeField]
    protected DebugPanel debugPan;

    /*Game State*/
    protected int hasSolved;

    protected RunningState playerState;
    protected int playerCMDNo;
    protected int playerOldCounter;

    protected float delaySec = 0.6f;

    /*Execution Record*/


    protected void PrepareFeedback(string message, GameObject feedback, Color32 colour)
    {
        Text feedbackText = feedback.GetComponentInChildren<Text>();
        feedbackText.text = message;
        Renderer changeColor = feedback.GetComponent<Renderer>();
        changeColor.material.color = colour;
        feedback.SetActive(true);
    }

    protected void InformFeedback(string message, GameObject feedback)
    {
        PrepareFeedback(message, feedback, new Color32(83, 144, 195, 212));
    }


    protected void FailFeedback(string message, GameObject feedback)
    {
        PrepareFeedback(message, feedback, new Color32(235, 46, 44, 212));
        if (playerCMDNo < instructionPan.GetLength())
        {
            enumPan.SetRunningState(playerCMDNo, EnumPanel.Status.Error);
        }
        else
        {
            enumPan.SetRunningState(playerCMDNo - 1, EnumPanel.Status.Error);
        }
        playerState = RunningState.Inactive;

        debugPan.SetDebugButtonActive(ButtonCode.Stop, true);
        debugPan.SetDebugButtonActive(ButtonCode.Step, false);
        debugPan.SetDebugButtonActive(ButtonCode.Back, (playerCMDNo > -1));
        debugPan.SetDebugButtonActive(ButtonCode.Run, false);
    }

    protected void SucceedFeedback(string message, GameObject feedback)
    {
        PrepareFeedback(message, feedback, new Color32(90, 174, 122, 212));
        enumPan.ResetRunningState();
        playerState = RunningState.Inactive;

        debugPan.SetDebugButtonActive(ButtonCode.Run, false);
        debugPan.SetDebugButtonActive(ButtonCode.Stop, false);
        debugPan.SetDebugButtonActive(ButtonCode.Back, false);
        debugPan.SetDebugButtonActive(ButtonCode.Step, false);
    }

    protected void SetCodingModeActive(bool setting)
    {
        CommandSelectPan.SetActive(setting);
        foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot>())
        {
            s.ActivateEventTrigger(setting);
        }
    }

    virtual protected bool CheckPlayerReady()
    {
        if (playerState == RunningState.Ready || playerState == RunningState.Step)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    virtual protected bool FinishWithoutSucceed()
    {
        return true;
    }

    virtual protected bool RunPlayerCommand()
    {
        if (!CheckPlayerReady())
        {
            return false;
        }
        if (FinishWithoutSucceed())
        {
            return false;
        }
        enumPan.SetRunningState(playerCMDNo, EnumPanel.Status.Executing);

        if (playerState == RunningState.Ready)
        {
            playerState = RunningState.NotReady;
        }
        else if (playerState == RunningState.Step)
        {
            playerState = RunningState.Pause;
        }

        playerOldCounter = player.counter;
        TopCommand topCommandToRun = instructionPan.GetTopCommandAt(playerCMDNo);
        if (topCommandToRun.myCode == TopCommand.Code.Inbox)
        {
            player.SetEndPosition(playerInbox.playerPos);
        }
        else if (topCommandToRun.myCode == TopCommand.Code.NoAction)
        {
            player.counter += 1;
        }
        else if (topCommandToRun.subCommandRef)
        {
            SetEndPositionBySubCMD(player, topCommandToRun.subCommandRef.myCode);
        }
        return true;
    }


    virtual protected void Reset()
    {
        hasSolved = 0;
        playerCMDNo = -1;
        playerOldCounter = player.counter;
        playerState = RunningState.Inactive;

        enumPan.ResetRunningState();
        player.ResetAnimator();
        playerInbox.ResetInbox(InitialInboxGenerator());
        playerOutbox.EmptyAllData();
        playerFeedback.SetActive(false);
        SetCodingModeActive(true);


        debugPan.SetDebugButtonActive(ButtonCode.Run, true);
        debugPan.SetDebugButtonActive(ButtonCode.Step, true);
        debugPan.SetDebugButtonActive(ButtonCode.Stop, false);
        debugPan.SetDebugButtonActive(ButtonCode.Back, false);

    }

    virtual protected void StartRunning()
    {
        if (playerState == RunningState.Inactive)
        {
            Reset();
            playerCMDNo = 0;
            SetCodingModeActive(false);
        }
        else if (playerState != RunningState.Pause)
        {
            throw new System.Exception("An unexpected player state: " + playerState);
        }
        playerState = RunningState.Ready;

        debugPan.SetDebugButtonActive(ButtonCode.Run, false);
        debugPan.SetDebugButtonActive(ButtonCode.Stop, true);
        debugPan.SetDebugButtonActive(ButtonCode.Back, (playerCMDNo > -1));
        debugPan.SetDebugButtonActive(ButtonCode.Step, true);

        Invoke("RunPlayerCommand", delaySec);

    }

    virtual protected void StartStepping()
    {
        switch (playerState)
        {
            case RunningState.Inactive:
                Reset();
                playerCMDNo = 0;
                SetCodingModeActive(false);
                playerState = RunningState.Step;
                break;
            case RunningState.Pause:
                playerState = RunningState.Step;
                break;
            case RunningState.NotReady:
                playerState = RunningState.Pause;
                break;
            case RunningState.Ready:
                //ignore
                return;
            default:
                //should not be in Step state. The button should be disactivated.
                throw new System.Exception("An unexpected player state: " + playerState);
        }

        debugPan.SetDebugButtonActive(ButtonCode.Stop, true);
        debugPan.SetDebugButtonActive(ButtonCode.Run, false);
        debugPan.SetDebugButtonActive(ButtonCode.Back, false);
        debugPan.SetDebugButtonActive(ButtonCode.Step, false);

        Invoke("RunPlayerCommand", delaySec);
    }

    virtual protected void StartBackStepping()
    {
        switch (playerState)
        {
            case RunningState.Inactive:
                //Must be in failling state
                playerState = RunningState.Back;    //Serves as a lock
                break;
            case RunningState.Pause:
                playerState = RunningState.Back; 
                break;
            case RunningState.NotReady:
                playerState = RunningState.Pause;
                debugPan.SetDebugButtonActive(ButtonCode.Stop, true);
                debugPan.SetDebugButtonActive(ButtonCode.Run, false);
                debugPan.SetDebugButtonActive(ButtonCode.Back, false);
                debugPan.SetDebugButtonActive(ButtonCode.Step, false);
                return;
            default:
                // Ignore the request if @ Back or Step RunningState.
                return;
        }

        /* Undo One Command*/
        UndoCommand();
        if(playerCMDNo < 0)
        {
            player.RebasePosition(player.initPosition);
            playerState = RunningState.Inactive; //The beginning of the state. Should be reset.
        }
        else
        {
            playerState = RunningState.Pause; //Release the lock
        }
        
        debugPan.SetDebugButtonActive(ButtonCode.Stop, true);
        debugPan.SetDebugButtonActive(ButtonCode.Run, true);
        debugPan.SetDebugButtonActive(ButtonCode.Back, (playerCMDNo > -1));
        debugPan.SetDebugButtonActive(ButtonCode.Step, true);

    }

    virtual protected bool ExecuteNextIfNotPaused()
    {
        if (playerState == RunningState.Pause)
        {
            debugPan.SetDebugButtonActive(ButtonCode.Run, true);
            debugPan.SetDebugButtonActive(ButtonCode.Stop, true);
            debugPan.SetDebugButtonActive(ButtonCode.Back, (playerCMDNo > -1));
            debugPan.SetDebugButtonActive(ButtonCode.Step, true);
            return false;
        }
        else if (playerState == RunningState.NotReady)
        {
            playerState = RunningState.Ready;

            debugPan.SetDebugButtonActive(ButtonCode.Step, true);
            debugPan.SetDebugButtonActive(ButtonCode.Run, false);
            debugPan.SetDebugButtonActive(ButtonCode.Step, true);
            debugPan.SetDebugButtonActive(ButtonCode.Back, (playerCMDNo > -1));

            Invoke("RunPlayerCommand", delaySec);
            return true;
        }
        else
        {
            throw new System.Exception("An unexpected player state: " + playerState);
        }

    }

    virtual protected void UndoCommand()
    {
 
    }

    virtual protected void ExecuteCommand()
    {

    }

    virtual protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode)
    {

    }

    virtual protected Data[] InitialInboxGenerator()
    {
        return null;
    }

    protected IEnumerator DiminishAfterSec(GameObject feedback, float time)
    {
        yield return new WaitForSeconds(time);
        if (playerState != RunningState.Inactive)
        {
            feedback.SetActive(false);
        }
    }

    protected void AddButtonListener()
    {
        debugPan.debugButtons[(int)ButtonCode.Run].onClick.AddListener(() => StartRunning());
        debugPan.debugButtons[(int)ButtonCode.Stop].onClick.AddListener(() => Reset());
        debugPan.debugButtons[(int)ButtonCode.Step].onClick.AddListener(() => StartStepping());
        debugPan.debugButtons[(int)ButtonCode.Back].onClick.AddListener(() => StartBackStepping());
    }

}
