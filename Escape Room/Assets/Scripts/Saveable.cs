using UnityEngine;
public abstract class Saveable : MonoBehaviour {
    public abstract void SaveToFile();
    public abstract void LoadFromFile();
    protected string fileName;

    protected void Awake() {

        SaveableObjects.GetInstance().AddSaveable(this);
        fileName = ToString() + ".json";

    }

}