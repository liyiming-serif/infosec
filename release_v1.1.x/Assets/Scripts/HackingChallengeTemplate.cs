using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class HackingChallengeTemplate : MonoBehaviour {

	[SerializeField]
    protected AnimatorController player;
	[SerializeField]
    protected Inbox playerInbox;

    [SerializeField]
    protected GameObject playerFeedback;
    [SerializeField]
    protected Outbox playerOutbox;

	[SerializeField]
    protected AttackInstructionPanel instructionPan;
	[SerializeField]
    protected EnumPanel enumPan;
    [SerializeField]
    protected GameObject commandSelectPan;
    [SerializeField]
    protected DebugPanel debugPan;

    [SerializeField]
    protected AnimatorController distrust;

    [SerializeField]
    protected GameObject distrustFeedback;
    [SerializeField]
    protected Outbox distrustOutbox;
    [SerializeField]
    protected MemoryBar memoryBar;

    [SerializeField]
    protected GameObject menuPanel;

    [SerializeField]
    protected Button menuToggle;

    /*Game Logic*/
    protected int hasSolved;

    protected Vector2 distrustInboxPos;

    protected TopCommand.Code[] distrustTopCode;
    protected SubCommand.Code[] distrustSubCode;

    protected RunningState distrustPlayerState;
    protected int distrustCMDNo;
    protected int distrustOldCounter;
        
	protected RunningState playerState;
	protected int playerCMDNo;
	protected int playerOldCounter;

	protected float delaySec = 0.6f;

	protected void PrepareFeedback(string message, GameObject feedback, Color32 colour){
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = colour;
		feedback.SetActive (true);
	}

	protected void InformFeedback(string message, GameObject feedback){
		PrepareFeedback (message, feedback, new Color32(83, 144, 195, 212));
	}

	protected void FailFeedback(string message, GameObject feedback) {
		PrepareFeedback (message, feedback, new Color32(235, 46, 44, 212));
		if (playerCMDNo < enumPan.transform.childCount) {
			enumPan.SetRunningState (playerCMDNo, EnumPanel.Status.Error);
		} else {
			enumPan.SetRunningState (playerCMDNo - 1, EnumPanel.Status.Error);
		}
		playerCMDNo = -1;
		playerState = RunningState.Inactive;
	}

	protected void SucceedFeedback(string message, GameObject feedback, string challengeName = null) {
		PrepareFeedback (message, feedback, new Color32(90, 174, 122, 212));
		enumPan.ResetRunningState ();
		playerCMDNo = -1;
        playerState = RunningState.Inactive;
        if (challengeName != null)
        {
            StartCoroutine(ToNextChallenge(challengeName));
        }
	}

    protected IEnumerator ToNextChallenge(string stageName)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(stageName);
    }

    protected void SetCodingModeActive(bool setting){
        commandSelectPan.SetActive(setting);
        foreach (AttackTopCommandSlot s in GameObject.FindObjectsOfType<AttackTopCommandSlot> ()) {
			s.ActivateEventTrigger (setting);
		}
	}

    virtual protected bool CheckPlayerReady()
    {

        if ((playerState == RunningState.NotReady) || (distrustPlayerState == RunningState.NotReady))
        {
            return false;
        }
        return true;
    }


    virtual protected bool FinishWithoutSucceed(){
		return true;
	}
		
    virtual protected bool DistrustMoves()
    {
        if (distrustPlayerState != RunningState.Inactive)
        {
            if (distrustCMDNo == enumPan.transform.childCount)
            {
                enumPan.ResetRunningState();
                distrustPlayerState = RunningState.Inactive;
                return false;
            }
            enumPan.SetRunningState(distrustCMDNo, EnumPanel.Status.Executing);
            distrustPlayerState = RunningState.NotReady;
            distrustOldCounter = distrust.counter;
            TopCommand.Code topCodeToRun = distrustTopCode[distrustCMDNo];
            if (topCodeToRun == TopCommand.Code.Inbox)
            {
                distrust.SetEndPosition(distrustInboxPos);
            }
            else if (distrustSubCode[distrustCMDNo] != SubCommand.Code.NoAction)
            {
                SetEndPositionBySubCMD(distrust, distrustSubCode[distrustCMDNo]);
            }
        }

        return true;
    }
    virtual protected bool RunPlayerCommand() {

        if (!CheckPlayerReady())
        {
            return false;
        }
        if (FinishWithoutSucceed())
        {
            return false;
        }
        if (!DistrustMoves())
        {
            return false;
        }
        if (playerState != RunningState.Inactive)
        {
            playerState = RunningState.NotReady;
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
        }
        
        return true;
    }

	 virtual protected void Reset(){
		hasSolved = 0;
		playerState = RunningState.Inactive;
		playerCMDNo = -1;
		playerOldCounter = player.counter;
        player.ResetAnimator ();

        playerInbox.ResetInbox (InitialInboxGenerator());
		playerOutbox.EmptyAllData();
		playerFeedback.SetActive (false);

        enumPan.ResetRunningState();
        memoryBar.EmptyMemoryBar();

        distrustCMDNo = -1;
        distrustPlayerState = RunningState.Inactive;
        distrustOldCounter = distrust.counter;
        distrust.ResetAnimator();

        distrustOutbox.EmptyAllData();
        distrustFeedback.SetActive(false);
        SetCodingModeActive(true);

        debugPan.SetDebugButtonActive(ButtonCode.Run, true);
        debugPan.SetDebugButtonActive(ButtonCode.Stop, false);
        //TODO Implement step back and step forward
        debugPan.debugButtons[(int)ButtonCode.Back].gameObject.SetActive(false);
        debugPan.debugButtons[(int)ButtonCode.Step].gameObject.SetActive(false);
    }

    virtual protected void StartRunning() {
        Reset();
        playerCMDNo = 0;
        distrustCMDNo = 0;
        distrustPlayerState = RunningState.Ready;
        playerState = RunningState.Ready;
        SetCodingModeActive(false);

        debugPan.SetDebugButtonActive(ButtonCode.Run, false);
        debugPan.SetDebugButtonActive(ButtonCode.Stop, true);

        Invoke("RunPlayerCommand", delaySec);
	}

	virtual protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode){
	
	}

	virtual protected Data[] InitialInboxGenerator (){
		return null;
	}

	protected IEnumerator DiminishAfterSec(GameObject feedback, float time)
	{
		yield return new WaitForSeconds(time);
		if (playerState != RunningState.Inactive) {
			feedback.SetActive (false);
		}
	}

    protected void MenuToggle()
    {
        menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
    }

    protected void linkMenuEntry()
    {
        menuPanel.gameObject.SetActive(false);
        menuToggle.onClick.AddListener(() => MenuToggle());
        for(int i = 0; i < 6; i++)
        {
            string s = "Challenge" + ((int)(i + 1)).ToString();
            menuPanel.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(s));
        }
    }
}
