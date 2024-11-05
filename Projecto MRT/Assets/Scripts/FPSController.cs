using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Asegúrate de incluir esto para usar TextMeshPro

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float gravity = 10f;
    public int KeyAmount;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public PlayerDataSO playerData;
    public TextMeshProUGUI keyDisplay; // Referencia al texto de la UI que mostrará las llaves

    public float crouchHeight = 1f; // Altura del CharacterController al agacharse
    public float originalHeight = 2f; // Altura original del CharacterController
    public float crouchSpeedMultiplier = 0.5f; // Factor de velocidad al agacharse
    public float verticalMoveSpeed = 5f; // Velocidad de movimiento vertical

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    bool isCrouching = false;
    bool isMovingVertically = false; // Para controlar el movimiento vertical

    public bool canMove = true;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.height = originalHeight; // Asegura que empieza con la altura original
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateKeyDisplay(); // Actualiza la UI al inicio
    }

    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching; // No se puede correr al estar agachado
        float speed = isRunning ? playerData.runSpeed : playerData.walkSpeed;

        if (isCrouching)
        {
            speed *= crouchSpeedMultiplier; // Reduce la velocidad al agacharse
        }

        float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        #endregion

        #region Handles Vertical Movement with Q
        if (Input.GetKey(KeyCode.Q) && canMove)
        {
            isMovingVertically = true;
            moveDirection.y = verticalMoveSpeed; // Mover hacia arriba
        }
        else if (isMovingVertically)
        {
            isMovingVertically = false;
            StartCoroutine(DelayedFall()); // Iniciar la caída con un retraso
        }
        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isCrouching)
        {
            moveDirection.y = playerData.jumpPower;
        }
        else if (!isMovingVertically)
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded && !isMovingVertically)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Crouching
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
        }
        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
    }

    private IEnumerator DelayedFall()
    {
        yield return new WaitForSeconds(1f); // Esperar 1 segundo antes de caer
        moveDirection.y = 0; // Reinicia el movimiento vertical
    }

    private void ToggleCrouch()
    {
        if (isCrouching)
        {
            // Volver a la altura original
            characterController.height = originalHeight;
            isCrouching = false;
        }
        else
        {
            // Cambiar a la altura de agachado
            characterController.height = crouchHeight;
            isCrouching = true;
        }
    }

    private void PlayerDeath()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hubo contacto");
        if (collision.gameObject.layer == 7)
        {
            PlayerDeath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            KeyAmount += 1;
            Destroy(other.gameObject);
            UpdateKeyDisplay(); // Actualiza la UI al recoger la llave
        }
    }

    private void UpdateKeyDisplay()
    {
        if (keyDisplay != null)
        {
            keyDisplay.text = "Key: " + KeyAmount; // Actualiza el texto de la UI
        }
    }
}
