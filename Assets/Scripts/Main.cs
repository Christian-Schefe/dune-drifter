using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Run run;

    private void Start()
    {
        run.BeginMatch();
    }
}