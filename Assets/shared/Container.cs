using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Container : MonoBehaviour {
    

    [System.Serializable]
    public class ContainerItem
    {

        public string Name;
        public int Maximum;
        public Guid Id;


        public int amountTaken;

        public ContainerItem()
        {
            Id = System.Guid.NewGuid();

        }

        public int Remaining
        {
            get
            {
                return Maximum - amountTaken;
            }
        }

        public int Get(int amount)
        {
            if (amountTaken + amount > Maximum)
            {
                int tooMuch = amountTaken + amount - Maximum;
                amountTaken = Maximum;
                return amount - tooMuch;
            }

            amountTaken += amount;

            return amount;

        }

        public void Put(int value)
        {

        }
    }


    public List<ContainerItem> items;

    private void Awake()
    {
    }

    public Guid Add(string name, int maximum)
    {
        if (items == null)
            items = new List<ContainerItem>();

        items.Add(new ContainerItem {
            Maximum = maximum,
            Name = name
        });

        return items.Last().Id;
    }

    public int TakeFromContainer(Guid id, int amount)
    {
        var containerItem = items.Where(x => x.Id == id).FirstOrDefault();

        if (containerItem == null)
            return -1;

        return containerItem.Get(amount);
    }
}
