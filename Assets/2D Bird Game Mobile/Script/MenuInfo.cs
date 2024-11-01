using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInfo : MonoBehaviour
{
    // Start is called before the first frame update

    int moneyAmout;
    public Text moneyAmoutText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moneyAmout = PlayerPrefs.GetInt("MoneyAmout");
        moneyAmoutText.text = "" + moneyAmout.ToString() + "";
    }
}
