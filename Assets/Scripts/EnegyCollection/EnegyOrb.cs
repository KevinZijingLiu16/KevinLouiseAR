// EnergyOrb.cs
using UnityEngine;
using Photon.Pun;

public class EnergyOrb : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        EnergyCollector collector = other.GetComponent<EnergyCollector>();
        if (collector != null)
        {
            //请求 MasterClient 来销毁 orb 并触发逻辑
            PhotonView.Get(this).RPC("RequestOrbCollected", RpcTarget.MasterClient, collector.photonView.ViewID);
        }
    }

    [PunRPC]
    
    public void RequestOrbCollected(int collectorViewID)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonView collectorView = PhotonView.Find(collectorViewID);
        if (collectorView != null)
        {
            collectorView.RPC("SummonDragon", RpcTarget.AllBuffered);



            PhotonNetwork.Destroy(this.gameObject);

           
            GameObject.FindObjectOfType<EnergyOrbSpawner>().RespawnOrbAfterDelay();
        }
    }

}
