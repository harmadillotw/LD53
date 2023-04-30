using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter 
{
    public Address address;
    public int destination;
    public GameObject backside;
    public bool isBack = false;

    public Letter()
    {
        address = new Address();
    }

    public string printAddress()
    {
        return address.name + " " + address.road + " " + address.postCode;
    }
}
