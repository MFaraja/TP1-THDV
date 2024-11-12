using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    private bool isOpen = false;
    private FPSController keys;

    void Start()
    {
        keys = FindObjectOfType<FPSController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Comprobar si la puerta no está abierta y el jugador tiene al menos 3 llaves
        if (!isOpen && keys.KeyAmount >= 1)
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cuando el jugador entra en el trigger, se comprueba si puede abrir la puerta
            if (keys.KeyAmount >= 1)
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        // Abrir la puerta y restar las llaves
        keys.KeyAmount -= 1;
        anim.SetTrigger("DoorOpen");
        isOpen = true; // Marca la puerta como abierta
        // Opcional: desactivar el collider para que no se vuelva a abrir
        GetComponent<Collider>().enabled = false;
    }
}