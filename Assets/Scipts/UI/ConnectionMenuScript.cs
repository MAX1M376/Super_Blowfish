using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionMenuScript : MonoBehaviour
{
    public async void GetUser(int id)
    {
        InventoryScript.User = await InventoryScript.GetUser(id);
    }
}
