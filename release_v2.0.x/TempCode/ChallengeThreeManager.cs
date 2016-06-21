using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeThreeManager: MonoBehaviour { 

	/*Constants*/
	public const int numCommands = 2;
	public const int numPlayerBoxes = 0;
	public const int numSpyBoxes = 0;
	public const int numMemoryBoxes = 3;

	/*Gameborad interfaces*/
	public Command[] commands = new Command[numCommands];


	public PlayerController player;
	public PlayerController spy;

	public Inbox playerInbox;
	public Outbox bossOutbox;

	public Inbox spyInbox;
	public Outbox spyOutbox;

	public Button runButton;

	public GameObject[] playerBoxes = new GameObject[numPlayerBoxes];
	public GameObject[] spyBoxes = new GameObject[numSpyBoxes];
	public GameObject[] memoryBoxes = new GameObject[numMemoryBoxes];

	public GameObject playerOutboxDialogue;
	public GameObject spyOutboxDialogue;

	/*game logic states*/
	public bool isExecuting;
	public bool isSucceeded;

	public int playerCmd;
	public int playerOldCounter;

	public int spyCmd;
	public int spyOldCounter;

	public int countDown;
	public Vector2[] pickupMemoryBoxPosition = new Vector2[2];

	void ActivateCodingPanel(bool setting) {
		for (int i = 0; i < numCommands; i++)
		{
			commands [i].SetEnable (setting);
		}
	}

	void StartRunning()
	{
		Reset ();
		RunSpyAnimation ();
		RunFirstCommand ();
	}

	void RunSpyAnimation()
	{
		switch (spyCmd) 
		{
		case 0:
			spy.SetEndPosition (pickupMemoryBoxPosition[0]);
			spyOldCounter = spy.counter;
			break;
		case 1:
			spy.SetEndPosition (spyOutbox.playerPosition);
			spyOldCounter = spy.counter;
			break;
		}
	}

	void RunFirstCommand()
	{
		switch (commands [0].optionMenu.value)
		{
		case 0:
			this.FailRunning ("Command is not selected.", bossOutbox.feedback);
			break;
		case 1:
			commands [0].SetStatus (Command.Status.Error);
			player.SetEndPosition (playerInbox.playerPosition);
			playerOldCounter = player.counter;
			break;
		case 2:
			commands [0].SetStatus (Command.Status.Error);
			playerOldCounter = player.counter;
			if (commands [0].actTo.GetCaptionText() == "Boss") {
				player.SetEndPosition (bossOutbox.playerPosition);
			} else if (commands [0].actTo.GetCaptionText() == "Nikita") {
				player.SetEndPosition (spyOutbox.playerPosition);
			}
			break;
		case 3:
			playerOldCounter = player.counter;
			if (commands [0].actTo.GetCaptionText() == "0") {
				commands [0].SetStatus (Command.Status.Correct);
				player.SetEndPosition (pickupMemoryBoxPosition[0]);
			} else if (commands [0].actTo.GetCaptionText() == "1") {
				commands [0].SetStatus (Command.Status.Error);
				player.SetEndPosition (pickupMemoryBoxPosition[1]);
			}
			break;
		}
	}

	void RunSecondCommand()
	{
		commands [0].SetStatus (Command.Status.NotExecuting);
		switch (commands [1].optionMenu.value) 
		{
		case 0:
			this.FailRunning ("Command is not selected.", bossOutbox.feedback);
			break;
		case 1:
			commands [1].SetStatus (Command.Status.Error);
			player.SetEndPosition (playerInbox.playerPosition);
			playerOldCounter = player.counter;
			break;
		case 2:
			playerOldCounter = player.counter;
			if (commands [1].actTo.GetCaptionText () == "Boss") {
				commands [1].SetStatus (Command.Status.Correct);
				player.SetEndPosition (bossOutbox.playerPosition);
			} else if(commands[1].actTo.GetCaptionText() == "Nikita"){
				commands [1].SetStatus (Command.Status.Error);
				player.SetEndPosition (spyOutbox.playerPosition);
			}
			break;
		case 3:
			commands [1].SetStatus (Command.Status.Error);
			if (commands [1].actTo.GetCaptionText() == "0") {
				this.FailRunning ("I didn't receive the pulic yellow box.", bossOutbox.feedback);
			} else if (commands [1].actTo.GetCaptionText() == "1") {
				playerOldCounter = player.counter;
				player.SetEndPosition (pickupMemoryBoxPosition[1]);
			}
			break;
		}
	}

	void FailRunning(string message, GameObject feedback)
	{
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = "Error @ Line 0" + (this.playerCmd+1) +": " + message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(235,46,44,255);
		feedback.SetActive (true);

		isExecuting = false;
		ActivateCodingPanel (true);
	}

	void succeedRunning(string message, GameObject feedback)
	{
		Text feedbackText = bossOutbox.feedback.GetComponentInChildren<Text> ();
		feedbackText.text = "Great! " + message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(90,174,122,255);
		feedback.SetActive (true);

		isExecuting = false;
		isSucceeded = true;
	}

	void Reset()
	{
		commands[playerCmd].SetStatus(Command.Status.NotExecuting);

		isExecuting = true;

		ActivateCodingPanel (false);

		playerCmd = 0;
		spyCmd = 0;
		playerOldCounter = player.counter;
		spyOldCounter = spy.counter;
		countDown = 150;

		player.ResetAnimator ();
		spy.ResetAnimator ();

		for (int i = 1; i < numMemoryBoxes; i++) {
			memoryBoxes [i].transform.parent = null;
			memoryBoxes [i].transform.position = memoryBoxes [0].transform.position;
			memoryBoxes [i].SetActive (true);
		}

		bossOutbox.feedback.SetActive (false);
		spyOutbox.feedback.SetActive (false);
	}
		
	void Start () {
		isExecuting = false;
		isSucceeded = false;

		playerCmd = 0;
		spyCmd = 0;
		playerOldCounter = player.counter;
		spyOldCounter = spy.counter;

		Vector2[] inboxPositions = new Vector2[numPlayerBoxes];
		Vector2 pickupBoxLoc = new Vector2 (-293f, 42f);
		playerInbox = new Inbox (numPlayerBoxes, pickupBoxLoc, inboxPositions, playerBoxes);

		Vector2[] outboxPositions = new Vector2[1];
		outboxPositions [0] = new Vector2 (-81f, -50f);
		Vector2 dropBoxLoc = new Vector2 (-147f, -39f);
		bossOutbox = new Outbox (1, dropBoxLoc, outboxPositions, playerOutboxDialogue);

		Vector2[] inboxSPositions = new Vector2[numSpyBoxes];
		Vector2 pickupSBoxLoc = new Vector2 (-293f, -140f);
		spyInbox = new Inbox (numSpyBoxes, pickupSBoxLoc, inboxSPositions, spyBoxes);

		Vector2[] outboxSPositions = new Vector2[2];
		outboxSPositions [0] = new Vector2 (-92f, -227f);
		outboxSPositions [1] = new Vector2 (-50f, -227f);
		Vector2 dropBoxSLoc = new Vector2 (-151f, -211f);
		spyOutbox = new Outbox (1, dropBoxSLoc, outboxSPositions, spyOutboxDialogue);

		pickupMemoryBoxPosition [0] = new Vector2 (-225f, -39f);
		pickupMemoryBoxPosition [1] = new Vector2 (-166f, -39f);

		runButton.onClick.AddListener (new UnityAction (this.StartRunning));

		bossOutbox.feedback.SetActive (false);
		spyOutbox.feedback.SetActive (false);
	}

	void Update () {

		if (spyOldCounter != spy.counter) { //Spy movement is done.
			spyOldCounter = spy.counter;

			switch (spyCmd) {

			case 0:
				//pickup the box for spy
				memoryBoxes[1].transform.SetParent (spy.transform);
				memoryBoxes[1].transform.localPosition = new Vector2 (0f, -16f);

				spyCmd++;
				RunSpyAnimation ();
				break;
			case 1:
				memoryBoxes[1].transform.parent = null;
				memoryBoxes[1].transform.position = spyOutbox.boxesPosition [0];
				spy.SetEndPosition (spy.initPosition);
				break;

			}

		}

		if (playerOldCounter != player.counter) { //Player movement is done
			playerOldCounter = player.counter;

			switch (playerCmd){

			case 0:
				if (commands [0].optionMenu.value == 1) {
					this.FailRunning ("No box on the inbox belt.", bossOutbox.feedback);
				} else if (commands [0].optionMenu.value == 2) {
					if (commands [0].actTo.GetCaptionText () == "Boss") {
						this.FailRunning ("No box in your hands.", bossOutbox.feedback);
					} else if (commands [0].actTo.GetCaptionText () == "Nikita") {
						this.FailRunning ("No box in your hands.", spyOutbox.feedback);
					}
				} else if (commands[0].optionMenu.value == 3){
					if(commands [0].actTo.GetCaptionText () == "0") {
					memoryBoxes [2].transform.SetParent (player.transform);
					memoryBoxes [2].transform.localPosition = new Vector2 (0f, -16f);

					playerCmd++;
					RunSecondCommand ();
					}else if (commands[0].actTo.GetCaptionText() == "1"){
						this.FailRunning ("No box in this memory cell.", bossOutbox.feedback);
					}
				}
				break;

			case 1:
				if (commands [1].optionMenu.value == 1) {
					memoryBoxes [2].SetActive (false);
					this.FailRunning ("No box on the inbox belt.", bossOutbox.feedback);
				} else if (commands [1].optionMenu.value == 2) {
					if (commands [1].actTo.GetCaptionText () == "Boss") {
						memoryBoxes [2].transform.parent = null;
						memoryBoxes [2].transform.position = bossOutbox.boxesPosition [0];
						this.succeedRunning ("Thanks for your hard working.", bossOutbox.feedback);
					} else if (commands [1].actTo.GetCaptionText () == "Nikita") {
						// hacking: put two identical boxes on spy's track without synchronisation
						memoryBoxes [2].transform.parent = null;
						memoryBoxes [2].transform.position = spyOutbox.boxesPosition [1];
						this.FailRunning ("I didn't receive the pulic yellow box.", bossOutbox.feedback);
					}
				} else if (commands[1].optionMenu.value == 3){
					if(commands [1].actTo.GetCaptionText () == "1") {
						memoryBoxes [2].SetActive (false);
						this.FailRunning ("No box in this memory cell.", bossOutbox.feedback);
					}
				}
				break;
			
			}

		}

		if (isSucceeded) {
			if (countDown > 0) {
				countDown--;
			} else {
				SceneManager.LoadScene ("Challenge4");
			}

		}

	}
}