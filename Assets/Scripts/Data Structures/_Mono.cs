using UnityEngine;
using System.Collections;

public class _Mono : MonoBehaviour {

	protected SpriteRenderer _spriteRenderer;

	public float x {
		set {
			transform.position = new Vector3 (value, transform.position.y, transform.position.z);
		}
		get {
			return transform.position.x;
		}
	}

	public float y {
		set {
			transform.position = new Vector3 (transform.position.x, value, transform.position.z);
		}
		get {
			return transform.position.y;
		}
	}

	public float z {
		set {
			transform.position = new Vector3 (transform.position.x, transform.position.y, value);
		}
		get {
			return transform.position.z;
		}
	}

	public int tileX {
		set {
			x = value;
		}
		get {
			return (int)Mathf.Round(x);
		}
	}

	public int tileY {
		set {
			y = value;
		}
		get {
			return (int)Mathf.Round(y);
		}
	}


	public float xs {
		set {
			transform.localScale = new Vector3 (value, transform.localScale.y, transform.localScale.z);
		}
		get {
			return transform.localScale.x;
		}
	}

	public float ys {
		set {
			transform.localScale = new Vector3 (transform.localScale.x, value, transform.localScale.z);
		}
		get {
			return transform.localScale.y;
		}
	}

	public float angle {
		set {
			//transform.rotation = Quaternion.AngleAxis(value % 360, Vector3.forward);
			Quaternion rotation = Quaternion.identity;
			rotation.eulerAngles = new Vector3(0, 0, value);
			transform.rotation = rotation;
		}
		get {
			return transform.rotation.eulerAngles.z;
		}
	}

	public float alpha {
		set {
			if(spriteRenderer != null){
				Color _color = spriteRenderer.color;
				spriteRenderer.color = new Color(_color.r, _color.g, _color.b, value); 
			}
		}
		get {
			if(spriteRenderer != null){
				return spriteRenderer.color.a;
			}
			else return 0;
		}
	}

	public float guiAlpha {
		set {
			if(guiTexture != null){
				Color _color = guiTexture.color;
				guiTexture.color = new Color(_color.r, _color.g, _color.b, value); 
			}
		}
		get {
			if(guiTexture != null){
				return guiTexture.color.a;
			}
			else return 0;
		}
	}

	public SpriteRenderer spriteRenderer{
		get{
			if(_spriteRenderer == null){
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}
			return _spriteRenderer;
		}
	}

	public void Destroy(){
		Destroy (this.gameObject);
	}


}
