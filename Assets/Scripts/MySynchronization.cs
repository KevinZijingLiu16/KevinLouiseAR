using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class MySynchronization : MonoBehaviour, IPunObservable
{
    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkPosition;
    Quaternion networkRotation;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        networkPosition = new Vector3();
        networkRotation = new Quaternion();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkPosition, distance*(1.0f / PhotonNetwork.SerializationRate));
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkRotation, 100.0f * angle*(1.0f / PhotonNetwork.SerializationRate));
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            if (synchronizeVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if (synchronizeAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {
            // call my player gameobject that in remote player's game
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(rb.position, networkPosition) > teleportIfDistanceGreaterThan)
                {
                    rb.position = networkPosition;
                }
            }


            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                if (synchronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();
                    networkPosition += rb.velocity * lag;
                    distance = Vector3.Distance(rb.position, networkPosition);
                   
                }
                if (synchronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkRotation;
                    angle = Quaternion.Angle(rb.rotation, networkRotation);
                }
            }

        }
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
