using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviour
{
   public Player player;
   public void setName(string Name)
    {
        Debug.Log(Name);
        player.playerName = Name;
    }
}
