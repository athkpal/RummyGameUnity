using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public string playerName;
    private MeshFilter[] playerCardMeshes;
    public int selectCard = -1;
    public GameObject playerButton, otherButton;
    private int[] playerHand;
    public bool myturn;
    public bool extraCardObtained = false;
    private Transform[] playerCardPositions;
    public int numberOfFrames;

    public void Initialize()
    {
        playerCardMeshes = GetComponentsInChildren<MeshFilter>();
        playerCardPositions = GetComponentsInChildren<Transform>();
        playerHand = new int[GameController.instance.maxCardsInHand + 1];
    }

    public void showCard(int i)
    {
        StartCoroutine(moveCard(i));
    }

    IEnumerator moveCard(int i)
    {
       // Debug.Log(playerCardPositions[i+1].position);
        Vector3 initialPosition = playerCardPositions[i+1].position - new Vector3(-1,1,0);
        playerCardPositions[i+1].position = new Vector3(-1,1,0);
        playerCardMeshes[i].mesh = GameController.instance.allCardMeshes[52];
        for (int j = 0; j < numberOfFrames; j++)
        {
            playerCardPositions[i+1].position += initialPosition / numberOfFrames;
            yield return null;
        }
    }

    public void showingCards()
    {
        int i;
        for (i = 0; i < playerHand.Length - 1; ++i)
            playerCardMeshes[i].mesh = (myturn) ?
                GameController.instance.allCardMeshes[playerHand[i]] :
                GameController.instance.allCardMeshes[52];
        playerCardMeshes[i].mesh = (extraCardObtained) ?
                GameController.instance.allCardMeshes[playerHand[i]] : null;
    }

    public void ReplaceCard(int cardNew, int cardIndex)
    {
        playerHand[cardIndex] = cardNew;
    }

    public int ThrowCard(int index)
    {
        int temp = playerHand[index];
        playerHand[index] = playerHand[playerHand.Length-1];
        extraCardObtained = false;
        showingCards();
        return temp;
    }

    public void arrangeCards(int index, Vector3 newpos)
    {
        if (myturn)
        {
            int temp = playerHand[index], i;
            float place = (float)(newpos.x / (float)1.5) + 6;
            if (place > GameController.instance.maxCardsInHand)
                place = GameController.instance.maxCardsInHand - (float)0.5;
            if (index > place)
                for (i = index; i > place + 1; i--)
                {
                    ReplaceCard(playerHand[i - 1], i);
                }
            else
                for (i = index; i < place - 1; i++)
                {
                    ReplaceCard(playerHand[i + 1], i);
                }
            ReplaceCard(temp, i);
            showingCards();
        }
    }
    public void MyTurn()
    {
        StartCoroutine(Turn());
    }
    
    IEnumerator Turn()
    {
        myturn = true;
        showingCards();
        GameController.instance.deckStack.selected = false;
        GameController.instance.pileStack.selected = false;

        if (GameController.instance.deckStack.GetSize() == 0)
        {
            GameController.instance.ReshufleDeck.SetActive(true);
            yield return new WaitForSeconds(1);
            GameController.instance.ReshuffleDeckFrompile();
            GameController.instance.ReshufleDeck.SetActive(false);
        }
        while (!GameController.instance.deckStack.selected &&
        !GameController.instance.pileStack.selected)
            yield return null;
        if (GameController.instance.deckStack.selected)
            {
                Debug.Log("Deck is selected");
                ReplaceCard(GameController.instance.deckStack.DrawCard(), playerHand.Length-1);
            }
        else
            {
                Debug.Log("Pile is selected");
                ReplaceCard(GameController.instance.pileStack.DrawCard(), playerHand.Length-1);
            }
        
        extraCardObtained = true;
        showingCards();
        selectCard = -1;
        while (selectCard == -1)
            yield return null;
        GameController.instance.pileStack.AddCard(ThrowCard(selectCard));
        extraCardObtained = false;
        myturn = false;
        showingCards();
        yield return null;
        if (CheckForWin())
        {
            GameController.instance.FinishGame(playerName);
            otherButton.SetActive(false);
        }
        else
        {
            otherButton.SetActive(true);
        }
        playerButton.SetActive(false);
        showingCards();
    }

    private bool CheckForWin()
    {
        int X = GameController.instance.maxCardsInHand;
        bool[] numSet = new bool[X],suitSet = new bool[X];
        int[] hand=new int[X], number=new int[X], numberSet=new int[X], suit=new int[X],back=new int[X], front=new int[X];
        bool sameNumberSet = false,rummySet=false;
        for (int i = 0; i < X; i++)
        {
            hand[i] = playerHand[i];
            number[i] = playerHand[i] % 13;
            numberSet[i] = 0;
            suit[i] = (int)(playerHand[i] / 13);
            back[i] = 0;
            front[i] = 0;
            numSet[i] = false;
            suitSet[i] = false;
        }
        for (int i=0;i<X;i++)
        {
            for(int j=i+1;j<X;j++)
            {
                if(number[i]==number[j])
                {
                    numberSet[i]++;numberSet[j]++;
                }
            }
            CheckConsecutive(number,suit,back,front, i,X);
            if (numberSet[i] > 1) numSet[i] = true;
            if (number[i] != 12)
                if (front[i] - back[i] > 1) suitSet[i] = true;
                else suitSet[i] = false;
            else
                if (front[i] > 1 || back[i] < -1) suitSet[i] = true;
                else suitSet[i] = false;

            if (!(suitSet[i]||numSet[i])) return false;
        }

        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
        {
            sameNumberSet = rummySet = false;
            for (int i = 0; i < X; i++)
            {
                sameNumberSet = sameNumberSet || numSet[i];
                rummySet = rummySet || suitSet[i];
                if(numSet[i]==suitSet[i]) Debug.Log("Error");
            }
            if (rummySet == true) return true;
            else return false;
        }
        else
        {
            Debug.Log("Sorry,Not Valid");
            return false;
        }
    }

    private bool FinalCheckForWin(int[]number,int[]suit,int[]front,int[]back,bool[]suitSet,bool[]numSet,int X)
    {
        
        int[] num = new int[4];
        int[] st = new int[13];
        int k;
        for(int i=0;i<X;i++)
        {
            if (!(suitSet[i] || numSet[i])) return false;
            if (!(numSet[i] && suitSet[i]))
                continue;
            else
            {
                k = 0;
                num[0] = num[1] = num[2] = num[3] = -1;
                st[0] = st[1] = st[2] = st[3] = st[4] = st[5] = st[6] =
                st[7] = st[8] = st[9] = st[10] = st[11] = st[12] = -1;
                for (int j = 0; j < X; j++)
                {
                    if (number[j] == number[i]) num[k++] = j;
                    if (suit[j] == suit[i]) st[number[j]] = j;
                }
                if (num[3] != -1)
                {
                    numSet[i] = false;
                    if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                        return true;
                    else
                        numSet[i] = true;
                }
                else
                {
                    numSet[num[0]] = numSet[num[1]] = numSet[num[2]] = false;
                    if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                        return true;
                    else
                        numSet[num[0]] = numSet[num[1]] = numSet[num[2]] = true;
                }
                if (number[i] != 12)
                {
                    if (front[i]==0&&back[i]<-2||back[i]==0&&front[i]>2||back[i]<-2&&front[i]>2)
                    {
                        suitSet[i] = false;
                        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                            return true;
                        else
                            suitSet[i] = true;
                    }
                    else
                    {
                        for(k=number[i]+back[i];k<=number[i]+front[i];k++)
                        {
                            if (k==-1&&back[st[12]]<-1||k==12&&front[st[12]]>1) suitSet[st[12]]= true;
                            else suitSet[st[k]] = false;
                        }
                        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                            return true;
                        else
                            for (k = number[i] + back[i]; k <= number[i] + front[i]; k++)
                            {
                                if (k == -1 && back[st[12]] < -1 || k == 12 && front[st[12]] > 1) suitSet[st[12]] = true;
                                else suitSet[st[k]] = true;
                            }
                    }
                }
                else
                {
                    if (back[i] < -2 && front[i] > 2)
                    {
                        suitSet[i] = false;
                        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                            return true;
                        else
                            suitSet[i] = true;
                    }
                    else if(front[i]==0) 
                    {
                        for (k = 10; k <= 12; k++)         
                            suitSet[st[k]] = false;
                        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                            return true;
                        else for (k = 10; k <= 12; k++)
                            suitSet[st[k]] = true;
                    }
                    else if (back[i] == 0)
                    {
                        suitSet[i] = false;
                        for (k = 0 ; k < 2; k++)
                            suitSet[st[k]] = false;
                        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                            return true;
                        else
                        {
                            suitSet[i] = true;
                            for (k = 0; k < 2; k++)
                                suitSet[st[k]] = true;
                        }
                    }
                    else
                    {
                        for (k = 0; k < 2; k++)
                            suitSet[st[k]] = false;
                        front[i] = 0;
                        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                            return true;
                        else
                        {
                            for (k = 0; k < 2; k++)
                            suitSet[st[k]] = true;
                            front[i] = 2;
                        }
                        for (k = 10; k < 12; k++)
                            suitSet[st[k]] = false;
                        back[i] = 0;
                        if (FinalCheckForWin(number, suit, front, back, suitSet, numSet, X))
                            return true;
                        else
                        {
                            for (k = 0; k < 2; k++)
                            suitSet[st[k]] = true;
                            back[i] = 2;
                        }
                    }
                
                }
            }
            return false;
        }
        return true;
    }
    private void CheckConsecutive(int[]number,int[]suit,int[]back,int[]front, int index,int max)
    {
        for(int i = 0 ;i < max ; i++)
        {
            if(suit[index]==suit[i] && index!=i)
                if(AreConsecutive(number,back, front, index,i))
                    CheckConsecutive(number, suit, back, front, index,max);
        }
    }

    private bool AreConsecutive(int[]number,int[]back,int[]front, int index , int checkIndex)
    {
        if (number[checkIndex] == number[index] + front[index] + 1)
        {
            front[index]++;
            return true;
        }
        else if (number[checkIndex] == number[index] + back[index] - 1)
        {
            back[index]--;
            return true;
        }
        else if (number[checkIndex] == 12 && number[index] + back[index] == 0)
        {
            back[index]++;
            return true;
        }
        else if (number[index] == 12 && number[checkIndex] == front[index])
        {
            front[index]++;
            return true;
        }
        else
            return false;
    }

}

