using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpawnManager : MonoBehaviourPunCallbacks
{

    public GameObject[] PlayerPrefabs;

    public Transform[] spawnPositions;
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        { 
          object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerSpinnerTopGame.PLAYER_SELECTION_NUMBER,out playerSelectionNumber))
                { 
                    Debug.Log("Player Selection Number is " + (int)playerSelectionNumber);

                int randomSpawnPoint = Random.Range(0, spawnPositions.Length-1);
                Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].position;

                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);

            }
        }

       
    }
}
