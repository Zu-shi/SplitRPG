using UnityEngine;
using System.Collections;

// To use this class, override ActionSequence with a series of calls of methods in this class.
public abstract class CutsceneScript : _Mono {
	protected GameObject leftPlayer, rightPlayer;

	public bool triggered = false;

	public void Start() {
		leftPlayer = Globals.playerLeft.gameObject;
		rightPlayer = Globals.playerRight.gameObject;
	}

	public void Update() {
		if(triggered)
			return;

		if(Globals.collisionManager.IsPlayerOnTile(tileVector, gameObject.layer)) {
			triggered = true;
			Begin();
		}
	}

	protected Utils.VoidDelegate callback;

	public virtual IEnumerator Begin(Utils.VoidDelegate callback = null) {
		this.callback = callback;
		Globals.gameManager.GetComponent<PlayerInputScript>().enabled = false;

		yield return StartCoroutine(DumbActionSequence());
	}

	private IEnumerator DumbActionSequence() {
		ActionSequence();
		yield return 0;
	}

	protected abstract void ActionSequence();

	protected float Move(GameObject mover, Direction dir, int numTiles = 1) {
		MovementScript ms = mover.GetComponent<MovementScript>();
		if(ms == null) {
			return 0.0f;
		}

		StartCoroutine(MoveCoroutine(ms, dir, numTiles));
		return numTiles * 14 * (1.0f/50);
		// ^^ numTiles * numFramesPerMove * lengthOfFrameInSeconds ^^
	}

	private IEnumerator MoveCoroutine(MovementScript ms, Direction dir, int numTiles) {
		while(numTiles > 0) {
			if(ms.isMoving) {
				yield return null;
				continue;
			}
			ms.MoveInDirection(dir);
			numTiles--;
		}
		yield return 0;
	}

	// This should be the only blocking method.
	protected void Wait(float timeInSeconds) {
		DumbWait(timeInSeconds);
	}

	private IEnumerator DumbWait(float timeInSeconds) {
		Debug.Log("Starting dumb wait.");
		yield return new WaitForSeconds(timeInSeconds);
	}


	protected void End() {
		Globals.gameManager.GetComponent<PlayerInputScript>().enabled = true;
		if(callback != null) {
			callback();
		}
	}
}
