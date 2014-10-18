using UnityEngine;
using System.Collections;

/// <summary>
/// Provides functionality for scrolling tiled background textures
/// </summary>
[ExecuteInEditMode]
public class BackgroundScrollingTextureScript : _Mono {

	public float parralaxDepth = 1;
	//Transparency of image
	public float alpha = 1;
	public Vector2 passiveMove;
	public Vector2 defaultOffset;
	public Vector2 lockToCameraOffset;
	public bool lockToCamera = true;
	public bool lockVerticalOffset = false;
	public bool runInEditor = false;

	bool started = false;
	Vector3 lastCameraPosition;
	
	protected virtual void Update () {

		if(!Application.isPlaying && !runInEditor){
			return;
		}

		if(renderer.sharedMaterial.HasProperty("_Color")){
			Color c = renderer.sharedMaterial.GetColor("_Color");
			renderer.sharedMaterial.color = new Color(c.r, c.g, c.b, alpha);
		}

		CameraScript cameraScript = Globals.cameraLeft;
		if(Utils.LayerIs(gameObject.layer, "Right")){
			cameraScript = Globals.cameraRight;
		}
		
		if(!started){
			if(lockToCamera)
				xy = cameraScript.xy + lockToCameraOffset;
			lastCameraPosition = cameraScript.transform.position;
			started = true;
		}

		if(cameraScript.transform.position != lastCameraPosition){
			Vector3 diff = cameraScript.transform.position - lastCameraPosition;

			// because the rect is moving with the camera, we need to add 2 to parralaxDepth to counteract
			// divide by orthoSize because the rect will be about that size... otherwise
			//    the texture will zoom around way too fast
			float fac = 1 / (2 + parralaxDepth) / cameraScript.camera.orthographicSize;

			// offset the texture
			renderer.sharedMaterial.mainTextureOffset += fac * new Vector2(diff.x, diff.y);

			if(lockToCamera)
				xy = cameraScript.xy + lockToCameraOffset; // keep in front of the camera
		}

		renderer.sharedMaterial.mainTextureOffset += Time.deltaTime * passiveMove;

		if(lockVerticalOffset){
			Vector2 offset = renderer.sharedMaterial.mainTextureOffset;
			renderer.sharedMaterial.mainTextureOffset = new Vector2(offset.x, 0);
		}
		
		lastCameraPosition = cameraScript.transform.position;
	}

	protected void OnApplicationQuit(){
		renderer.sharedMaterial.mainTextureOffset = defaultOffset;
	}
}
