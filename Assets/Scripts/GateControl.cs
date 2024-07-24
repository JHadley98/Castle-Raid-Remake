using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControl : MonoBehaviour
{
    [Header("Gate Controls")]
    [SerializeField] private Animator leftGate = null;
    [SerializeField] private Animator rightGate = null;
    [SerializeField] private bool isOpen = false;

    public void OpenGates()
    {
        //If gates are closed then set to open
        if (!isOpen)
        {
            leftGate.SetBool("isOpen", true);
            rightGate.SetBool("isOpen", true);

            isOpen = true;
        }
    }
}
