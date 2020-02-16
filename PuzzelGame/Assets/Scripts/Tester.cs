using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Weapons
{
    None,
    Pistol,
    Rifle,
    Sword
}

public class Tester : MonoBehaviour
{
    private Weapons weapon;
    private void Start()
    {
        weapon = Weapons.None;
        weapon = Weapons.Pistol;

        if (weapon == Weapons.Pistol | weapon == Weapons.Rifle)
        {
            Debug.Log("user has a gun as a weapon");
        }
    }





}



public class Player
{
    private int _power;

    public int Power
    {
        set
        {
            _power = value;
        }
        get
        {
            return _power;
        }
    }
}