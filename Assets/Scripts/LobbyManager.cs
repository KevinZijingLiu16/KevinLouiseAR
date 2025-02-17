using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("Login UI References")]
    public InputField playerNameInputField;
    public GameObject UILoginGameObject;


    [Header("Lobby UI References")]
    public GameObject UILobbyGameObject;
    public GameObject UI3DGameObject;

    [Header("ConnectionStatus UI References")]
    public GameObject UIConnectionStatusGameObject;

    public Text connectionStatusText;
    public bool showConnectionStatus = false;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            UILobbyGameObject.SetActive(true);
            UI3DGameObject.SetActive(true);
            UIConnectionStatusGameObject.SetActive(false);
            UILoginGameObject.SetActive(false);
        }
        else
        {
            UILobbyGameObject.SetActive(false);
            UI3DGameObject.SetActive(false);
            UIConnectionStatusGameObject.SetActive(false);
            UILoginGameObject.SetActive(true);
        }


       
    }

    // Update is called once per frame
    void Update()
    {
        if (showConnectionStatus)
        {
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
        }
            
    }
    #endregion

    #region UI Callback Methods


    public void OnQuickMatchButtonClicked()
    {
        // SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    public void OnEnterGameButtonClicked()
    {

        


        string playerName = playerNameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            UILobbyGameObject.SetActive(false);
            UI3DGameObject.SetActive(false);
            UIConnectionStatusGameObject.SetActive(true);
            showConnectionStatus = true;
            UILoginGameObject.SetActive(false);


            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;// Sets the name of the player in Photon
                PhotonNetwork.ConnectUsingSettings(); //Connects to Photon master server
            }
        }
        else
        {
            Debug.Log("Player Name is invalid");
        }

    }

    #endregion

    #region Photon Callback Methods

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Master Server");

        UILobbyGameObject.SetActive(true);
        UI3DGameObject.SetActive(true);
        UIConnectionStatusGameObject.SetActive(false);

        UILoginGameObject.SetActive(false);

    }
    #endregion
}





