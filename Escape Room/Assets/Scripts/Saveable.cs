using UnityEngine;
public abstract class Saveable : MonoBehaviour {
    public abstract void SaveToFile();
    public abstract void LoadFromFile();

    protected string fileName = null;

    protected void Awake() {

        SaveableObjects.GetInstance().AddSaveable(this);

    }

    protected string GetFileName() {
        if (fileName == null) fileName = GameController.GetInstance().GetSaveFolder() + "\\" + ToString() + ".json";
        return fileName;
    }

}