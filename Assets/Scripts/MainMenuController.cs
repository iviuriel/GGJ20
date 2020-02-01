using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Animator PopUpAnimator; 

    public void Play() {
        SceneManager.LoadScene("GameController");
    }

    public void ShowHidePopUp() {
        PopUpAnimator.SetTrigger("hidePopUp");
    }

    public void ResetPopulation() {

    }
}
