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
    public AudioSource backgroundMusic;
    private bool playAudio;
    public string room1Save, room2Save;
    public TextMeshProUGUI countdownText, winText;
    public Color solvedCountdownColor, runningCountdownColor;
    public float timeForSolve;

    public TextMeshPro officerText;

    private bool timerRunning = true;

    public int levelNumber;
    public string nextSceneName;

    protected new string GetFileName() {

        string saveFolder = "";
        if (levelNumber == 1) {
            saveFolder = room1Save;
        } else if (levelNumber == 2) {
            saveFolder = room2Save;
        }

        if (fileName == null) fileName = saveFolder + "\\" + "GameController" + levelNumber + ".json";
        return fileName;
    }

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
            if (timeForSolve <= 0) {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("MenuScene");
            }
        }
        countdownText.text = timeForSolve.ToString("0.0") + "s";

        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MenuScene");
        }


    }

    public void PlayMusic() {
        backgroundMusic = GetComponent<AudioSource>();
        if (playAudio) {
            backgroundMusic.Play();
        }
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

    public void EnableWinText(){
        winText.enabled = true;
    }

    public string GetSaveFolder() {
        if (levelNumber == 1) {
            return room1Save;
        } else if (levelNumber == 2) {
            return room2Save;
        }
        return "";
    }

    public override void LoadFromFile() {
        Directory.CreateDirectory(Path.GetDirectoryName(GetFileName()));
        FileStream file = new FileStream(GetFileName(), FileMode.OpenOrCreate);

        byte[] jsonBytes = new byte[file.Length];
        if (file.Length == 0) {
            file.Close();
        } else {
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

        // Get if audio should play
        Directory.CreateDirectory("ERSettings");
        file = new FileStream("ERSettings\\Settings.dat", FileMode.OpenOrCreate);

        byte[] musicToggledData = new byte[file.Length];
        if (file.Length == 0) {
            file.Close();
        } else {
            file.Read(musicToggledData, 0, (int)file.Length);

            playAudio = bool.Parse(Encoding.UTF8.GetString(musicToggledData, 0, musicToggledData.Length));
            file.Close();
        }

        PlayMusic();


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
    public class Saver {

        public bool timerRunning;
        public float timeForSolve;


    }

}
