using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.Rendering;

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
        spinSpeedRatioText.text = currentSpeed.ToString();

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
            //Compare the speed of 2 players, the one with the higher speed wins
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            float opponentSpeed = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            // Debug.Log("My Speed: " + mySpeed + " Opponent Speed: " + opponentSpeed);

            if (mySpeed > opponentSpeed)
            {
                Debug.Log("I Win");

                float defaultDamageAmount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * common_damage_coifficient;
                if (isAttacker)
                {
                    defaultDamageAmount *= doDamage_coefficient_Attacker;
                }
                else if (isDefender)
                {
                    defaultDamageAmount *= doDamage_coefficient_Defender;

                }


                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine) //make sure it is local player to avoid multiple RPC
                {

                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, defaultDamageAmount);
                }

            }


        }
    }

    [PunRPC]

    public void DoDamage(float _damageAmount)
    {
        if (!isDead)
        {
            if (isAttacker)
            {
                _damageAmount *= getDamage_coefficient_Attacker;
            }
            else if (isDefender)
            {
                _damageAmount *= getDamage_coefficient_Defender;
            }

            Debug.Log("Damage Done");

            spinnerScript.spinSpeed -= _damageAmount;
            currentSpeed = spinnerScript.spinSpeed;

            spinSpeedBarImage.fillAmount = currentSpeed / startSpinSpeed;

            spinSpeedRatioText.text = currentSpeed.ToString("F0");

            if (currentSpeed <= 0)
            {
                Die();
                Debug.Log("Player die");
            }
        }



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
            Debug.Log("Player respawn");
        }


    }

    IEnumerator Respawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");


        if (deathPanelUIGameObject == null)
        {
            deathPanelUIGameObject = Instantiate(deathPanelUIPrefab, canvasGameObject.transform);
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
            respawnTime -= 1f;
            GetComponent<MovementController>().enabled = false;
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




    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
