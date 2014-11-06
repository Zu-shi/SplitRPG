using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class LineScript : _Mono {

	public void LineVisible(bool visible = true) {
		spriteRenderer.enabled = visible;
	}

	public void Update() {
		if(Globals.levelManager.currentLeftLevel == "SanctuaryLeft_NewTileset")
			spriteRenderer.enabled = false;
	}
}
