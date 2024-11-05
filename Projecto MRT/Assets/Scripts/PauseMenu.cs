using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool IsInPause = false;
    public UniversalAdditionalCameraData playerCamera;

    // Referencia al GameObject que deseas desactivar/activar
    public GameObject targetGameObject;

    void Start()
    {
        // Asegurarse de que el juego esté en modo "reanudado" al cargar la escena
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsInPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        IsInPause = true;
        pauseMenu.SetActive(true);

        // Detener el tiempo y desactivar la cámara del jugador
        Time.timeScale = 0;
        playerCamera.enabled = false;

        // Desactivar el GameObject deseado
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(false);
        }

        // Mostrar y desbloquear el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        IsInPause = false;
        pauseMenu.SetActive(false);

        // Reactivar el tiempo y la cámara del jugador
        Time.timeScale = 1;
        playerCamera.enabled = true;

        // Activar el GameObject deseado
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(true);
        }

        // Ocultar y bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ExitToMainMenu()
    {
        // Restablecer el tiempo y el cursor al salir del juego
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Cargar el menú principal (ajusta el nombre de la escena según tu configuración)
        SceneManager.LoadScene("MainMenu");
    }
}
