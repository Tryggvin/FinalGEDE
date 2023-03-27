using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text TMP_text;
    // Start is called before the first frame update
    void Start()
    {
        TMP_text.text = GameManager.instance.currentAccount.name;
    }

    
}
