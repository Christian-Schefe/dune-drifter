using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject shield;

    public void GainShield()
    {
        shield.SetActive(true);
    }

    public void LoseShield()
    {
        shield.SetActive(false);
    }
}
