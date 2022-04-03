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
            cards[i].Init();
            Debug.Log("Init card: " + cards[i].card.prefabToSpawn.name);
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
			Debug.Log("Selected card: " + cards[selectedCard].card.prefabToSpawn.name);
			CardSFXLogic(cards[selectedCard].card.prefabToSpawn.name);

        }
    }

    public void CardSFXLogic(string cardname)

    {
        switch (cardname)
        {
        case "Mansion":
            AudioManager.Instance.PlaySound("Select_LargeHouse");
            break;
        case "TallHouse":
            AudioManager.Instance.PlaySound("Select_MediumHouse");
            break;
        case "LargeHouse":
            AudioManager.Instance.PlaySound("Select_MediumHouse");
            break;
        case "SmallHouse":
            AudioManager.Instance.PlaySound("Select_SmallHouse");
            break;
        case "Apartment":
            AudioManager.Instance.PlaySound("Select_LargeHouse");
            break;
        case "Brewery":
            AudioManager.Instance.PlaySound("Select_Brewery");
            break;
        case "Market":
            AudioManager.Instance.PlaySound("Select_Market");
            break;

        case "Park":
            AudioManager.Instance.PlaySound("Select_Park");
            break;

        case "Wall":
            AudioManager.Instance.PlaySound("Select_SmallStone");
            break;

        case "Lighthouse":
            AudioManager.Instance.PlaySound("Select_MediumStone");
            break;

        case "Keep":
            AudioManager.Instance.PlaySound("Select_LargeStone");
            break;

        case "Farm":
			switch (Random.Range(0, 1))
				{
					case 0:
						AudioManager.Instance.PlaySound("Select_Farm1");
						break;
 	           		case 1:
 	               		AudioManager.Instance.PlaySound("Select_Farm2");
 	               		break;
				}
            break;

        case "Cathedral":
        	switch (Random.Range(0, 6))
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
