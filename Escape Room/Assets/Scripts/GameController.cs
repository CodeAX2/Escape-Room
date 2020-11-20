using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class GameController : Saveable {

    private static GameController gameController;

    public GameObject pickUpParticleEffect;
    public string saveFolder;
    public TextMeshProUGUI countdownText;
    public Color solvedCountdownColor, runningCountdownColor;
    public float timeForSolve;

    public TextMeshPro officerText;

    private bool timerRunning = true;
    public string nextSceneName;

    private new void Awake() {
        base.Awake();
        if (gameController == null) {
            gameController = this;
            StartTimer();
        } else {
            Destroy(gameObject);
        }
    }

    public static GameController GetInstance() {
        return gameController;
    }

    void Update() {
        if (timerRunning) {
            timeForSolve -= Time.deltaTime;
        }
        countdownText.text = timeForSolve.ToString("0.0") + "s";
    }

    public void StopTimer() {
        timerRunning = false;
        countdownText.color = solvedCountdownColor;
    }

    public void StartTimer() {
        timerRunning = true;
        countdownText.color = runningCountdownColor;
    }

    public bool TimerIsRunning() {
        return timerRunning;
    }

    public void ClearLevel() {
        SceneManager.LoadScene(nextSceneName);
    }

    public override void LoadFromFile() {
        Directory.CreateDirectory(Path.GetDirectoryName(GetFileName()));
        FileStream file = new FileStream(GetFileName(), FileMode.OpenOrCreate);

        byte[] jsonBytes = new byte[file.Length];
        if (file.Length == 0) {
            file.Close();
            return;
        }
        file.Read(jsonBytes, 0, (int)file.Length);

        string json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
        Saver s = JsonUtility.FromJson<Saver>(json);

        timeForSolve = s.timeForSolve;
        if (s.timerRunning) {
            StartTimer();
        } else {
            StopTimer();
        }

        file.Close();


    }


    public override void SaveToFile() {

        Saver s = new Saver();
        s.timerRunning = timerRunning;
        s.timeForSolve = timeForSolve;

        string json = JsonUtility.ToJson(s);

        Directory.CreateDirectory(Path.GetDirectoryName(GetFileName()));
        FileStream file = new FileStream(GetFileName(), FileMode.Truncate);

        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        file.Write(jsonBytes, 0, jsonBytes.Length);

        file.Close();

    }

    [Serializable]
    private class Saver {

        public bool timerRunning;
        public float timeForSolve;


    }

}
