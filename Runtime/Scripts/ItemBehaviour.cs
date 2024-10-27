using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IterationToolkit
{
    public class ItemBehaviour : MonoBehaviour
    {
        public Rigidbody rigidBody;
        public ItemData itemData;

        public ExtendedEvent<ItemBehaviour> OnItemAvailableToggle = new ExtendedEvent<ItemBehaviour>();
        public ExtendedEvent<ItemBehaviour> OnItemActiveToggle = new ExtendedEvent<ItemBehaviour>();
        public ExtendedEvent<ItemBehaviour> OnItemTryUsed = new ExtendedEvent<ItemBehaviour>();
        public ExtendedEvent<ItemBehaviour> OnItemUsed = new ExtendedEvent<ItemBehaviour>();

        public bool IsItemActive {  get; private set; }
        public bool isItemAvailable { get; private set; } //Lowkey don't know why this exists

        protected virtual void Update()
        {
            if (IsItemActive == false) return;

            if (itemData.UseMouse == true && Input.GetMouseButtonDown(itemData.UseItemMouseIndex) || Input.GetKeyDown(itemData.UseItemKeyCode))
            {
                OnItemTryUsed.Invoke();
                if (TryUseItem() == true)
                {
                    UseItem();
                    OnItemUsed.Invoke();
                }
            }
        }

        public void ToggleItemActive(bool newValue)
        {
            IsItemActive = newValue;

            if (IsItemActive == true)
                ItemActivated();
            else
                ItemDeactivated();
           
            OnItemActiveToggle.Invoke();
        }

        public void ToggleItemAvailable(bool newValue)
        {
            isItemAvailable = newValue;

            if (isItemAvailable == true)
                ItemAvailable();
            else
                ItemUnavailable();

            OnItemActiveToggle.Invoke();
        }

        public virtual bool TryUseItem() => false;

        protected virtual void UseItem() { }

        protected virtual void ItemActivated() { }

        protected virtual void ItemDeactivated() { }

        protected virtual void ItemAvailable() { }

        protected virtual void ItemUnavailable() { }

        public virtual string GetItemPrimaryDisplayText() => itemData.itemName;

        public virtual string GetItemSecondaryDisplayText() => string.Empty;
    }
}
