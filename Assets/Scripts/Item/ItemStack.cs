using UnityEngine;

namespace WinterUniverse
{
    [System.Serializable]
    public class ItemStack
    {
        [SerializeField] private ItemData _item;
        [SerializeField] private int _amount;

        public ItemData Item => _item;
        public int Amount => _amount;

        public bool HasFreeSpace => _amount < _item.MaxCountInStack;
        public bool Empty => _amount <= 0;

        public void AddToStack(int value = 1)
        {
            _amount += value;
        }

        public void RemoveFromStack(int value = 1)
        {
            _amount -= value;
        }

        public ItemStack(ItemData item, int amount = 1)
        {
            _item = item;
            _amount = amount;
        }
    }
}