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

        [field: SerializeField] public bool UseMouse { get; private set; }
        [field: SerializeField] public int UseItemMouseIndex { get; private set; }
        [field: SerializeField] public KeyCode UseItemKeyCode { get; private set; } //Will be replaced with ScriptableSetting stuff later

        public ItemBehaviour SpawnPrefab()
        {
            GameObject instancedItemPrefab = GameObject.Instantiate(itemPrefab);
            return (instancedItemPrefab.GetComponent<ItemBehaviour>());
        }
    }

}