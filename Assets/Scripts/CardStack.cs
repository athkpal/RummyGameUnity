using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	
	public class CardStack : MonoBehaviour {

		private Stack<int> cards;
        public bool selected=false;

    public void InitializeRandom(int size)
        {
			if (size > 0) 
				ShuffleAndCopy (size);
		    else
				cards = new Stack<int> ();
		}
    
    private void OnMouseDown()
    {
    if(GetSize()!=0)
        selected = true;
    }

    public int DrawCard()
    {
      return cards.Pop();
    }

	public int GetTopCard()
    {
		return cards.Peek ();
	}

	public void AddCard(int card)
    {
       cards.Push (card);
	}

    public int GetSize()
    {
		return cards.Count;
	}

	void ShuffleAndCopy(int size)
    {
		// Fisher-Yates O(n) shuffle
		int i, j, temp;
        int[] reference=new int[size];
        for (i = 0; i < size; i++)
        {
            reference[i] = i;
        }
	    for (i = 0; i < size; ++i)
        {
			j = Random.Range (i, reference.Length);
			temp = reference[j];
			reference [j] = reference [i];
			reference [i] = temp;
		}

		cards = new Stack<int> (reference);
	}


	}