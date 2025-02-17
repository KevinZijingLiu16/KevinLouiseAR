using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{

    public Button nextPlayerButton;
    public Button previousPlayerButton;
    public Transform playerSwitcherTransform;

    public int playerSelectionNumber;
    public GameObject[] spinnerTopModel;

    public TextMeshProUGUI playerModelTypeText;

    public GameObject UISelection;
    public GameObject UIAfterSelection;


    private void Start()
    {
        UISelection.SetActive(true);
        UIAfterSelection.SetActive(false);
        playerSelectionNumber = 0;
        playerModelTypeText.text = "Attacker";


    }
    public void NextPlayer()
    {
        playerSelectionNumber++;

        if (playerSelectionNumber >= spinnerTopModel.Length)
        {
            playerSelectionNumber = 0;
        }


        Debug.Log(playerSelectionNumber);

        nextPlayerButton.enabled = false;
        previousPlayerButton.enabled = false;
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90f, 1f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelTypeText.text = "Attacker";
        }

        else
        {
            playerModelTypeText.text = "Defender";
        }
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber--;

        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModel.Length - 1;
        }

       
        Debug.Log(playerSelectionNumber);

        nextPlayerButton.enabled = false;
        previousPlayerButton.enabled = false;
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90f, 1f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelTypeText.text = "Attacker";
        }

        else
        {
            playerModelTypeText.text = "Defender";
        }

    }

    public void OnSelectButtonClicked()
    {
        UISelection.SetActive(false);
        UIAfterSelection.SetActive(true);

        //set custom properties to player
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { MultiplayerSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);



        

    }

    public void OnReselectButtonClicked()
    {
        UISelection.SetActive(true);
        UIAfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {

        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    { 
    
    
    SceneLoader.Instance.LoadScene("Scene_Lobby");

    }


        IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime/duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        nextPlayerButton.enabled = true;
        previousPlayerButton.enabled = true;
    }
}
