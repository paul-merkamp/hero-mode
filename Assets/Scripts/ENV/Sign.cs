using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private DialogController dialogController;

    public List<string> dialog = new();

    public void Start()
    {
        dialogController = FindObjectOfType<DialogController>();
    }

    public void Interact()
    {
        dialogController.TriggerDialog(dialog);
    }
}
