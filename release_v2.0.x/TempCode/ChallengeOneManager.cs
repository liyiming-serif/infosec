using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeTwoManager: MonoBehaviour { 

	/*Constants*/
	public const int numCommands = 4;
	public const int numPlayerBoxes = 2;
	public const int numSpyBoxes = 1;

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
	public GameObject playerOutboxDialogue;
	public GameObject spyOutboxDialogue;

	/*game logic states*/
	public bool isExecuting;
	public bool isSucceeded;

	public int playerCmd;
	public int playerOldCounter;

	public int spyCmd;
	public int spyOldCounter;

	public int countdown;

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
			spyOldCounter = spy.counter;
			spy.SetEndPosition (spyInbox.playerPosition);
			break;
		case 1:
			spyOldCounter = spy.counter;
			spy.SetEndPosition (spyOutbox.playerPosition);
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
			commands [0].SetStatus (Command.Status.Correct);
			playerOldCounter = player.counter;
			player.SetEndPosition (playerInbox.playerPosition);
			break;
		case 2:
			playerOldCounter = player.counter;
			commands [0].SetStatus (Command.Status.Error);
			if (commands [0].actTo.GetCaptionText() == "Boss") {
				player.SetEndPosition (bossOutbox.playerPosition);
			} else if (commands [0].actTo.GetCaptionText() == "Nikita") {
				player.SetEndPosition (spyOutbox.playerPosition);
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
			playerInbox.boxesReference [0].SetActive (false);
			this.FailRunning ("You should have given the box to me.", bossOutbox.feedback);
			break;
		case 2:
			playerOldCounter = player.counter;
			if (commands [1].actTo.GetCaptionText() == "Boss") {
				commands [1].SetStatus (Command.Status.Correct);
				player.SetEndPosition (bossOutbox.playerPosition);
			} else if (commands [1].actTo.GetCaptionText() == "Nikita") {
				commands [1].SetStatus (Command.Status.Error);
				player.SetEndPosition (spyOutbox.playerPosition);
			}
			break;
		}
	}

	void RunThirdCommand()
	{
		commands [1].SetStatus (Command.Status.NotExecuting);
		switch (commands [2].optionMenu.value) {
		case 0:
			this.FailRunning ("Command is not selected.", bossOutbox.feedback);
			break;
		case 1:
			commands [2].SetStatus (Command.Status.Correct);
			playerOldCounter = player.counter;
			player.SetEndPosition (playerInbox.playerPosition); 
			break;
		case 2:
			commands [2].SetStatus (Command.Status.Error);
			if (commands [2].actTo.GetCaptionText () == "Boss") {
				this.FailRunning ("No box in your hands.", bossOutbox.feedback);
			} else if (commands [2].actTo.GetCaptionText () == "Nikita") {
				playerOldCounter = player.counter;
				player.SetEndPosition (spyOutbox.playerPosition);
			}
			break;
		}
	}

	void RunFourthCommand()
	{
		commands [2].SetStatus (Command.Status.NotExecuting);
		switch (commands [3].optionMenu.value) {
		case 0:
			this.FailRunning ("Command is not selected.", bossOutbox.feedback);
			break;
		case 1:
			commands [3].SetStatus (Command.Status.Error);
			playerInbox.boxesReference [1].SetActive (false);
			this.FailRunning ("You should have given the box to me.", spyOutbox.feedback);
			break;
		case 2:
			playerOldCounter = player.counter;
			if (commands [3].actTo.GetCaptionText () == "Boss") {
				commands [3].SetStatus (Command.Status.Error);
				player.SetEndPosition (bossOutbox.playerPosition);
			} else if (commands [3].actTo.GetCaptionText () == "Nikita") {
				commands [3].SetStatus (Command.Status.Correct);
				player.SetEndPosition (spyOutbox.playerPosition);
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
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
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

		player.ResetAnimator ();
		spy.ResetAnimator ();

		playerInbox.ResetBoxes ();
		spyInbox.ResetBoxes ();

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
		countdown = 150;

		Vector2[] inboxPositions = new Vector2[numPlayerBoxes];
		inboxPositions [0] = new Vector2 (-293f, -50f);
		inboxPositions [1] = new Vector2 (-333f, -50f);
		Vector2 pickupBoxLoc = new Vector2 (-293f, 42f);
		playerInbox = new Inbox (numPlayerBoxes, pickupBoxLoc, inboxPositions, playerBoxes);

		Vector2[] outboxPositions = new Vector2[2];
		outboxPositions [0] = new Vector2 (-81f, -50f);
		outboxPositions [1] = new Vector2 (-39f, -50f);
		Vector2 dropBoxLoc = new Vector2 (-147f, -39f);
		bossOutbox = new Outbox (1, dropBoxLoc, outboxPositions, playerOutboxDialogue);

		Vector2[] inboxSPositions = new Vector2[numSpyBoxes];
		inboxSPositions [0] = new Vector2 (-291f, -227f);
		Vector2 pickupSBoxLoc = new Vector2 (-293f, -140f);
		spyInbox = new Inbox (numSpyBoxes, pickupSBoxLoc, inboxSPositions, spyBoxes);

		Vector2[] outboxSPositions = new Vector2[2];
		outboxSPositions [0] = new Vector2 (-92f, -227f);
		outboxSPositions [1] = new Vector2 (-50f, -227f);
		Vector2 dropBoxSLoc = new Vector2 (-151f, -211f);
		spyOutbox = new Outbox (1, dropBoxSLoc, outboxSPositions, spyOutboxDialogue);

		runButton.onClick.AddListener (new UnityAction (this.StartRunning));

		bossOutbox.feedback.SetActive (false);
		spyOutbox.feedback.SetActive (false);
	}

	void Update () {

		if (spyOldCounter != spy.counter) { //Spy movement is done.
			spyOldCounter = spy.counter;

			switch (spyCmd) {

			case 0:
				//pickup the first box
				spyInbox.boxesReference [0].transform.SetParent (spy.transform);
				spyInbox.boxesReference [0].transform.localPosition = new Vector2 (0f, -16f);

				spyCmd++;
				RunSpyAnimation ();
				break;
			case 1:
				//dropdown the first box then return
				spyInbox.boxesReference [0].transform.parent = null;
				spyInbox.boxesReference [0].transform.position = spyOutbox.boxesPosition [0];
				spyCmd++;
				spy.SetEndPosition (spy.initPosition);
				break;

			}

		}

		if (playerOldCounter != player.counter) { //Player movement is done
			playerOldCounter = player.counter;

			switch (playerCmd){

			case 0:
				if (commands [0].optionMenu.value == 1) {
					// pickup the first box
					playerInbox.boxesReference [0].transform.SetParent (player.transform);
					playerInbox.boxesReference [0].transform.localPosition = new Vector2 (0f,-16f);
					playerInbox.boxesReference [1].transform.position = playerInbox.boxesPosition [0];
					playerCmd++;
					RunSecondCommand ();
				} else if (commands [0].optionMenu.value == 2) {
					if (commands [0].actTo.GetCaptionText() == "Boss") {
						this.FailRunning ("No box in your hands.", bossOutbox.feedback);
					} else if (commands [0].actTo.GetCaptionText() == "Nikita") {
						this.FailRunning ("No box in your hands.", spyOutbox.feedback);
					}
				}
				break;

			case 1:
				if (commands [1].optionMenu.value == 2){
					playerInbox.boxesReference [0].transform.parent = null;
					if (commands [1].actTo.GetCaptionText() == "Boss") {
						playerInbox.boxesReference [0].transform.position = bossOutbox.boxesPosition [0];
						playerCmd++;
						RunThirdCommand ();
					} else if (commands [1].actTo.GetCaptionText() == "Nikita") {
						spyInbox.boxesReference [0].transform.position = spyOutbox.boxesPosition [1];
						playerInbox.boxesReference [0].transform.position = spyOutbox.boxesPosition [0];
						this.FailRunning ("Aha! Your boss has a private box 'E'!", spyOutbox.feedback);
					}

				}
				break;
			
			case 2:
				if (commands [2].optionMenu.value == 1) {
					//pickup the second box
					playerInbox.boxesReference [1].transform.SetParent (player.transform);
					playerInbox.boxesReference [1].transform.localPosition = new Vector2 (0f, -16f);
					playerCmd++;
					RunFourthCommand ();
				} else if (commands [2].optionMenu.value == 2) {
					this.FailRunning ("No box in your hands.", spyOutbox.feedback);
				}
				break;

			case 3:
				if (commands [3].optionMenu.value == 2) {
					playerInbox.boxesReference [1].transform.parent = null;
					if (commands [3].actTo.GetCaptionText () == "Boss") {
						playerInbox.boxesReference [0].transform.position = bossOutbox.boxesPosition [1];
						playerInbox.boxesReference [1].transform.position = bossOutbox.boxesPosition [0];
						this.FailRunning ("This is Nikita's private box that I shoudn't know.", bossOutbox.feedback);
					} else if (commands [3].actTo.GetCaptionText () == "Nikita") {
						spyInbox.boxesReference [0].transform.position = spyOutbox.boxesPosition [1];
						playerInbox.boxesReference [1].transform.position = spyOutbox.boxesPosition [0];
						this.succeedRunning ("Thanks for your hard working!", spyOutbox.feedback);
					}
				}
				break;

			}

		}

		if (isSucceeded) {
			if (countdown > 0) {
				countdown--;
			}else{
				SceneManager.LoadScene ("Challenge3");
			}
		}

	}
}