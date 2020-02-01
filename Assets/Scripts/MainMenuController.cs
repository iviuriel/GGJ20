using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Animator ResetPopUpAnimator;
    public Animator ContactPopUpAnimator;

    public Text PopulationCounter;
    public Slider Heart;
    public int GlobalPopulation;
    public int CoupledPopulation = 0;
    public int MaxHapiness;
    public int HappinessPercentage;
    public int Happiness = 0;

    void Start()
    {
        GlobalPopulation = CheckSetOrGet(Constants.GlobalPopulationKey, GlobalPopulation);
        CoupledPopulation = CheckSetOrGet(Constants.CoupledPopulationKey, CoupledPopulation);
        Happiness = CheckSetOrGet(Constants.HappinessKey, Happiness);
        MaxHapiness = GlobalPopulation * Constants.MaxAffinity;
        HappinessPercentage = (Happiness * 100) / MaxHapiness;
        PopulationCounter.text = HappinessPercentage + "%";
        Heart.value = HappinessPercentage;
    }

    public void Play() {
        SceneManager.LoadScene("GameController");
    }

    public void ShowHideResetPopUp() {
        ResetPopUpAnimator.SetTrigger(Constants.HideShowPopUp);
    }

    public void ShowHideContactPopUp() {
        ContactPopUpAnimator.SetTrigger(Constants.HideShowPopUp);
    }

    public void ResetPopulation() {
        //TODO: Reset Poputation
        ShowHideResetPopUp();
    }

    public int CheckSetOrGet(string key, int value) {
        int result;
        if (!PlayerPrefs.HasKey(key)){
            PlayerPrefs.SetInt(key, value);
            result = value;
        } else {
            result = PlayerPrefs.GetInt(key);
        }
        return result;
    }
}
