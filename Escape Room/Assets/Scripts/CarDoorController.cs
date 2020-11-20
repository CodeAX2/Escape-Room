using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class CarDoorController : Saveable, Interactable {

    public Animator carDoorAnimator;
    private bool isOpen = false;

    public AudioClip openSound, closeSound, lockedSound;

    public void OnInteract(PlayerController interactor) {

        if (interactor.InventoryContains("CarKey - RM1")) {
            if (!isOpen) {
                carDoorAnimator.ResetTrigger("CloseTrigger");
                carDoorAnimator.SetTrigger("OpenTrigger");
                AudioSource.PlayClipAtPoint(openSound, transform.position);
            } else {
                carDoorAnimator.ResetTrigger("OpenTrigger");
                carDoorAnimator.SetTrigger("CloseTrigger");
                AudioSource.PlayClipAtPoint(closeSound, transform.position);
            }
            isOpen = !isOpen;
        } else {
            AudioSource.PlayClipAtPoint(lockedSound, transform.position);
        }

    }
    public string GetInteractText() {
        if (!isOpen)
            return "Open Car Door";
        else
            return "Close Car Door";
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

        isOpen = s.isOpen;

        file.Close();

        if (isOpen) {
            carDoorAnimator.ResetTrigger("CloseTrigger");
            carDoorAnimator.SetTrigger("OpenTrigger");
        } else {
            carDoorAnimator.ResetTrigger("OpenTrigger");
            carDoorAnimator.SetTrigger("CloseTrigger");
        }

    }


    public override void SaveToFile() {

        Saver s = new Saver();
        s.isOpen = isOpen;
        string json = JsonUtility.ToJson(s);

        Directory.CreateDirectory(Path.GetDirectoryName(GetFileName()));
        FileStream file = new FileStream(GetFileName(), FileMode.Truncate);

        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        file.Write(jsonBytes, 0, jsonBytes.Length);

        file.Close();

    }

    [Serializable]
    private class Saver {

        public bool isOpen;

    }


}
