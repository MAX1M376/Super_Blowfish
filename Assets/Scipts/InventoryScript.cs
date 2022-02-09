using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [HideInInspector] public static int PrizeEarnDuringLevel = 0;
    [HideInInspector] public static List<Prize> AllPrizes { get; private set; }
    [HideInInspector] public static User User { get; set; }
    [HideInInspector] public static List<Prize> Inventory 
    { 
        get
        {
            return User.Inventory;
        }
    }

    [Header("Property")]
    [SerializeField] private Sprite defaultSprite;

    private void Awake()
    {
        // Suppression des gameobject multiple
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Inventory");
        if (objs.Length > 1)
        {
            objs.Where(x => x != this.gameObject).ToList().ForEach(x => Destroy(x));
        }
        DontDestroyOnLoad(this.gameObject);

        // Initialisation de la liste de toutes les offres disponibles ainsi que de l'inventaire
        AllPrizes = GetJson();
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
