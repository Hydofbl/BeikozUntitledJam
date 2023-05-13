using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransitionScript : MonoBehaviour
{
    [SerializeField] private String levelName;
    
    void Start()
    {
        GetComponentInChildren<TMP_Text>().text = levelName;
    }
}
