using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbita : MonoBehaviour
{
    

    void Update()
    {
        transform.Rotate(0f, 20 * Time.deltaTime, 0f, Space.Self);
    }
}
