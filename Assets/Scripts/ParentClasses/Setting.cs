using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Setting<T> : Storage<T> where T: new() {

	public T settingsData {
		get {
			return data;
		}
		set {
			data = value;
			SendDataChangedEvent ();
		}
	}

	protected void SendDataChangedEvent() {
		if (OnDataChange != null)
			OnDataChange (data);
	}

	public event Action<T> OnDataChange; 

}
