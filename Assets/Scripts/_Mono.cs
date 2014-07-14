using UnityEngine;
using System.Collections;

public class _Mono : MonoBehaviour {

	protected SpriteRenderer _spriteRenderer;

	public float x {
		set {
			transform.position = new Vector2 (value, transform.position.y);
		}
		get {
			return transform.position.x;
		}
	}

	public float y {
		set {
			transform.position = new Vector2 (transform.position.x, value);
		}
		get {
			return transform.position.y;
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
			transform.localScale = new Vector2 (value, transform.localScale.y);
		}
		get {
			return transform.localScale.x;
		}
	}

	public float ys {
		set {
			transform.localScale = new Vector2 (transform.localScale.x, value);
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
