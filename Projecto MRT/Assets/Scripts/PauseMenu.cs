using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool IsInPause = false;
    public UniversalAdditionalCameraData playerCamera;

    // Referencia al GameObject que deseas desactivar/activar
    public GameObject targetGameObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsInPause = !IsInPause;
            pauseMenu.SetActive(IsInPause);

            if (IsInPause)
            {
                // Desactivar los componentes del jugador;
                // Desactivar el objeto completo que contiene el audio
                Time.timeScale = 0;
                playerCamera.enabled = false;

                // Desactivar el GameObject
                if (targetGameObject != null)
                {
                    targetGameObject.SetActive(false);
                }
            }
            else
            {
                // Reactivar los componentes del jugador
                // Reactivar el objeto completo que contiene el audio
                Time.timeScale = 1;
                playerCamera.enabled = true;

                // Activar el GameObject
                if (targetGameObject != null)
                {
                    targetGameObject.SetActive(true);
                }
            }
        }
    }
}