using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;


    // To make sure when spwaning the player prefabs, player can only control their own player prefab. Incase the joysticks overlap.
    void Start()
    {
        if (photonView.IsMine)
        {
            //the player is local
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);

        }
        else
        {
            //the player is remote
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }

        SetPlayerName();
    }

    void SetPlayerName()
    {
        playerNameText.text = photonView.Owner.NickName;

        if (photonView.IsMine)
        {
            
            playerNameText.color = Color.green;
        }

        else
        {
            playerNameText.color = Color.red;
        }
    }
}
