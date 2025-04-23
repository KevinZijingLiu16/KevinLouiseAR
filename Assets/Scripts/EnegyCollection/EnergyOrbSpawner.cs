using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EnergyOrbSpawner : MonoBehaviourPunCallbacks
{
    public GameObject energyOrbPrefab;
    public Transform[] spawnPoints;
    private GameObject currentOrb;

    public float respawnDelay = 20f;

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("MasterClient joined ¡ú spawning orb");
            SpawnEnergyOrb();
        }
    }

    void SpawnEnergyOrb()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        Debug.Log($"Spawning orb at index {index}, pos {spawnPoint.position}");

        currentOrb = PhotonNetwork.Instantiate(
            energyOrbPrefab.name,
            spawnPoint.position,
            Quaternion.identity
        );
    }

    public void RespawnOrbAfterDelay()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnergyOrb();
    }
}
