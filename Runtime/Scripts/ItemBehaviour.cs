using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ItemBehaviour : MonoBehaviour
    {
        public Rigidbody rigidBody;
        public ItemData itemData;

        public bool IsItemActive {  get; private set; }
        public bool isItemAvailable { get; private set; }

        public void ToggleItemActive(bool newValue)
        {
            IsItemActive = newValue;

            if (IsItemActive == true)
                OnItemActivated();
            else
                OnItemDectivated();
        }

        public void ToggleItemAvailable(bool newValue)
        {
            isItemAvailable = newValue;

            if (isItemAvailable == true)
                OnItemAvailable();
            else
                OnItemUnavailable();
        }

        protected virtual void OnItemActivated()
        {

        }

        protected virtual void OnItemDectivated()
        {

        }

        protected virtual void OnItemAvailable()
        {

        }

        protected virtual void OnItemUnavailable()
        {

        }

        public virtual string GetItemPrimaryDisplayText() => string.Empty;

        public virtual string GetItemSecondaryDisplayText() => string.Empty;
    }
}
