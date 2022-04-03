using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController Instance;

    [SerializeField] GameObject gamePlay;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject shop;
    [SerializeField] TextMeshProUGUI popText;
    [SerializeField] TextMeshProUGUI moneyText;

    void Awake()
    {
        Instance = this;
        popText.text = GlobalData.population.ToString();
        moneyText.text = GlobalData.money.ToString();
    }

    void Start()
    {
        UiController.Instance.ShowMainmenuUI();
    }

    public void HideAll()
    {
        gamePlay.SetActive(false);
        mainMenu.SetActive(false);
        gameOver.SetActive(false);
        settings.SetActive(false);
        shop.SetActive(false);
    }

    public void ShowGamePlayUI()
    {
        HideAll();
        gamePlay.SetActive(true);
    }

    public void ShowMainmenuUI()
    {
        HideAll();
        mainMenu.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        HideAll();
        gameOver.SetActive(true);
    }

    public void ShowSettingsUI()
    {
        HideAll();
        settings.SetActive(true);
    }
    public void ShowShopUI()
    {
        HideAll();
        shop.SetActive(true);
    }

    private void Update()
    {
        popText.text = GlobalData.population.ToString();
        moneyText.text = GlobalData.money.ToString();
    }
}
