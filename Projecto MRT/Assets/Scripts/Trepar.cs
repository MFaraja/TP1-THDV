using System.Collections;
using UnityEngine;

public class EdgeClimb : MonoBehaviour
{
    public float grabDistance = 1.0f;           // Distancia para detectar el borde
    public float climbSpeed = 3.0f;             // Velocidad de escalada
    public LayerMask edgeLayer;                 // Layer para los bordes
    public Transform handPosition;              // Posición de la mano para alinearse con el borde

    private bool isGrabbing = false;
    private bool isClimbing = false;
    private Vector3 edgePosition;

    void Update()
    {
        if (!isGrabbing && !isClimbing)
        {
            CheckForEdge();
        }
    }

    private void CheckForEdge()
    {
        RaycastHit hit;
        Vector3 forward = transform.forward;

        // Detecta un borde dentro de la distancia de agarre
        if (Physics.Raycast(transform.position, forward, out hit, grabDistance, edgeLayer))
        {
            edgePosition = hit.point;
            isGrabbing = true;

            // Coloca al personaje en la posición para el agarre
            transform.position = handPosition.position + (edgePosition - handPosition.position);

            // Inicia la escalada automáticamente
            StartCoroutine(ClimbEdge());
        }
    }

    private IEnumerator ClimbEdge()
    {
        isGrabbing = false;
        isClimbing = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 2.0f + transform.forward * 0.5f; // Ajusta la altura y distancia según el borde

        float elapsedTime = 0;
        while (elapsedTime < climbSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / climbSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que el personaje esté en la posición final
        transform.position = endPosition;
        isClimbing = false;
    }
}
