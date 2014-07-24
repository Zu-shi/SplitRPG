using UnityEngine;
using System.Collections;

public class HeightScript : _Mono {

	public DrawingOrder drawingOrder;
	public int height;

	void Start () {
		Globals.heightSorter.AddObject(this);
	}
}
