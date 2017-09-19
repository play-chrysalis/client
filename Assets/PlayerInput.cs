using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	Transform actionBar;
	ActionBarScript actionBarScript;
	bool canWalk = true;
	float clientWalkThrottleTime = 1f;


	// Use this for initialization
	void Start() {
		actionBar = this.gameObject.transform.GetChild(0);
		actionBarScript = actionBar.gameObject.GetComponent<ActionBarScript>();
	}
	
	// Update is called once per frame
	void Update() {
		checkForActionInput();
	}

	void checkForActionInput() {
		if (canWalk) {
			float horizontalMovement = Input.GetAxisRaw("Horizontal");
			if (horizontalMovement != 0) {
				beginMovement("Horizontal", horizontalMovement);
				return;
			}

			float verticalMovement = Input.GetAxisRaw("Vertical");
			if (verticalMovement != 0) {
				beginMovement("Vertical", verticalMovement);
				return;
			}
		}

    if (Input.GetKeyDown(KeyCode.Space)) {
      BeginAssail();
			return;
    }
	}

	void beginMovement(string axis, float direction) {
		StartCoroutine(Walk(axis, direction));
	}

	IEnumerator Walk(string axis, float direction) {
		CancelCurrentAction();
		canWalk = false;
		if (axis == "Horizontal") {
			if (direction == 1) {
				Debug.Log("Going right");
			} else if (direction == -1) {
				Debug.Log("Going left");
			}
		} else if (axis == "Vertical") {
			if (direction == 1) {
				Debug.Log("Going up");
			} else if (direction == -1) {
				Debug.Log("Going down");
			}
		}
		yield return new WaitForSeconds(clientWalkThrottleTime);
		canWalk = true;
	}

	void CancelCurrentAction() {
		actionBarScript.Empty();
	}

	void BeginAssail() {
		// Tell the server that we intend to use "Assail."
		StartCoroutine("SubmitActionToServer", "Assail");
		// Begin providing feedback to the user that the Assail is in progress.
		actionBarScript.Fill(1.5f);
	}

	IEnumerator SubmitActionToServer(string type) {
		Debug.Log("Sending TCP packet with type: '" + type + "'");
		yield return new WaitForSeconds(1.6f);
		Debug.Log("Server responded: assailing!");
		actionBarScript.FillThenEmpty();

		yield return null;
	}
}
