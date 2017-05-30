using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    public Player player;
    private Button playerButton;
    private Text playerText;

    public void Start()
    {
        playerButton = GetComponent<Button>();
        playerText = GetComponentInChildren<Text>();
    }

    public void Update()
    {
        if (playerButton.interactable)
            playerText.text = "Begin My Turn";
        else if (player.extraCardObtained)
            playerText.text = "Throw a Card";
        else
            playerText.text = "Select Card from Deck/Pile";
	}

}
