using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgarrarObjeto : MonoBehaviour
{
    public GameObject HandPoint;
    public EdgeClimb edgeClimbScript; // Referencia al script EdgeClimb

    private GameObject pickedObject = null;

    void Update()
    {
        if (pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                // Soltar el objeto
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;

                // Reactivar el script EdgeClimb
                edgeClimbScript.enabled = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Objeto"))
        {
            if (Input.GetKey("e") && pickedObject == null)
            {
                // Agarrar el objeto
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = HandPoint.transform.position;
                other.gameObject.transform.SetParent(HandPoint.transform);
                pickedObject = other.gameObject;

                // Desactivar el script EdgeClimb
                edgeClimbScript.enabled = false;
            }
        }
    }
}
