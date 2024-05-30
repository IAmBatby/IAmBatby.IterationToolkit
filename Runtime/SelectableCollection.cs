using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectableCollection
{
    public List<GameObject> gameObjects;
    public GameObject SelectedObject
    {
        get
        {
            if (gameObjects != null || gameObjects.Count != 0)
                return (gameObjects[selectionIndex]);
            else
                return (null);
        }
    }

    private int selectionIndex;

    public ExtendedEvent<GameObject> onSelected = new ExtendedEvent<GameObject>();
    public ExtendedEvent<GameObject> onUnselected = new ExtendedEvent<GameObject>();

    public SelectableCollection(List<GameObject> objects, GameObject defaultSelectedObject = null)
    {
        gameObjects = new List<GameObject>(objects);
        if (defaultSelectedObject != null)
        {
            Select(defaultSelectedObject);
        }
    }

    public void Select(GameObject gameObject)
    {
        if (gameObjects.Contains(gameObject))
        {
            selectionIndex = gameObjects.IndexOf(gameObject);
            onSelected.Invoke(gameObject);

            foreach (GameObject unselectedObject in gameObjects)
                if (unselectedObject != SelectedObject)
                    onUnselected.Invoke(unselectedObject);
        }    
    }

    public void SelectForward()
    {
        selectionIndex = selectionIndex.Increase(gameObjects);
    }

    public void SelectBackward()
    {
        selectionIndex = selectionIndex.Decrease(gameObjects);
    }
}
