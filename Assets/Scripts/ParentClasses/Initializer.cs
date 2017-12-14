using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer<T> : MonoBehaviour where T : new(){

	[SerializeField] string settingTag;
	protected Setting<T> setting;

	protected virtual void OnEnable() {
		if (setting==null) {
			setting = GameObject.FindWithTag (settingTag).GetComponent< Setting<T> >();
		} 
			
		ConfigureSettings (setting.settingsData);

		// dat is the data passed from the Setting. See Setting.cs
		setting.OnDataChange += ConfigureSettings;
	}

	protected virtual void OnDisable() {
		if (setting == null)
			return;
		setting.OnDataChange -= ConfigureSettings;
	}

	protected virtual void ConfigureSettings(T data) {
		// Method overwritten in children to configure settings. 
		return;
	}

}
