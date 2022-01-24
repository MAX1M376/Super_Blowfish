using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public AllPrizes allPrizes { get; private set; }
    public List<Prize> inventory { get; set; }

    void Start()
    {
        allPrizes = new AllPrizes();
        allPrizes.Instanciate(@"Assets\prizes.json");

        foreach (var item in allPrizes.prizes)
        {
            Debug.Log(item.Name);
        }
    }
}
