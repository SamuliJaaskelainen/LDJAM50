using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class HandSet
{
    public List<Card> cards = new List<Card>();
    public int price;
    public string setName;
    public string setDescription;
}

[System.Serializable]
public class CardSplitData
{
    public GameObject prefabToSplit;
    public List<Card> splitResult = new List<Card>();
}

public class Hand : MonoBehaviour
{
    public static Hand Instance;
    [SerializeField] List<HandSet> handSets = new List<HandSet>();
    [SerializeField] Camera handCam;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardParent;
    [SerializeField] LayerMask cardLayers;
    [SerializeField] int freeMoney = 10;
    [SerializeField] List<Card> freeCards = new List<Card>();
    [SerializeField] List<Card> randomAddCards = new List<Card>();
    [SerializeField] List<GameObject> cardSetUI = new List<GameObject>();
    [SerializeField] List<CardSplitData> splits = new List<CardSplitData>();
    List<CardObject> cards = new List<CardObject>();
    int selectedCard;
    RaycastHit hit;
    HandSet[] generatedHandSets = new HandSet[3];

    void Awake()
    {
        Instance = this;
        selectedCard = -1;
    }

    void Start()
    {
        GenerateHandSets();
        HideHand();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = handCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 99999.0f, cardLayers))
            {
                CardObject cardObject = hit.transform.GetComponent<CardObject>();
                if (cardObject != null)
                {
                    SelectCard(cardObject.card.number);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = handCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 99999.0f, cardLayers))
            {
                CardObject cardObject = hit.transform.GetComponent<CardObject>();
                if (cardObject != null)
                {
                    SplitCard(cardObject.card.number);
                }
            }
        }
    }

    public void SetCards(List<Card> newCards)
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            Destroy(cards[i].gameObject);
        }
        cards.Clear();

        cards = new List<CardObject>();
        for (int i = 0; i < newCards.Count; ++i)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent) as GameObject;
            cards.Add(newCard.GetComponent<CardObject>());
            cards[i].card.prefabToSpawn = newCards[i].prefabToSpawn;
            cards[i].card.number = i;
            cards[i].Init(newCards.Count);
            //Debug.Log("Init card: " + cards[i].card.prefabToSpawn.name);
        }
    }

    public void AddToHand(List<Card> newCards)
    {
        List<Card> allCards = new List<Card>();
        foreach (CardObject cardObject in cards)
        {
            Card card = cardObject.card;
            allCards.Add(card);
        }
        allCards.AddRange(newCards);
        SetCards(allCards);
    }

    public void SelectCard(int number)
    {
        if (selectedCard == number)
        {
            UnselectCard();
        }
        else
        {
            selectedCard = number;
            //Debug.Log("Selected card: " + cards[selectedCard].card.prefabToSpawn.name);
            CardSFXLogic(cards[selectedCard].card.prefabToSpawn.name);
        }
    }

    public void CardSFXLogic(string cardname)
    {
        switch (cardname)
        {
            case "Mansion":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_LargeHouse1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_LargeHouse2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_LargeHouse3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_LargeHouse4");
                        break;
                }
                break;
            case "TallHouse":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_MediumHouse1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_MediumHouse2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_MediumHouse3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_MediumHouse4");
                        break;
                }
                break;
            case "LargeHouse":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_MediumHouse1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_MediumHouse2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_MediumHouse3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_MediumHouse4");
                        break;
                }
                break;
            case "SmallHouse":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_SmallHouse1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_SmallHouse2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_SmallHouse3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_SmallHouse4");
                        break;
                }
                break;
            case "Apartment":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_LargeHouse1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_LargeHouse2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_LargeHouse3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_LargeHouse4");
                        break;
                }
                break;
            case "Brewery":
                AudioManager.Instance.PlaySound("Select_Brewery");
                break;
            case "Market":
                AudioManager.Instance.PlaySound("Select_Market");
                break;

            case "Park":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_Park1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_Park2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_Park3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_Park4");
                        break;
                }
                break;

            case "Greenhouse":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_GreenHouse1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_GreenHouse2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_GreenHouse3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_GreenHouse4");
                        break;
                }
                break;

            case "Wall":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_SmallStone1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_SmallStone2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_SmallStone3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_SmallStone4");
                        break;
                }
                break;

            case "Lighthouse":
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_MediumStone1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_MediumStone2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_MediumStone3");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("Select_MediumStone4");
                        break;
                }
                break;

            case "Keep":
                AudioManager.Instance.PlaySound("Select_LargeStone");
                break;

            case "Farm":
                switch (Random.Range(0, 2))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_Farm1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_Farm2");
                        break;
                }
                break;

            case "Shrine":
                switch (Random.Range(0, 3))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Select_Shrine1");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("Select_Shrine2");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("Select_Shrine3");
                        break;
                }
                break;

            case "Temple":
                switch (Random.Range(0, 7))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("ChantKarjala_02");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("ChantOmena_02");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("ChantPeruna_02");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("ChantPiirakka_02");
                        break;
                    case 4:
                        AudioManager.Instance.PlaySound("ChantPorkkana_02");
                        break;
                    case 5:
                        AudioManager.Instance.PlaySound("ChantSaatana_02");
                        break;
                    case 6:
                        AudioManager.Instance.PlaySound("ChantPerkele_02");
                        break;
                }
                break;

            case "Cathedral":
                switch (Random.Range(0, 7))
                {
                    case 0:
                        AudioManager.Instance.PlaySound("ChantKarjala_01");
                        break;
                    case 1:
                        AudioManager.Instance.PlaySound("ChantOmena_01");
                        break;
                    case 2:
                        AudioManager.Instance.PlaySound("ChantPeruna_01");
                        break;
                    case 3:
                        AudioManager.Instance.PlaySound("ChantPiirakka_01");
                        break;
                    case 4:
                        AudioManager.Instance.PlaySound("ChantPorkkana_01");
                        break;
                    case 5:
                        AudioManager.Instance.PlaySound("ChantSaatana_01");
                        break;
                    case 6:
                        AudioManager.Instance.PlaySound("ChantPerkele_01");
                        break;
                }
                break;

            default:
                AudioManager.Instance.PlaySound("deselect");
                break;
        }
    }

    public void SplitCard(int number)
    {
        foreach (CardSplitData splitData in splits)
        {
            if (cards[number].card.prefabToSpawn == splitData.prefabToSplit)
            {
                Destroy(cards[number].gameObject);
                cards.RemoveAt(number);
                AddToHand(splitData.splitResult);
            }
        }
    }

    public void UseSelectedCard(Vector3 pos, float yRot)
    {
        if (cards.Count > selectedCard)
        {
            GameObject newObject = Instantiate(cards[selectedCard].card.prefabToSpawn, pos, cards[selectedCard].card.prefabToSpawn.transform.rotation) as GameObject;
            newObject.transform.eulerAngles += new Vector3(0.0f, yRot, 0.0f);
            Destroy(cards[selectedCard].gameObject);
            cards.RemoveAt(selectedCard);
            if (cards.Count <= 0)
            {
                GlobalData.rounds++;
                Debug.Log("Round " + GlobalData.rounds + ": " + "Faith: " + GlobalData.faith + ", Sea level: " + GlobalData.seaLevel);
                GlobalData.seaLevel += Mathf.Max(1.0f - (GlobalData.faith / 50.0f), 0.25f) + (GlobalData.rounds * 0.1f);
                GlobalData.faith /= 2;
                GenerateHandSets();
                Invoke("ResolveRound", GlobalData.population > 0 ? 2.0f : 6.0f);
                HideHand();
            }
            else
            {
                for (int i = 0; i < cards.Count; ++i)
                {
                    cards[i].card.number = i;
                }
            }
        }
        UnselectCard();
    }

    void ResolveRound()
    {
        if (GlobalData.population <= 0)
        {
            UiController.Instance.ShowGameOverUI();
        }
        else
        {
            UiController.Instance.ShowShopUI();
        }
    }

    public void UnselectCard()
    {
        selectedCard = -1;
        AudioManager.Instance.PlaySound("deselect");
    }

    public bool IsCardSelected()
    {
        return selectedCard >= 0;
    }

    void GenerateHandSets()
    {
        int r1, r2, r3 = 0;
        r1 = Random.Range(0, handSets.Count);
        do
        {
            r2 = Random.Range(0, handSets.Count);
        }
        while (r2 == r1);
        do
        {
            r3 = Random.Range(0, handSets.Count);
        }
        while (r3 == r2 || r3 == r1);

        generatedHandSets[0] = handSets[r1];
        generatedHandSets[1] = handSets[r2];
        generatedHandSets[2] = handSets[r3];

        for (int i = 0; i < 3; ++i)
        {
            cardSetUI[i].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = generatedHandSets[i].setName;
            cardSetUI[i].transform.Find("Description").GetComponent<TextMeshProUGUI>().text = generatedHandSets[i].setDescription;
            cardSetUI[i].transform.Find("Price").GetComponent<TextMeshProUGUI>().text = generatedHandSets[i].price.ToString() + " gold";
        }
    }

    public void BuyHand(int hand)
    {
        if (GlobalData.money >= generatedHandSets[hand].price)
        {
            GlobalData.money -= generatedHandSets[hand].price;
            switch (Random.Range(0, 3))
            {
                case 0:
                    AudioManager.Instance.PlaySound("HandPurchase01");
                    break;
                case 1:
                    AudioManager.Instance.PlaySound("HandPurchase02");
                    break;
                case 2:
                    AudioManager.Instance.PlaySound("HandPurchase03");
                    break;
            }
            SetCards(generatedHandSets[hand].cards);
            UiController.Instance.ShowGamePlayUI();
            ShowHand();
        }
    }

    public void ShowHand()
    {
        cardParent.gameObject.SetActive(true);
    }

    public void HideHand()
    {
        cardParent.gameObject.SetActive(false);
    }

    public void FreeHand()
    {
        GlobalData.money += freeMoney;
        SetCards(freeCards);
        UiController.Instance.ShowGamePlayUI();
        ShowHand();
    }
}
