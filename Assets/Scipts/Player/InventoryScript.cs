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
        Inventory = new List<Prize>();
        AllPrizes = GetJson(@"Assets/prizes.json");
    }

    public List<Prize> GetJson(string jsonPath)
    {
        List<Prize> prizes;
        try
        {
            List<PrizeString> prizesString;
            using (var reader = new StreamReader(jsonPath))
            {
                prizesString = JsonConvert.DeserializeObject<List<PrizeString>>(reader.ReadToEnd());
            }
            prizes = prizesString.Select(x => x.ToPrize(defaultSprite)).ToList();
        }
        catch (Exception ex)
        {
            prizes = new List<Prize> { new Prize { Name = "Error", Description = ex.Message, Image = defaultSprite } };
        }
        return prizes;
    }
}
