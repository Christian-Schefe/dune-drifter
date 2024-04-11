using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        transform.position = SaveState.Use("transformPos", () => transform.position);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += 10 * Time.deltaTime * Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += 10 * Time.deltaTime * Vector3.right;
        }
    }
}
