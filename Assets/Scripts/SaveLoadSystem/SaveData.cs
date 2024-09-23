using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveLoadSystem
{
    [System.Serializable]
    public class SaveData
    {
        public List<string> collectedItems;
        public SerializableDictionary<string, ChestSaveData> chestDictionary;
        public SerializableDictionary<string, ItemPickupSaveData> activeItems;
        

        public SaveData() 
        { 
        activeItems = new SerializableDictionary<string, ItemPickupSaveData> ();
        chestDictionary = new SerializableDictionary<string, ChestSaveData> ();
        collectedItems = new List<string> ();

        }



    }
}
