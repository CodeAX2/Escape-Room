using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    private static GameController gameController;

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

    public static GameController GetInstance() {
        return gameController;
    }

}
