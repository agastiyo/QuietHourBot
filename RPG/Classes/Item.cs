using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuietHourBot.RPG.Classes
{
    [Serializable]
    public class Item
    {
        public string ItemName { get; private set; }
        public int ItemValue { get; private set; }
        public int ItemAmount { get; private set; }

        public Item(string itemName, int itemValue, int itemAmount)
        {
            this.ItemName = itemName;
            this.ItemValue = itemValue;
            this.ItemAmount = itemAmount;
        }

        /// <summary>
        /// Gives the specified user a set amount of items
        /// </summary>
        /// <param name="amount">Amount of items to give</param>
        public void Give(int amount)
        {
            ItemAmount += amount;
        }

        /// <summary>
        /// Takes a set amount of items from a specified user
        /// </summary>
        /// <param name="amount">Amount of items to take</param>
        public void Take(int amount)
        {
            ItemAmount -= amount;
        }
    }
}