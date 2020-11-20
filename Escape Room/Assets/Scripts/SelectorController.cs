using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;

public class SelectorController : MonoBehaviour {

    public Canvas baseCanvas, settingsCanvas, creditsCanvas;

    public Button room1Button, room2Button;
    private ColorBlock originalRoom1ButtonColors, originalRoom2ButtonColors;
    private bool room1Cleared = false, room2Cleared = false;
    public string room1Directory = "Room1Save", room2Directory = "Room2Save";

    public Color clearedColor;
    public Color inProgressColor;

    public Toggle musicToggle;

    void Start() {
        originalRoom1ButtonColors = room1Button.colors;
        originalRoom2ButtonColors = room2Button.colors;
        LoadRoom1Button();
        LoadRoom2Button();
        LoadSettings();
    }

    private void LoadRoom1Button() {
        room1Button.colors = originalRoom1ButtonColors;
        Directory.CreateDirectory(room1Directory);
        FileStream file = new FileStream(room1Directory + "\\GameController1.json", FileMode.OpenOrCreate);

        byte[] jsonBytes = new byte[file.Length];
        if (file.Length == 0) {
            room1Cleared = false;
            file.Close();
        } else {
            file.Read(jsonBytes, 0, (int)file.Length);

            string json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
            GameController.Saver s = JsonUtility.FromJson<GameController.Saver>(json);

            if (s.timeForSolve <= 0) {
                file.Close();
                DeleteSave();
                return;
            }

            file.Close();

            if (!s.timerRunning) {
                // Set the room as cleared
                ColorBlock colors = room1Button.colors;
                colors.normalColor = clearedColor;
                room1Button.colors = colors;

                room1Cleared = true;

            } else {
                // Room is in progress
                room1Cleared = false;
                ColorBlock colors = room1Button.colors;
                colors.normalColor = inProgressColor;
                room1Button.colors = colors;
            }

        }
    }

    private void LoadRoom2Button() {
        room2Button.colors = originalRoom2ButtonColors;
        if (!room1Cleared) {
            room2Button.interactable = false;
            return;
        }
        Directory.CreateDirectory(room2Directory);
        FileStream file = new FileStream(room2Directory + "\\GameController2.json", FileMode.OpenOrCreate);

        byte[] jsonBytes = new byte[file.Length];
        if (file.Length == 0) {
            room2Cleared = false;
            file.Close();
        } else {
            file.Read(jsonBytes, 0, (int)file.Length);

            string json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
            GameController.Saver s = JsonUtility.FromJson<GameController.Saver>(json);

            if (s.timeForSolve <= 0) {
                file.Close();
                DeleteSave();
                return;
            }

            file.Close();

            if (!s.timerRunning) {
                // Set the room as cleared
                ColorBlock colors = room2Button.colors;
                colors.normalColor = clearedColor;
                room2Button.colors = colors;

                room2Cleared = true;

            } else {
                // Room is in progress
                room2Cleared = false;
                ColorBlock colors = room2Button.colors;
                colors.normalColor = inProgressColor;
                room2Button.colors = colors;
            }

        }
    }

    private void LoadSettings() {
        Directory.CreateDirectory("ERSettings");
        FileStream file = new FileStream("ERSettings\\Settings.dat", FileMode.OpenOrCreate);

        byte[] musicToggledData = new byte[file.Length];
        if (file.Length == 0) {
            byte[] defaultData = Encoding.UTF8.GetBytes(musicToggle.isOn.ToString());
            file.Write(defaultData, 0, defaultData.Length);
            file.Close();
        } else {
            file.Read(musicToggledData, 0, (int)file.Length);

            bool musicToggled = bool.Parse(Encoding.UTF8.GetString(musicToggledData, 0, musicToggledData.Length));
            file.Close();
            musicToggle.isOn = musicToggled;
        }
    }


    public void PlayRoom1() {
        if (room1Cleared) {
            // The room was cleared, so reset its data
            Directory.Delete(room1Directory, true);
        }
        SceneManager.LoadScene("Level1");
    }

    public void PlayRoom2() {
        if (room2Cleared) {
            // The room was cleared, so reset its data
            Directory.Delete(room2Directory, true);
        }
        SceneManager.LoadScene("Level2");
    }

    public void OpenSettings() {
        settingsCanvas.gameObject.SetActive(true);
        baseCanvas.gameObject.SetActive(false);
    }
    public void CloseSettings() {
        baseCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
    }

    public void OpenCredits() {
        creditsCanvas.gameObject.SetActive(true);
        baseCanvas.gameObject.SetActive(false);
    }

    public void CloseCredits() {
        baseCanvas.gameObject.SetActive(true);
        creditsCanvas.gameObject.SetActive(false);
    }

    public void DeleteSave() {
        try {
            Directory.Delete(room1Directory, true);
            Directory.Delete(room2Directory, true);
        } catch (DirectoryNotFoundException e) {
            Debug.Log(e);
        }
        CloseSettings();
        LoadRoom1Button();
        LoadRoom2Button();
    }

    public void MusicToggled() {
        Directory.CreateDirectory("ERSettings");
        FileStream file = new FileStream("ERSettings\\Settings.dat", FileMode.Truncate);

        byte[] musicToggledData = Encoding.UTF8.GetBytes(musicToggle.isOn.ToString());
        file.Write(musicToggledData, 0, musicToggledData.Length);

        file.Close();
    }

    public void ExitGame() {
        Application.Quit();
    }
}
