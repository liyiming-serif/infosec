using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class NewChallengeFiveManager: MonoBehaviour, IHasChanged{ 

	/*Constants*/
	public const string attackerName = "Boss";
	public const string playerName = "Nikita";
	public const string LocZero = "0";
	public const string LocOne = "1";
	public const string dataE = "E";
	public const string dataY = "Y";

	/*Gameborad interfaces*/


	[SerializeField] NewPlayerController attackerCharacter;
	[SerializeField] NewPlayerController playerCharacter;

	[SerializeField] GameObject attackerFeedback;
	[SerializeField] GameObject playerFeedback;

	[SerializeField] GameObject commandContent;
	[SerializeField] List<AttackCommand> attackerCommands;
	[SerializeField] Command commandTemplate;
	[SerializeField] Button runButton;

	[SerializeField] NewInbox playerInbox;
	[SerializeField] NewInbox attackerInbox;
	[SerializeField] NewOutbox attackerOutbox;
	[SerializeField] NewOutbox playerOutbox;
	[SerializeField] NewMemoryBar memoryBar;

	/*game logic states*/
	private List<Command> playerCommands;

	private bool playerRunning;
	private bool attackerRunning;
	private bool[] readySignals;
	private bool isSucceeded;

	private int playerCmdNo;
	private int playerOldCounter;

	private int attackerCmdNo;
	private int attackerOldCounter;

	private int countDown;
	private int index;

	void ActivateCodingPanel(bool setting) {
		foreach (Command c in playerCommands)
		{
			c.SetEnable (setting);
		}
	}

	void setEndPosition(NewPlayerController character, string captionTxt){
		switch (captionTxt) {
		case attackerName:
			character.SetEndPosition (attackerOutbox.playerPos);
			break;
		case playerName:
			character.SetEndPosition (playerOutbox.playerPos);

			break;
		case LocZero:
			character.SetEndPosition (memoryBar.getPickupPos(0));
			break;
		case LocOne:
			character.SetEndPosition (memoryBar.getPickupPos(1));
			break;
		default:
			
			break;
		}
	}

	NewBox[] GenInitSpyInputBoxes(){
		NewBox[] initSpyInputBoxes = new NewBox[2];
		initSpyInputBoxes[0] = Instantiate(Resources.Load("slotPurpleQM", typeof(NewBox))) as NewBox;
		initSpyInputBoxes[1] = Instantiate(Resources.Load("slotPurpleQM", typeof(NewBox))) as NewBox;
		return initSpyInputBoxes;
	}

	void StartRunning()
	{
		Reset ();
		Invoke ("RunPlayerCommand", 0.6f);
		Invoke ("RunAttackerCommand", 0.6f);
	}
		

	void RunAttackerCommand()
	{
		if (!attackerRunning) {
			return;
		}
		if (attackerCmdNo == attackerCommands.Count) {
			attackerRunning = false;
			return;
		}

		attackerOldCounter = attackerCharacter.counter;
		int optionValue = attackerCommands [attackerCmdNo].optionMenu.value;
		if (optionValue == 0) {
			attackerCharacter.counter += 1; //No Action
		} else {
			setEndPosition (attackerCharacter, attackerCommands [attackerCmdNo].actTo.GetCaptionText ());
		}
	}

	void RunPlayerCommand()
	{
		if (!playerRunning) {
			return;
		}
		if (playerCmdNo == playerCommands.Count) {
			attackerCommands [--playerCmdNo].SetStatus (Command.Status.NotExecuting);//The end of the programme
			playerRunning = false;
			return;
		}	
		if (playerCmdNo > 0) {
			attackerCommands [playerCmdNo - 1].SetStatus (Command.Status.NotExecuting);
		}
		attackerCommands [playerCmdNo].SetStatus (Command.Status.Executing);

		playerOldCounter = playerCharacter.counter;
		if (playerCommands [playerCmdNo].optionMenu.value == 1) {
			playerCharacter.SetEndPosition (playerInbox.playerPos);
		} else {
			setEndPosition (playerCharacter, playerCommands [playerCmdNo].actTo.GetCaptionText ());
		}
	}

	void Inform(string message, GameObject feedback){
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(83,144,195,212);
		feedback.SetActive (true);
	}

	void Fail(string message, GameObject feedback)
	{
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(235,46,44,212);
		feedback.SetActive (true);

		attackerRunning = false;
	}

	void Succeed(string message, GameObject feedback)
	{
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(90,174,122,212);
		feedback.SetActive (true);

		attackerRunning = false;
		isSucceeded = true;
		runButton.interactable = false;
	}

	void Reset()
	{
		playerRunning = true;
		attackerRunning = true;
		isSucceeded = false;
		readySignals [0] = false;
		readySignals [1] = false;


		attackerCommands [playerCmdNo].SetStatus (Command.Status.NotExecuting);
		attackerCmdNo = 0;
		attackerOldCounter = attackerCharacter.counter;

		playerCmdNo = 0;
		playerOldCounter = playerCharacter.counter;


		playerCharacter.ResetAnimator ();
		attackerCharacter.ResetAnimator ();

		playerInbox.ResetInbox (GenInitSpyInputBoxes());
		attackerOutbox.EmptyBoxes ();
		playerOutbox.EmptyBoxes ();
		memoryBar.EmptyMemoryBar ();
		attackerFeedback.SetActive (false);
		playerFeedback.SetActive (false);
	}
		
	void Start () {

		playerCommands = new List<Command> ();

		playerCommands.Add (Instantiate(commandTemplate));
		playerCommands [0].optionMenu.value = 1;
		playerCommands.Add (Instantiate(commandTemplate));
		playerCommands [1].optionMenu.value = 4;
		playerCommands.Add (Instantiate(commandTemplate));
		playerCommands [2].optionMenu.value = 1;
		playerCommands.Add (Instantiate(commandTemplate));
		playerCommands [3].optionMenu.value = 2;
		playerCommands [3].actTo.hasOption.value = 1;
		playerCommands.Add (Instantiate(commandTemplate));
		playerCommands [4].optionMenu.value = 3;
		playerCommands.Add (Instantiate(commandTemplate));
		playerCommands [5].optionMenu.value = 2;
		playerCommands [5].actTo.hasOption.value = 1;

		foreach (AttackCommand c in attackerCommands) {
			c.UpdateAdvancedMenus ();
		}

		playerRunning = false;
		attackerRunning = false;
		isSucceeded = false;
		readySignals = new bool[2];
		readySignals [0] = false;
		readySignals [1] = false;


		playerCmdNo = 0;
		playerOldCounter = playerCharacter.counter;

		attackerCmdNo = 0;
		attackerOldCounter = attackerCharacter.counter;

		playerInbox.Initialise(new Vector2 (-293f, 42f), GenInitSpyInputBoxes());
		attackerInbox.Initialise (new Vector2 (-293f, -140f));
		attackerOutbox.Initialise (new Vector2 (-151f, -211f));
		playerOutbox.Initialise (new Vector2 (-147f, -39f));
		Vector2[] memoryPlayerLoc = new Vector2[2];
		memoryPlayerLoc [0] = new Vector2 (-225f, -39f);
		memoryPlayerLoc [1] = new Vector2 (-166f, -39f);
		memoryBar.Initialise (memoryPlayerLoc);

		runButton.onClick.AddListener (() => StartRunning());

		attackerFeedback.SetActive (false);
		playerFeedback.SetActive (false);

	}

	void Update () {
		//TODO Put aside synchronisation
		if(playerOldCounter != playerCharacter.counter){ //player's movement is done
			playerOldCounter = playerCharacter.counter;	
			switch(playerCommands[playerCmdNo].optionMenu.value){
			case 1:
				playerCharacter.PickupBox (playerInbox.sendFirstBox ());
				break;
			case 2:
				if (playerCommands [playerCmdNo].actTo.GetCaptionText () == attackerName) {
					attackerOutbox.AcceptBox (playerCharacter.SendBox ());
				} else if (playerCommands [playerCmdNo].actTo.GetCaptionText () == playerName) {
					playerOutbox.AcceptBox (playerCharacter.SendBox ());
				}
				break;
			case 3:
				int.TryParse (playerCommands [playerCmdNo].actTo.GetCaptionText (), out index);
				playerCharacter.PickupBox (memoryBar.CloneBoxAt (index));
				break;
			case 4:
				int.TryParse (playerCommands [playerCmdNo].actTo.GetCaptionText (), out index);
				memoryBar.AcceptBoxAt (index, playerCharacter.CloneBox ());
				break;
			}
			playerCmdNo++;
			readySignals [0] = true;
		}

		if (attackerOldCounter != attackerCharacter.counter) {
			attackerOldCounter = attackerCharacter.counter;
			switch(attackerCommands[attackerCmdNo].optionMenu.value){
			case 0:
				attackerCmdNo++;
				readySignals [1] = true;
				break;
			case 1:
				int.TryParse (attackerCommands [attackerCmdNo].actTo.GetCaptionText (), out index);
				if (memoryBar.CloneBoxAt (index) == null) {
					Fail ("There is no box you can pick up.", attackerFeedback);
				} else {
					attackerCharacter.PickupBox (Instantiate(Resources.Load("slotPurpleE", typeof(NewBox))) as NewBox);
					attackerCmdNo++;
					readySignals [1] = true;
				}
				break;
			case 2:
				if (attackerCharacter.IsCarryingBox ()) {
					attackerOutbox.AcceptBox (attackerCharacter.SendBox ());
					Succeed("That's E!! Well done! We stole their private box.", attackerFeedback);
				} else {
					attackerCommands [attackerCmdNo].SetStatus (Command.Status.Error);
					Fail ("You don't have any box in your hands.", attackerFeedback);
				}
				break;
			}

		}

		if (attackerRunning) {
			if (readySignals [0] && readySignals [1]) {
				readySignals [0] = false;
				readySignals [1] = false;
				Invoke ("RunPlayerCommand", 0.6f);
				Invoke ("RunAttackerCommand", 0.6f);
			}
		} else {
			if (readySignals [0]) {
				Invoke ("RunPlayerCommand", 0.6f);
			}
		}
		
			
	}

	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		Succeed("You've sent 1 out of 1 correct box to your boss.", attackerFeedback);
	}

	#region IHasChanged implementation
	public void HasChanged (NewBox boxToAccept)
	{
		playerOutbox.AcceptBox (boxToAccept); // Successfully drag the private box to spyOutbox from meory bar.

		StartCoroutine (ExecuteAfterTime(1f)); // It's like creating a child thread.
	}
	#endregion
}

// Inform the canvas to update its state based on changes in the inventory panel.
namespace UnityEngine.EventSystems{
	public interface IHasChanged : IEventSystemHandler {
		void HasChanged(NewBox boxToAccept);
	}
}