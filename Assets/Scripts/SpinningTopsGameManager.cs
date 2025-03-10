using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.Rendering;
using Photon.Realtime;
using TMPro;

public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{

    [Header("UI References")]
    public GameObject uI_InformationPanelGameObject;
    public TextMeshProUGUI uI_InformationText;
    public GameObject searchForGameButton;

    // Start is called before the first frame update
    void Start()
    {
        uI_InformationPanelGameObject.SetActive(true);
        uI_InformationText.text = "Search for Battle";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinRandomRoom()
    {
        uI_InformationText.text = "Searching for available rooms...";
        PhotonNetwork.JoinRandomRoom();

        searchForGameButton.SetActive(false);
    }

    #region PHOTON CALLBACKS Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
       Debug.Log(message);
        uI_InformationText.text = message;

        CreateAndJoinRoom();
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uI_InformationText.text = " Joined to " + PhotonNetwork.CurrentRoom.Name + ". Waiting for opponent...";
        }
        else
        {
            uI_InformationText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name ;
            StartCoroutine(DeactiveAfterSeconds(uI_InformationPanelGameObject, 2f));
        }


        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + "Player count " + PhotonNetwork.CurrentRoom.PlayerCount);

        uI_InformationText.text = newPlayer.NickName + "joined to " + PhotonNetwork.CurrentRoom.Name + ". Player count " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactiveAfterSeconds(uI_InformationPanelGameObject, 2f));
    }


    #endregion
    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + UnityEngine.Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        //create room
        PhotonNetwork.CreateRoom(randomRoomName,roomOptions);
    }

    IEnumerator DeactiveAfterSeconds(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
