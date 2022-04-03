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
    }

    public void SetCards(List<Card> newCards)
    {
        cards = new List<CardObject>();
        for (int i = 0; i < newCards.Count; ++i)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent) as GameObject;
            cards.Add(newCard.GetComponent<CardObject>());
            cards[i].card.prefabToSpawn = newCards[i].prefabToSpawn;
            cards[i].card.number = i;
            cards[i].Init();
        }
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
                GlobalData.seaLevel += 1.0f;
                GlobalData.faith /= 2;
                GenerateHandSets();
                Invoke("ResolveRound", 2.0f);
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
    }

    public bool IsCardSelected()
    {
        return selectedCard >= 0;
    }

    void GenerateHandSets()
    {
        generatedHandSets[0] = handSets[Random.Range(0, handSets.Count)];
        generatedHandSets[1] = handSets[Random.Range(0, handSets.Count)];
        generatedHandSets[2] = handSets[Random.Range(0, handSets.Count)];

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
