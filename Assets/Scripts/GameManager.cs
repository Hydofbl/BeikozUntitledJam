using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject openGate, closeGate;
    public GameObject doorText;
    [SerializeField] private TMP_Text keyAmount;
        
    public int neededKey = 2;
    public bool isGameEnd, isGateOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        isGameEnd = false;
        isGateOpen = false;
        keyAmount.text = neededKey.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(!neededKey.ToString().Equals(keyAmount.text))
            keyAmount.text = neededKey.ToString();
        
        if (neededKey <= 0)
        {
            // Gate Opens 
            isGateOpen = true;
            
            openGate.gameObject.SetActive(true);
            closeGate.gameObject.SetActive(false);
        }
    }
}
