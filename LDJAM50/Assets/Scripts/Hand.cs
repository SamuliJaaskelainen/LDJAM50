using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandSet
{
    public Card[] cards;
    public int price;
}

public class Hand : MonoBehaviour
{
    public static Hand Instance;
    [SerializeField] List<HandSet> handSets = new List<HandSet>();
    [SerializeField] Camera handCam;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardParent;
    [SerializeField] LayerMask cardLayers;
    List<CardObject> cards = new List<CardObject>();
    int selectedCard;
    RaycastHit hit;

    [SerializeField] List<Card> debugCards = new List<Card>();

    void Awake()
    {
        Instance = this;
        selectedCard = -1;
    }

    void Start()
    {
        SetCards(debugCards);
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
                SetCards(debugCards);
                // TODO: Increase sea level
                UiController.Instance.ShowShopUI();
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

    public void UnselectCard()
    {
        selectedCard = -1;
    }

    public bool IsCardSelected()
    {
        return selectedCard >= 0;
    }
}
