using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Player player1, player2;
    public CardStack deckStack, pileStack;
    public Button[] numberOfCardsButtons;
    public int maxCardsInHand,totalCards=52;
    public Mesh[] allCardMeshes;
    public Text GameOverText,playerName1,playerName2;
    public GameObject ReshufleDeck;
    public GameObject gameOverPanel,player1Button,player2Button;
    public float cardWait;
    private bool gameOver = false,startTurn;

    public static GameController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            deckStack.InitializeRandom(totalCards);
            pileStack.InitializeRandom(0);
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    public void firstTurn(bool ft)
    {
        startTurn = ft;
    }
    public void refresh()
    {
        if (!numberOfCardsButtons[0].interactable)
            maxCardsInHand = 6;
        else if (!numberOfCardsButtons[1].interactable)
            maxCardsInHand = 7;
        else if (!numberOfCardsButtons[2].interactable)
            maxCardsInHand = 10;
        else 
            maxCardsInHand = 13;
        player1.Initialize();
        player2.Initialize();
        playerName1.text = "Player 1 : " + player1.playerName;
        playerName2.text = "Player 2 : " + player2.playerName;
    }

    public void DealCards()
    {
        StartCoroutine(DealCard());
    }

    IEnumerator DealCard()
    {
    for (int i = 0; i < maxCardsInHand; ++i)
        {
            if (startTurn)
            {
                player1.ReplaceCard(deckStack.DrawCard(), i);
                player1.showCard(i);
                yield return new WaitForSeconds(cardWait);
                player2.ReplaceCard(deckStack.DrawCard(), i);
                player2.showCard(i);
                yield return new WaitForSeconds(cardWait);
            }
            else
            {
                player2.ReplaceCard(deckStack.DrawCard(), i);
                player2.showCard(i);
                yield return new WaitForSeconds(cardWait);
                player1.ReplaceCard(deckStack.DrawCard(), i);
                player1.showCard(i);
                yield return new WaitForSeconds(cardWait);
            }
        }
        player1.showingCards();
        player2.showingCards();
        player1Button.SetActive(true);
        player2Button.SetActive(false);
    }

    public void ReshuffleDeckFrompile()
    {
        int i, j, temp,pile;
        pile = pileStack.DrawCard();
        int N = pileStack.GetSize();
        int[] reference = new int[N];
        Debug.Log(N);
        for (i = 0; i < N; i++)
        {
            reference[i] = pileStack.DrawCard();
            Debug.Log(reference[i]);
        }
        if (pileStack.GetSize() != 0 ) Debug.Log("Error");
        Debug.Log(pileStack.GetSize());
        for (i = 0; i < reference.Length; ++i)
        {
            j = Random.Range(i, reference.Length);
            temp = reference[j];
            reference[j] = reference[i];
            reference[i] = temp;
        }
        for (i = 0; i < reference.Length; i++)
        {
            Debug.Log(reference[i]);
            deckStack.AddCard(reference[i]);
        }
        pileStack.AddCard(pile);
        if (pileStack.GetSize() != 1) Debug.Log("Error");
    }

    public void FinishGame(string name)
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        GameOverText.text = "The Game is Won by\nPlayer " +name;
        player1.myturn = player2.myturn = true; 
        player1.showingCards();
        player2.showingCards();
    }


}
