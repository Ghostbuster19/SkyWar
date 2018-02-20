using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public int StartingMoney = 10000;

    public static int Money;

	// Use this for initialization
	void Start ()
	{
	    Money = StartingMoney;
	}
}
