using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionBarScript : MonoBehaviour {
	float fillAmount = 0;
	public float fillThenEmptyDelay = 1f;
	public float indeterminateDelay = 1f;

	public int fps = 5;
	public Transform filledBarTransform;

	float delay;

	// Use this for initialization
	void Start () {
		delay = 1f / fps;
		filledBarTransform = this.gameObject.transform.GetChild(0).transform;
		SetBarToEmpty();
	}

	// time: how long it will take to fill the bar, in seconds
	// Begins a fill operation. Will reset the bar to start at zero
	// when called.
	public void Fill(float time) {
		CancelFillCoroutines();
		StartCoroutine("_FillBar", Math.Abs(time));
	}

	void CancelFillCoroutines() {
		StopCoroutine("_FillBar");
		StopCoroutine("_FillThenEmpty");
	}

	public void Empty() {
		CancelFillCoroutines();
		SetBarToEmpty();
	}

	void SetBarToEmpty() {
		filledBarTransform.localScale = new Vector3(0, 1, 1);
	}

	public void FillThenEmpty() {
		CancelFillCoroutines();

		StartCoroutine("_FillThenEmpty");
	}

	IEnumerator _FillThenEmpty() {
		// Check if the fill bar is too far away from being filled. If it is,
		// then we fill it for a moment before clearing it. This is mostly useful for
		// when the `fps` value is very low.
		if (filledBarTransform.localScale.x <= 0.97) {
			filledBarTransform.localScale = new Vector3(1, 1, 1);
			yield return new WaitForSeconds(fillThenEmptyDelay);
		}

    SetBarToEmpty();
		yield return null;
	}

	IEnumerator _FillBar(float totalTime) {
		// A bar _must_ start at zero, always.
		SetBarToEmpty();

		float startTime = Time.time;

		float localDeltaTime = 0;
		float lastUpdateTime = Time.time;
    for (float elapsedTime = 0; elapsedTime < totalTime; elapsedTime += localDeltaTime) {
			float naiveCompletionAmount = (Time.time - startTime) / totalTime;
			float completionAmount = Mathf.Min(1, naiveCompletionAmount);
    	filledBarTransform.localScale = new Vector3(completionAmount, 1, 1);

			lastUpdateTime = Time.time;

      yield return new WaitForSeconds(delay);
			// Calculate how long that `WaitForSeconds` actually took.
			localDeltaTime = Time.time - lastUpdateTime;
    }

		yield return new WaitForSeconds(indeterminateDelay);
		Debug.Log("OVERDUE...");
		yield return null;
	}
}
