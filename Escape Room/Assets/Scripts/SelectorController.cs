using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorController : MonoBehaviour {
    void Start() {

    }

    void Update() {

    }

    public void PlayRoom1() {
        SceneManager.LoadScene("Level1");
    }
}
