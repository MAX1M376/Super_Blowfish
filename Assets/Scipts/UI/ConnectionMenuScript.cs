using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectionMenuScript : MonoBehaviour
{
    public void GetUser()
    {
        InventoryScript.User = new User
        {
            Id = 0,
            Name = "Maxime",
            Email = "maxime.adler76@gmail.com",
            Inventory = Enumerable.Range(0, 2).Select(x => InventoryScript.AllPrizes[Random.Range(0, InventoryScript.AllPrizes.Count)]).ToList(),
        };
    }
}
