using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EnergyCollector : MonoBehaviourPun
{
    public GameObject dragon;
    public float dragonDuration = 5f;
    public float damagePerSecond = 100f;

    private void Start()
    {
        // 你之前的 battleScript 没用到就可以删掉了
    }

    [PunRPC]
    public void SetDragonActive(bool isActive)
    {
        if (dragon != null)
        {
            dragon.SetActive(isActive);
        }
    }

    [PunRPC]
    public void SummonDragon()
    {
        if (dragon != null)
        {
            photonView.RPC("SetDragonActive", RpcTarget.AllBuffered, true);
            StartCoroutine(DragonAttackRoutine());
        }
    }

    IEnumerator DragonAttackRoutine()
    {
        float elapsed = 0f;

        while (elapsed < dragonDuration)
        {
            DealDamageToEnemies();
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }

        if (dragon != null)
        {
            photonView.RPC("SetDragonActive", RpcTarget.AllBuffered, false);
        }
    }

    void DealDamageToEnemies()
    {
        if (!photonView.IsMine) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player == this.gameObject) continue;

            PhotonView enemyView = player.GetComponent<PhotonView>();

            if (enemyView != null)
            {
                // 直接命令敌人客户端自己执行 DoDamage
                enemyView.RPC("DoDamage", enemyView.Owner, damagePerSecond);
            }
        }
    }
}
