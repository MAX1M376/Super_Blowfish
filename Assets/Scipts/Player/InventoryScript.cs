using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [HideInInspector] public List<Prize> AllPrizes { get; private set; }
    public List<Prize> Inventory { get; set; }

    [Header("Property")]
    [SerializeField] private Sprite defaultSprite;

    void Start()
    {
        try
        {
            List<PrizeString> prizesString;
            using (var reader = new StreamReader(@"Assets\prizes.json"))
            {
                prizesString = JsonConvert.DeserializeObject<List<PrizeString>>(reader.ReadToEnd());
            }
            AllPrizes = prizesString.Select(x => x.ToPrize(defaultSprite)).ToList();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
