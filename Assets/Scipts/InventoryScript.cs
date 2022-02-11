using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryScript : MonoBehaviour
{
    private static Sprite DefaultSprite;

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

    private async void Awake()
    {
        // Suppression des gameobject multiple
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Inventory");
        if (objs.Length > 1)
        {
            objs.Where(x => x != this.gameObject).ToList().ForEach(x => Destroy(x));
        }
        DontDestroyOnLoad(this.gameObject);

        DefaultSprite = defaultSprite;
        // Initialisation de la liste de toutes les offres disponibles ainsi que de l'inventaire
        AllPrizes = await GetPrizes();
    }

    public static async Task<User> GetUser(int id)
    {
        var message = await GetRequest($"http://127.0.0.1:8080/inventory/{id}");
        List<Prize> inventory = new List<Prize>();
        foreach (JSONArray n in JSONNode.Parse(message).AsArray)
        {
            for (int i = 0; i < n.Count; i++)
            {
                if (int.TryParse(n[i], out int invId))
                {
                    inventory.Add(AllPrizes.First(x => x.Id == invId));
                }
                else
                {
                    Debug.LogWarning($"Parse error: {n[i]}");
                }
            }
        }
        return new User
        {
            Id = id,
            Inventory = inventory
        };
    }

    public static async Task<List<Prize>> GetPrizes()
    {
        string message = await GetRequest($"http://127.0.0.1:8080/promos");
        List<Prize> all = new List<Prize>();
        foreach (JSONNode n in JSONNode.Parse(message))
        {
            Sprite image;
            try
            {
                image = Resources.Load<Sprite>(n["ImagePath"]);
            }
            catch (Exception)
            {
                image = DefaultSprite;
            }
            all.Add(new Prize
            {
                Id = n["Id"],
                Name = n["Name"],
                Description = n["Description"],
                Image = image
            });
        }
        return all;
    }

    public static async Task PostPrize(int prizeId)
    {
        await PostRequest($"http://127.0.0.1:8080/inventory", User.Id, prizeId);
    }

    private static async Task<string> GetRequest(string uri)
    {
        var request = UnityWebRequest.Get(uri);

        // Request and wait for the desired page.
        var task = request.SendWebRequest();
        while (!task.isDone)
            await Task.Yield();

        return request.downloadHandler.text;
    }

    private static async Task<bool> PostRequest(string uri, int userId, int prizeId)
    {
        string postData = JsonUtility.ToJson(new { client_id = userId, promo_id = prizeId });
        var request = UnityWebRequest.Post(uri, postData);

        // Request and wait for the desired page.
        var task = request.SendWebRequest();
        while (!task.isDone)
            await Task.Yield();

        return request.isDone;
    }
}
