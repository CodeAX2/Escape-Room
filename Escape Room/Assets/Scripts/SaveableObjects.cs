using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class SaveableObjects : MonoBehaviour {

    private static SaveableObjects instance = null;

    private List<Saveable> savedObjects;

    private void Awake() {

        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public static SaveableObjects GetInstance() {
        if (instance != null) {
            return instance;
        } else {
            GameObject go = new GameObject("Object Saver");
            return go.AddComponent<SaveableObjects>();
        }
    }

    public void AddSaveable(Saveable objectToSave) {

        if (savedObjects == null) savedObjects = new List<Saveable>();

        savedObjects.Add(objectToSave);

    }

    public void Start() {

        for (int i = 0; i < savedObjects.Count; i++) {
            savedObjects[i].LoadFromFile();
        }

    }

    private void OnDestroy() {
        if (instance == this) {
            for (int i = 0; i < savedObjects.Count; i++) {
                savedObjects[i].SaveToFile();
            }
        }
    }



}