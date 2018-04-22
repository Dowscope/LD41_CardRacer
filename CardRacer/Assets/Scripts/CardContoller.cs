using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContoller : MonoBehaviour {

	// Get reference to the game controller script
	GameContoller GC;

	// Need references to the hands
	GameObject Hand;

	// Get a reference to if this the player card or computer card.
	Transform parent;

	// Get a reference to the sprite
	SpriteRenderer CardSprite;

	// Get a list of images
	public Sprite FaceDownSprite;
	public List<Sprite> sprites = new List<Sprite>();

	// Initialize variables
	bool IsFaceDown = true;

	public int CardValue {
		get;
		protected set;
	}

	// For moving purposes
	bool isMoving = false;
	float MovingSpeed = 4f;

	// Use this for initialization
	void Start () {
		
		// Initialize the references
		parent = transform.parent;

		CardSprite = transform.GetChild (0).GetComponent<SpriteRenderer> ();
		CardSprite.sprite = FaceDownSprite;

		GC = GameObject.Find ("GameController").GetComponent<GameContoller> ();

		// Set the Hand
		if (parent.name == "PlayerDeck") {
			Hand = GameObject.Find ("PlayerHand");
		} else {
			Hand = GameObject.Find ("ComputerHand");
		}

	}

	// Update is called once per frame
	void Update () {
		if (isMoving) {
			transform.position = Vector3.Lerp (transform.position, 
				Hand.transform.position, MovingSpeed * Time.deltaTime);
			if (transform.position.Equals (Hand.transform.position)) {
				isMoving = false;
			}
		}
	}

	void OnMouseDown() {
		if (!GC.isRaceOver) {
			// Flip Card over
			Flip ();
		}
	}

	public void Flip() {
		// Procedure for when the card is face down
		if (IsFaceDown) {
			// Before we move the card, delete any cards that are there
			if (Hand.transform.childCount != 0) {
				Destroy (Hand.transform.GetChild (0).gameObject);
			}

			// Move the card to the hand and change position on screen
			transform.SetParent (Hand.transform);
			isMoving = true;

			// Will randomally choose a card sprite to place and store 
			// for later use.
			CardValue = Random.Range (0, 13);
			CardSprite.sprite = sprites [CardValue];

			if (Hand.name == "PlayerHand") {
				GC.HasPlayerMoved = true;
				GC.PlayerCard_Value = CardValue;
			} else {
				GC.ComputerCard_Value = CardValue;
			}
		}
	}
}
