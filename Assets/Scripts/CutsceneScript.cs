using UnityEngine;
using System.Collections;

// To use this class, override ActionSequence with a series of calls of methods in this class.
/// <summary>
/// Base class for all cutscenes. Clients should override the ActionSequence method with the events
/// of the cutscene, using the other methods of this class.
/// Note that this class caches references to both player characters and both cameras for ease of use.
/// 
/// By default, cutscenes are triggered when a player collides with the cutscene's tile. Override the
/// update function to disable or change this behavior.
/// </summary>
/// <author>Tyler Wallace</author>
public abstract class CutsceneScript : _Mono {

	protected GameObject			leftPlayer, rightPlayer;
	protected CameraScript			leftCamera, rightCamera;
	protected Utils.VoidDelegate	callback;

	/// <summary>
	/// Has this cutscene been triggered yet?
	/// </summary>
	public bool triggered = false;

	public void Start() {
		leftPlayer = Globals.playerLeft.gameObject;
		rightPlayer = Globals.playerRight.gameObject;
		leftCamera = Globals.cameraLeft;
		rightCamera = Globals.cameraRight;
	}
	
	public virtual void Update() {
		if(triggered)
			return;

		if(Globals.collisionManager.IsPlayerOnTile(tileVector, gameObject.layer)) {
			triggered = true;
			Begin();
		}
	}

	/// <summary>
	/// Begin the cutscene and store the specified callback.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public virtual void Begin(Utils.VoidDelegate callback = null) {
		this.callback = callback;
		Globals.gameManager.GetComponent<PlayerInputScript>().enabled = false;

		StartCoroutine("ActionSequence");
	}

	/// <summary>
	/// The meat of the cutscene. Derived classes implement this to determine what happens in the cutscene.
	/// The implementation of this should rely heavily on the other methods of this class, namely:
	/// 	Move,
	/// 	FadeCamOut,
	/// 	FadeCamIn,
	/// 	ShowSpeechBubble (TODO),
	/// 	HideSpeechBubble (TODO)
	/// 
	/// IMPORTANT: The End() needs to be called at the end of the sequence, else clean up code and callbacks
	/// will not be run.
	/// </summary>
	/// <returns>The sequence.</returns>
	protected abstract IEnumerator ActionSequence();

	/// <summary>
	/// Move the specified mover in the direction dir for numTiles.
	/// If numTiles = 0 and the mover is a character, the mover will face dir.
	/// </summary>
	/// <param name="mover">Mover.</param>
	/// <param name="dir">Dir.</param>
	/// <param name="numTiles">Number tiles.</param>
	/// <returns>The amount of time it will take to finish this movement.</returns>
	protected float Move(GameObject mover, Direction dir, int numTiles = 1) {
		MovementScript ms = mover.GetComponent<MovementScript>();
		if(ms == null || dir == Direction.NONE) {
			return 0.0f;
		}

		if(numTiles == 0) {
			// Can we face in the given direction? Only characters do that.
			if(ms.GetType() == typeof(CharacterMovementScript)) {
				CharacterMovementScript cms = ms as CharacterMovementScript;
				StartCoroutine(ChangeDirectionCoroutine(cms, dir));
			}
			return 0.0f; // Not exactly true, but close enough.
		}

		StartCoroutine(MoveCoroutine(ms, dir, numTiles));
		return numTiles * 14 * (1.0f/50);
		// ^^ numTiles * numFramesPerMove * lengthOfFrameInSeconds ^^
	}

	protected void PlayAnimation(GameObject player, string animation, int startIndex = 0) {
		SpriteAnimationManagerScript sams = player.GetComponent<SpriteAnimationManagerScript>();
		if(sams == null) {
			Debug.LogError("Object " + player.name + " has no SpriteAnimationManagerScript.");
			return;
		}
		sams.PlayAnimation(animation, 0);
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
	}

	private IEnumerator ChangeDirectionCoroutine(CharacterMovementScript cms, Direction dir) {
		while(cms.isMoving || cms.isChangingDirection) {
			yield return null;
		}
		cms.ChangeDirection(dir);
	}

	/// <summary>
	/// End the cutscene with any clean up code and call the callback.
	/// </summary>
	protected void End() {
		Globals.gameManager.GetComponent<PlayerInputScript>().enabled = true;
		if(callback != null) {
			callback();
		}
	}

	/// <summary>
	/// Fades the passed camera out.
	/// </summary>
	/// <returns>Estimated time this fade out will take.</returns>
	/// <param name="cam">Cam.</param>
	protected float FadeCameraOut(CameraScript cam) {
		cam.BeginFadeDown();
		return cam.fader.EstimateTime();
	}

	/// <summary>
	/// Fades the passed camera in.
	/// </summary>
	/// <returns>Estimated time this fade in will take.</returns>
	/// <param name="cam">Cam.</param>
	protected float FadeCameraIn(CameraScript cam) {
		cam.BeginFadeUp();
		return cam.fader.EstimateTime();
	}

	protected GameObject ShowSpeechBubble(GameObject speaker, GameObject bubblePrefab) {
		if(speaker == null) {
			Debug.LogError("Cannot show bubble without a speaker.");
			return null;
		}
		return ShowSpeechBubble(speaker.transform.position, bubblePrefab);
	}

	protected GameObject ShowSpeechBubble(Vector3 pos, GameObject bubblePrefab) {
		if(bubblePrefab == null) {
			Debug.LogError("Cannot show null bubble prefab.");
			return null;
		}
		GameObject bubbleInstance = Instantiate(bubblePrefab, pos, Quaternion.identity) as GameObject;
		bubbleInstance.name = bubblePrefab.name;
		return bubbleInstance;
	}

	protected void HideSpeechBubble(GameObject bubbleInstance) {
		if(bubbleInstance == null) {
			Debug.LogError("Cannot hide null bubble instance.");
			return;
		}
		Destroy(bubbleInstance);
	}
	
}
