using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UiController : MonoBehaviour
{
    public static UiController Instance;

    [SerializeField] GameObject gamePlay;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject stats;
    [SerializeField] TextMeshProUGUI popText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI faithText;
    [SerializeField] Slider quality;
    [SerializeField] UniversalRenderPipelineAsset settingsAsset;
    [SerializeField] TextMeshProUGUI roundsSurvived;

    void Awake()
    {
        Instance = this;
        popText.text = GlobalData.population.ToString();
        moneyText.text = GlobalData.money.ToString();
        faithText.text = GlobalData.faith.ToString();
    }

    void Start()
    {
        UiController.Instance.ShowMainmenuUI();
    }

    public void HideAll()
    {
        Time.timeScale = 1.0f;
        gamePlay.SetActive(false);
        mainMenu.SetActive(false);
        gameOver.SetActive(false);
        settings.SetActive(false);
        shop.SetActive(false);
        stats.SetActive(false);
    }

    public void ShowGamePlayUI()
    {
        HideAll();
        gamePlay.SetActive(true);
        stats.SetActive(true);
    }

    public void ShowMainmenuUI()
    {
        HideAll();
        mainMenu.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        HideAll();
        roundsSurvived.text = "You survived " + GlobalData.rounds + " rounds.";
        gameOver.SetActive(true);
    }

    public void ShowSettingsUI()
    {
        settings.SetActive(true);
    }

    public void HideSettingsUI()
    {
        settings.SetActive(false);
    }

    public void ShowShopUI()
    {
        HideAll();
        shop.SetActive(true);
        stats.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void Update()
    {
        popText.text = GlobalData.population.ToString();
        moneyText.text = GlobalData.money.ToString();
        faithText.text = GlobalData.faith.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settings.activeSelf)
            {
                HideSettingsUI();
            }
            else
            {
                ShowSettingsUI();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetRenderScale()
    {
        var urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        urpAsset.renderScale = quality.value;
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
