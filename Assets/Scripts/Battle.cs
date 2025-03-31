using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Battle : MonoBehaviour
{
    private float currentSpeed;
    private float startSpinSpeed;

    public Image spinSpeedBarImage;

    public Spinner spinnerScript;
    public TextMeshProUGUI spinSpeedRatioText;

    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpeed = spinnerScript.spinSpeed;

        spinSpeedBarImage.fillAmount = currentSpeed / startSpinSpeed;
        spinSpeedRatioText.text = currentSpeed.ToString();

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

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 100f);
                }
              
            }
           

        }
    }

    [PunRPC]

  public void DoDamage(float _damageAmount)
    {
        Debug.Log("Damage Done");

        spinnerScript.spinSpeed -= _damageAmount;
        currentSpeed = spinnerScript.spinSpeed;

        spinSpeedBarImage.fillAmount = currentSpeed / startSpinSpeed;

        spinSpeedRatioText.text = currentSpeed.ToString();
    }






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
