using UnityEngine;
using System.Collections.Generic;

public class FireManagerScript : MonoBehaviour {

	public List<GameObject> fire1 = new List<GameObject>();
	public List<GameObject> fire2 = new List<GameObject>();
	public List<GameObject> fire3 = new List<GameObject>();

	private int fireLevel = 3;

	public void PutOutFires() {
		switch(fireLevel) {
		case 3:
			foreach(GameObject fire in fire1)
				fire.SetActive(false);
			fireLevel = 2;
			break;
		case 2:
			foreach(GameObject fire in fire2)
				fire.SetActive(false);
			fireLevel = 1;
			break;
		case 1:
			foreach(GameObject fire in fire3)
				fire.SetActive(false);
			fireLevel = 0;
			break;
		}
	}
}
