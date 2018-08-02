using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Container : MonoBehaviour {

    private class ContainerItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Maximum { get; set; }

        private int amountTaken;

    }

    List<ContainerItem> items;

    private void Awake()
    {
        items = new List<ContainerItem>();
    }

    public Guid Add(string name, int maximum)
    {
        items.Add(new ContainerItem {
            Id = System.Guid.NewGuid(),
            Maximum = maximum,
            Name =name
        });

        return items.Last().Id;
    }
}
