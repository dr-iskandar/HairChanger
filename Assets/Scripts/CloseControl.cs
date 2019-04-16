using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseControl : MonoBehaviour {

    public GameObject[] button;

    private void OnMouseDown()
    {
        for (int i = 0; i < button.Length; i++)
        {
            button[i].SetActive(false);
        }
    }
}
