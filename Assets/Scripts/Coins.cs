using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Coins : MonoBehaviour
{
    public static int totalCoins = 0;
    void OnTriggerEnter(UnityEngine.Collider other)
    {
        totalCoins++;
        Destroy(gameObject);
    }
}
