using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "IterationToolkit/ItemData", order = 1)]
    public class ItemData : ScriptableObject
    {
        public ItemGroupData itemGroupData;
        public string itemName;

        public GameObject itemPrefab;

        public ItemBehaviour SpawnPrefab()
        {
            GameObject instancedItemPrefab = GameObject.Instantiate(itemPrefab);
            return (instancedItemPrefab.GetComponent<ItemBehaviour>());
        }
    }

}