using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private static GameController gameController;

    public GameObject pickUpParticleEffect;

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
