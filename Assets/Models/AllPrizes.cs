using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Models
{
    public class AllPrizes
    {
        public List<Prize> prizes { get; set; }

        public bool Instanciate(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(@"Assets\prizes.json"))
                {
                    prizes = JsonUtility.FromJson<AllPrizes>(reader.ReadToEnd()).prizes;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
