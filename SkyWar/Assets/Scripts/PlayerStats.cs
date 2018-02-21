using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to keep track of the Player stats, basiclly the money.
/// </summary>
public class PlayerStats : MonoBehaviour
{

    public Text MoneyText;

    public float StartingMoney = 10000;

    public static float Money;

    private float lastMoneyCheck;

	// Use this for initialization
	void Start ()
	{
	    Money = StartingMoney;
	    lastMoneyCheck = Money;
	    MoneyText.text = Money.ToString() + " €";
	}

    // Is called to update the text in the UI.
    void Update()
    {
        if (lastMoneyCheck != Money)
        {
            lastMoneyCheck = Money;
            MoneyText.text = Money.ToString() + " €";
        }
    }
}
