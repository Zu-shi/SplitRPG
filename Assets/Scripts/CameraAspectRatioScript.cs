using UnityEngine;
using System.Collections;

public class CameraAspectRatioScript : MonoBehaviour {

	[Tooltip("The aspect ratio for each camera. Note that this is the PER CAMERA aspect ratio, not the total aspect ratio.")]
	public float targetAspectRatio = 9.0f / 5.0f;

	private Camera left, right;

	// TODO: Add the specail camera here.

	void Start () {
		left = Globals.cameraLeft.GetComponent<Camera>();
		right = Globals.cameraRight.GetComponent<Camera>();
	}

	public void Update() {
		// determine the game window's current aspect ratio
		float windowAspectRatio = (float)Screen.width / (float)Screen.height;
		
		// current viewport height should be scaled by this amount
		float scaleHeight = windowAspectRatio / targetAspectRatio;
		
		// if scaled height is less than current height, add letterbox
		if (scaleHeight < 1.0f)
		{
			Rect leftRect = left.rect;
			
			leftRect.width = 0.5f;
			leftRect.height = scaleHeight;
			leftRect.x = 0;
			leftRect.y = (1.0f - scaleHeight) / 2.0f;
			
			left.rect = leftRect;
			
			Rect rightRect = right.rect;
			
			rightRect.width = 0.5f;
			rightRect.height = scaleHeight;
			rightRect.x = 0.5f;
			rightRect.y = (1.0f - scaleHeight) / 2.0f;
			
			right.rect = rightRect;

			Rect specialRect = new Rect(0,leftRect.yMin,1,rightRect.height);
			Globals.gameManager.transform.Find("LineCamera").GetComponent<Camera>().rect = specialRect;
			Globals.gameManager.transform.Find("CameraSpecial").GetComponent<Camera>().rect = specialRect;

		}
		else // add pillarbox
		{
			float scaleWidth = 1.0f / scaleHeight;
			
			Rect leftRect = left.rect;
			
			leftRect.width = scaleWidth;
			leftRect.height = 1.0f;
			leftRect.x = (1.0f - 2 * scaleWidth) / 2.0f;
			leftRect.y = 0;
			
			left.rect = leftRect;
			
			Rect rightRect = right.rect;
			
			rightRect.width = scaleWidth;
			rightRect.height = 1.0f;
			rightRect.x = 0.5f;
			rightRect.y = 0;
			
			right.rect = rightRect;

			Rect specialRect = new Rect(rightRect.xMin,0,leftRect.width * 2,1);
			Globals.gameManager.transform.Find("LineCamera").GetComponent<Camera>().rect = specialRect;
			Globals.gameManager.transform.Find("CameraSpecial").GetComponent<Camera>().rect = specialRect;

		}
	}
}
