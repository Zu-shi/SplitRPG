using UnityEngine;
using System.Collections;

public class CameraScript : _Mono {

	public Vector2 center{get; set;}

	float moveSpeed;

	public bool isMoving{get;set;}

	void Start () {
		isMoving = false;

		moveSpeed = .2f;
	}
	
	void Update () {
		float cx = center.x;
		float cy = center.y;

		if(x == cx && y == cy){
			isMoving = false;
			return;
		} else {
			isMoving = true;
		}

		if(Utils.CloseValues(cx, x, moveSpeed+.01f)){
			x = cx;
		} else if (x < cx){
			x += moveSpeed;
		} else if (x > cx){
			x -= moveSpeed;
		}

		if(Utils.CloseValues(cy, y, moveSpeed+.01f)){
			y = cy;
		} else if (y < cy){
			y += moveSpeed;
		} else if (y > cy){
			y -= moveSpeed;
		}
	}
}
