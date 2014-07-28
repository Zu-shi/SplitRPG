using UnityEngine;
using System.Collections;

public abstract class Action : _Mono {

	bool _started;
	public bool started{get{return _started;}}

	Utils.VoidDelegate callbacks;

	public Action(){
		_started = false;
		callbacks = null;
	}

	public Action StartAction(){
		_started = true;
		OnStartAction();
		return this;
	}

	public virtual void OnStartAction(){}

	public void AddDelegate(Utils.VoidDelegate d){
		callbacks += d;
	}
	
	public void Finish(){
		if(callbacks != null)
			callbacks();
		GameObject.Destroy(gameObject);
	}
}
