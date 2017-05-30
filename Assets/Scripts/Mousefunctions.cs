using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mousefunctions : MonoBehaviour
{
   
    private Player player;
    private Vector3 intialMousePosition;
    private Vector3 origin;
    private Vector3 NewPosition;
    public int Id;
    private bool cardToPile = false,collided=false;


    public void Start()
    {
        player = GetComponentInParent<Player>();
        if (Id > GameController.instance.maxCardsInHand)
            gameObject.SetActive(false);
        origin = transform.position;
    }

    private void OnMouseDown()
    {
        transform.localScale = new Vector3(24, 24, 24);
        intialMousePosition = Input.mousePosition;
    }

    private Vector3 GetNewTransformPosition()
    {
        Vector3 oldpos = new Vector3(intialMousePosition.x * 20 / Screen.width, 0, intialMousePosition.y * 15 / Screen.height);
        Vector3 newpos = new Vector3(Input.mousePosition.x * 20 / Screen.width, 0, Input.mousePosition.y * 15 / Screen.height);
        return newpos - oldpos;
    }

    private void OnMouseDrag()
    {
        transform.position = origin + GetNewTransformPosition();
    }

    public void OnMouseUp()
    {
        transform.localScale = new Vector3(20, 20, 20);
        if (cardToPile) player.selectCard = Id;
        else if (collided) player.arrangeCards(Id, transform.position);
        transform.position = origin;
        cardToPile = false;
        collided = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Pile")
        {
            cardToPile=true;
        }
        else if(other.tag==gameObject.tag)
        {
            collided=true;
        }
        
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.name == "Pile")
        {
            cardToPile = false;
        }
        collided = false;
    }

}
