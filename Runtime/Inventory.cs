using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    int itemSlots;
    int itemRowsAmount;

    Dictionary<ItemData, List<ItemBehaviour>> activeItemDict = new Dictionary<ItemData, List<ItemBehaviour>>();
    public List<ItemData> activeItemDatas => activeItemDict.Keys.ToList();
    public List<ItemBehaviour> ActiveItems { get; private set; } = new List<ItemBehaviour>();

    public ItemBehaviour SelectedItem
    {
        get
        {
            if (selectedItems != null)
                return (selectedItems.ActiveSelection);
            else
                return (null);
        }
    }

    [SerializeField] private SelectableCollection<ItemBehaviour> selectedItems = new SelectableCollection<ItemBehaviour>();

    ExtendedEvent onAddItem = new ExtendedEvent();
    ExtendedEvent onRemoveItem = new ExtendedEvent();
    ExtendedEvent onSelectionChanged = new ExtendedEvent();

    protected virtual void Awake()
    {
        selectedItems.Initalize();
    }

    public virtual bool CanAddItem(ItemData newItemData)
    {
        return (false);
    }

    public virtual void AddItem(ItemData newItemData)
    {
        ItemBehaviour spawnedItem = newItemData.SpawnPrefab();

        selectedItems.AddObject(spawnedItem);
        if (activeItemDict.ContainsKey(newItemData))
            activeItemDict[newItemData].Add(spawnedItem);
        else
            activeItemDict.Add(newItemData, new List<ItemBehaviour>() { spawnedItem });
        ActiveItems.Add(spawnedItem);
    }

    public virtual void RemoveItem(ItemBehaviour removingItem, bool destroyOnRemoval = false)
    {
        selectedItems.RemoveObject(removingItem);
        ActiveItems.Remove(removingItem);
        activeItemDict[removingItem.itemData].Remove(removingItem);
        if (activeItemDict[removingItem.itemData].Count == 0)
            activeItemDict.Remove(removingItem.itemData);

        if (destroyOnRemoval == true)
            GameObject.Destroy(removingItem.gameObject);
    }

    public virtual void SelectForward()
    {
        selectedItems.SelectForward();
    }

    public virtual void SelectBackward()
    {
        selectedItems.SelectBackward();
    }

    public virtual void Select(ItemBehaviour itemBehaviour)
    {

    }

    public virtual void Select(int itemBehaviourIndex)
    {

    }
}

