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
    public Text Unpaired;
    public Slider Heart;
    public int GlobalPopulation;
    public int CoupledPopulation = 0;
    public int MaxHapiness;
    public int HappinessPercentage;
    public int Happiness = 0;
    public int NumberOfLevels;

    void Start()
    {
        PlayerPrefs.SetInt(Constants.GlobalPopulationKey, GlobalPopulation);
        CoupledPopulation = CheckSetOrGet(Constants.CoupledPopulationKey, CoupledPopulation);
        Happiness = CheckSetOrGet(Constants.HappinessKey, Happiness);
        int nonCoupledPopulation = GlobalPopulation - CoupledPopulation;
        Unpaired.text = Constants.UnpairedText + nonCoupledPopulation;
        MaxHapiness = (GlobalPopulation * Constants.MaxAffinity) / 2;
        HappinessPercentage = (Happiness * 100) / MaxHapiness;
        PopulationCounter.text = HappinessPercentage + "%";
        Heart.value = HappinessPercentage;
    }

    public void Play() {
        int rand =  Random.Range(1, NumberOfLevels + 1);
        SceneManager.LoadScene("Level" + rand);
    }

    public void ShowHideResetPopUp() {
        ResetPopUpAnimator.SetTrigger(Constants.HideShowPopUp);
    }

    public void ShowHideContactPopUp() {
        ContactPopUpAnimator.SetTrigger(Constants.HideShowPopUp);
    }

    public void ResetPopulation() {
        int globalPopulation = PlayerPrefs.GetInt(Constants.GlobalPopulationKey);
        PlayerPrefs.SetInt(Constants.GlobalPopulationKey, globalPopulation);
        PlayerPrefs.SetInt(Constants.HappinessKey, 0);
        PlayerPrefs.SetInt(Constants.CoupledPopulationKey, 0);
        PlayerPrefs.SetInt(Constants.EndGameKey, 0);
        Unpaired.text = Constants.UnpairedText + globalPopulation;
        Happiness = 0;
        CoupledPopulation = 0;
        MaxHapiness = (globalPopulation * Constants.MaxAffinity) / 2;
        HappinessPercentage = (Happiness * 100) / MaxHapiness;
        PopulationCounter.text = HappinessPercentage + "%";
        Heart.value = HappinessPercentage;
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
