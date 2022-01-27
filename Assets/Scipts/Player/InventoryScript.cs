using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [HideInInspector] public static List<Prize> AllPrizes { get; private set; }
    public List<Prize> Inventory { get; set; }

    [Header("Property")]
    [SerializeField] private Sprite defaultSprite;

    void Start()
    {
        Inventory = new List<Prize>();
        AllPrizes = GetJson();
    }

    public List<Prize> GetJson()
    {
        List<Prize> prizes;
        try
        {
            List<PrizeString> prizesString = JsonConvert.DeserializeObject<List<PrizeString>>(Resources.Load<TextAsset>("prizes").text);
            prizes = prizesString.Select(x => x.ToPrize(defaultSprite)).ToList();
        }
        catch (Exception ex)
        {
            prizes = new List<Prize> { new Prize { Name = "Error", Description = ex.Message, Image = defaultSprite } };
        }
        return prizes;
    }
}
