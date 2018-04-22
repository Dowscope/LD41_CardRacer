using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContoller : MonoBehaviour {

	// Grab prefab of the face down cards
	public GameObject Card_FaceDownPreFab;
	public GameObject PlayerCarPreFab;
	public GameObject ComputerCarPreFab;

	// Initialize some variables mechanics
	// How many cards should the players start with. Start with a normal 52
	// cards in a deck.  So each player will get half.
	int NumOfCards_Start = 26;
	int PlayerCards_Remaining;
	int ComputerCards_Remaining;
	int StartLine = -7;
	int FinishLine = 6;
	int Winner = -1;

	// This is the flag for when the race is OveR
	public bool isRaceOver {
		get;
		protected set;
	}

	// Get references to card spot objects
	GameObject CardSpots; 
	GameObject RaceTrack;
	Transform PlayerDeck;
	Transform ComputerDeck;

	// Get references to the Cars
	GameObject PlayerCar;
	GameObject ComputerCar;
	bool ComputerCarIsMoving = false;
	bool PlayerCarIsMoving = false;
	Vector3 PlayerDest;
	Vector3 ComputerDest;
	float CarSpeed = 4f;

	// CallBacks from Cards
	public bool HasPlayerMoved {
		get;
		set;
	}

	public int PlayerCard_Value {
		get;
		set;
	}

	public int ComputerCard_Value {
		get;
		set;
	}

	// Use this for initialization
	void Start () {
		// Initialize the variables
		PlayerCards_Remaining = NumOfCards_Start;
		ComputerCards_Remaining = NumOfCards_Start;

		// Initialize the references
		// NOTE: This may not work if the cardspots start after this object.
		CardSpots = GameObject.Find ("CardSpots");
		RaceTrack = GameObject.Find ("RaceTrack");
		PlayerDeck = CardSpots.transform.GetChild (0);
		ComputerDeck = CardSpots.transform.GetChild (1);

		// Setup Cars
		SetupCar (0);
		SetupCar (1);

		// Setup the card decks
		SetupDecks (0);
		SetupDecks (1);

		HasPlayerMoved = false;
		PlayerCard_Value = 0;
		ComputerCard_Value = 0;
		isRaceOver = false;
	}
	
	// Update is called once per frame
	void Update () {

		// Check if there is a winner first
		if (Winner != -1) {
			WeHaveAWinner ();
		}

		if (PlayerCarIsMoving) {
			PlayerCar.transform.position = Vector3.Lerp (PlayerCar.transform.position,
				PlayerDest, CarSpeed * Time.deltaTime);
			if (PlayerCar.transform.position.Equals (PlayerDest)) {
				PlayerCarIsMoving = false;
			}
		}

		if (ComputerCarIsMoving) {
			ComputerCar.transform.position = Vector3.Lerp (ComputerCar.transform.position,
				ComputerDest, CarSpeed * Time.deltaTime);
			if (ComputerCar.transform.position.Equals (ComputerDest)) {
				ComputerCarIsMoving = false;
			}
		}

		// Check if a card has been taken from the Deck
		if (PlayerDeck.childCount == 0) {
			// Check if there if player has remaining cards left
			if (PlayerCards_Remaining > 0) {
				// Now we add a new card.
				SetupDecks(0);
			}
		}

		if (ComputerDeck.childCount == 0) {
			if (ComputerCards_Remaining > 0) {
				SetupDecks (1);
			}
		}
			
		if (HasPlayerMoved) {
			if (ComputerDeck.childCount != 0) {
				ComputerDeck.GetChild (0).GetComponent<CardContoller> ().Flip ();
				HasPlayerMoved = false;
			}

			// Check who had the higher value
			if (PlayerCard_Value > ComputerCard_Value) {
				// Move the player car ahead by the difference
				int moveAmount = PlayerCard_Value - ComputerCard_Value;
				MoveCar (0, moveAmount);
			} else if (ComputerCard_Value > PlayerCard_Value) {
				// Move the computer car ahead by the difference
				int moveAmount = ComputerCard_Value - PlayerCard_Value;
				MoveCar (1, moveAmount);
			} else {

			}
		}
	}

	void SetupDecks(int side) {
		if (side == 0) {
			// This is the player deck
			GameObject player_go = Instantiate (Card_FaceDownPreFab, PlayerDeck);
			player_go.name = "PlayerCard";
			player_go.transform.position = CardSpots.transform.GetChild (0).position;
			PlayerCards_Remaining -= 1;
		} else {
			// This is the computer deck
			GameObject computer_go = Instantiate (Card_FaceDownPreFab, CardSpots.transform.GetChild (1));
			computer_go.transform.position = CardSpots.transform.GetChild (1).position;
			computer_go.name = "ComputerCard";
			ComputerCards_Remaining -= 1;
		}
	}

	void SetupCar(int side) {
		if (side == 0) {
			// This is the player car
			PlayerCar = Instantiate (PlayerCarPreFab, RaceTrack.transform, true);
			PlayerCar.name = "PlayerCar";
		} else {
			// This is the computer deck
			ComputerCar = Instantiate (ComputerCarPreFab, RaceTrack.transform, true);
			ComputerCar.name = "ComputerCard";
		}
	}

	void MoveCar(int side, int amount) {
		// We are going to take the amount of difference and use that 
		// as a scale for movement.
		int moveAmount;

		if (amount == 13) {
			moveAmount = 5;
		} else if (amount > 6) {
			moveAmount = 2;
		} else {
			moveAmount = 1;
		}

		if (side == 0) {
			// This is the player car
			Vector3 pos = PlayerCar.transform.position;
			pos.x += moveAmount;
			PlayerDest = pos;

			if (pos.x >= FinishLine) {
				Winner = 0;
			}

			PlayerCarIsMoving = true;
		} else {
			// This is the computer deck
			Vector3 pos = ComputerCar.transform.position;
			pos.x += moveAmount;
			ComputerDest = pos;

			if (pos.x >= FinishLine) {
				Winner = 1;
			}

			ComputerCarIsMoving = true;
		}
	}

	void WeHaveAWinner() {
		isRaceOver = true;
	}
}
