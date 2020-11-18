using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface Interactable {

    void OnInteract(PlayerController interactor);
    string GetInteractText();

}
