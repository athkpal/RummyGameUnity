using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStack : MonoBehaviour {

    public CardStack deckStack, pileStack;
    public Text deckText, pileText;
    public MeshFilter pileMeshFilter,deckMeshFilter;

    void Update ()
    {
        deckText.text = "Deck\n (" + deckStack.GetSize() + " Cards)";
        pileText.text = "Pile\n (" + pileStack.GetSize() + " Cards)";
        pileMeshFilter.mesh = (pileStack.GetSize() > 0) ?
                 GameController.instance.allCardMeshes[pileStack.GetTopCard()] :
                 null;
        deckMeshFilter.mesh = (deckStack.GetSize() > 0) ?
              GameController.instance.allCardMeshes[52] :
              null;
    }
}
