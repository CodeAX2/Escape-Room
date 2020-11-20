using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController2 : MonoBehaviour {

    private static GameController2 gameController;

    public GameObject pickUpParticleEffect;
    public string saveFolder;

    public TextMeshPro officerText;

    private void Awake() {
        if (gameController == null) {
            gameController = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static GameController2 GetInstance() {
        return gameController;
    }

}
