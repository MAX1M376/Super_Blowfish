using SimpleJSON;
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

    private void Start()
    {
        Inventory = new List<Prize>();
        AllPrizes = GetJson();
    }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Inventory");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public List<Prize> GetJson()
    {
        List<Prize> prizes = new List<Prize>();
        try
        {
            var jsonRoot = JSON.Parse(Resources.Load<TextAsset>("prizes").text);
            foreach (JSONNode n in jsonRoot)
            {
                Sprite image;
                try
                {
                    image = Resources.Load<Sprite>(n["ImagePath"]);
                }
                catch (Exception)
                {
                    image = defaultSprite;
                }
                prizes.Add(new Prize
                {
                    Name = n["Name"],
                    Description = n["Description"],
                    Image = image
                });
            }
        }
        catch (Exception ex)
        {
            prizes = new List<Prize> { new Prize { Name = "Error", Description = ex.Message, Image = defaultSprite } };
        }
        return prizes;
    }
}
