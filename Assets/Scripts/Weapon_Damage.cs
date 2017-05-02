﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples.Archery;

public class Weapon_Damage : MonoBehaviour {

    public bool StickToCollision;
    public Rigidbody attatchedBody;
    public float damage;

    void Start() {
        //Ignore the FPSController bounds
        foreach (GameObject playerColliderBox in GameObject.FindGameObjectsWithTag("PlayerCollisionBox")) {
            Physics.IgnoreCollision(playerColliderBox.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude > 0.7f) {
            if (StickToCollision && collision.gameObject.tag == "Target") {
                gameObject.transform.parent = collision.gameObject.transform;
                GetComponent<Rigidbody>().isKinematic = true;
                attatchedBody.isKinematic = true;
                Debug.Log("Good Collison");
            }
        }
    }
}