using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Storage<T> : MonoBehaviour where T: new() {

	// The data to store
	protected T data;

	// This includes the extensions and the starting slash. e.g., "/savedData.dat"
	public string path;

	public T GetAllData() {
		return data;
	}

	void Start () {
        //ResetAllData();
        CreatePathIfItDoesNotExist();
		Load();
	}

	protected void Save () {
		BinaryFormatter bf = new BinaryFormatter();

		// This overwrites the file if it exists.
		FileStream file = File.Create(Application.persistentDataPath + path);

		bf.Serialize(file, data);
		file.Close();
	}

	protected void CreatePathIfItDoesNotExist() {
		if (!File.Exists(Application.persistentDataPath + path)) {
			ResetAllData ();
		}
	}

	protected void Load () {
		if (File.Exists(Application.persistentDataPath + path)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
			data = (T) bf.Deserialize(file);
			file.Close();
		}
	}

	protected void ResetAllData () {
		data = new T ();
		Save ();
		Load ();
	}
}
