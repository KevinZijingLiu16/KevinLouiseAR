using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Battle : MonoBehaviourPun
{
    private float currentSpeed;
    private float startSpinSpeed;
    private Rigidbody rb;

    public Image spinSpeedBarImage;
    public GameObject uI_3D_GameObject;
    public GameObject deathPanelUIPrefab;
    private GameObject deathPanelUIGameObject;

    public Spinner spinnerScript;
    public TextMeshProUGUI spinSpeedRatioText;

    public float common_damage_coifficient = 0.04f;

    public bool isAttacker;
    public bool isDefender;
    private bool isDead;

    [Header("Player Type Damage Coefficients")]
    public float doDamage_coefficient_Attacker = 10f;
    public float getDamage_coefficient_Attacker = 1.2f;

    public float doDamage_coefficient_Defender = 0.75f;
    public float getDamage_coefficient_Defender = 0.2f;

    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpeed = spinnerScript.spinSpeed;

        spinSpeedBarImage.fillAmount = currentSpeed / startSpinSpeed;
        spinSpeedRatioText.text = currentSpeed.ToString("F0");
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;

            spinnerScript.spinSpeed = 5000f;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpeed = spinnerScript.spinSpeed;
            spinSpeedRatioText.text = currentSpeed.ToString("F0");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float mySpeed = rb.velocity.magnitude;
            float opponentSpeed = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            if (mySpeed > opponentSpeed)
            {
                float damage = mySpeed * 3600 * common_damage_coifficient;

                if (isAttacker)
                    damage *= doDamage_coefficient_Attacker;
                else if (isDefender)
                    damage *= doDamage_coefficient_Defender;

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, damage);
                }
            }
        }
    }

    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if (isDead) return;

        if (isAttacker)
        {
            _damageAmount *= getDamage_coefficient_Attacker;
            if (_damageAmount > 1000) _damageAmount = 400;
        }
        else if (isDefender)
        {
            _damageAmount *= getDamage_coefficient_Defender;
        }

        spinnerScript.spinSpeed -= _damageAmount;
        currentSpeed = spinnerScript.spinSpeed;

        spinSpeedBarImage.fillAmount = currentSpeed / startSpinSpeed;
        spinSpeedRatioText.text = currentSpeed.ToString("F0");

        if (currentSpeed <= 0)
        {
            Die();
        }
        // 同步当前转速到所有客户端（用于显示敌方血量）
        photonView.RPC("SyncSpinSpeed", RpcTarget.Others, spinnerScript.spinSpeed);

    }

    private void Die()
    {
        isDead = true;
        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinnerScript.spinSpeed = 0;
        uI_3D_GameObject.SetActive(false);

        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if (deathPanelUIGameObject == null)
        {
            deathPanelUIGameObject = Instantiate(deathPanelUIPrefab, canvas.transform);
        }
        else
        {
            deathPanelUIGameObject.SetActive(true);
        }

        Text respawnTimeText = deathPanelUIGameObject.transform.Find("RespawnTimeText").GetComponent<Text>();
        float respawnTime = 5f;

        while (respawnTime > 0)
        {
            respawnTimeText.text = respawnTime.ToString("F0");
            yield return new WaitForSeconds(1f);
            respawnTime--;
        }

        deathPanelUIGameObject.SetActive(false);
        GetComponent<MovementController>().enabled = true;

        photonView.RPC("Revive", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Revive()
    {
        spinnerScript.spinSpeed = startSpinSpeed;
        currentSpeed = spinnerScript.spinSpeed;

        spinSpeedBarImage.fillAmount = currentSpeed / startSpinSpeed;
        spinSpeedRatioText.text = currentSpeed.ToString("F0");

        uI_3D_GameObject.SetActive(true);
        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        isDead = false;
    }

    [PunRPC]
    public void SyncSpinSpeed(float syncedSpeed)
    {
        currentSpeed = syncedSpeed;
        spinSpeedBarImage.fillAmount = currentSpeed / startSpinSpeed;
        spinSpeedRatioText.text = currentSpeed.ToString("F0");
    }


    private void Start()
    {
        CheckPlayerType();
        rb = GetComponent<Rigidbody>();
    }
}
