using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : Weapon
{
    string _name = "Shotgun";
    public override string name { get { return _name; } }

    int _attacksLeft = 6;
    public override int attacksLeft { get { return _attacksLeft; } }

    int _totalAttacks = 6;
    public override int totalAttacks { get { return _totalAttacks; } }

    int _damage = 3;
    public override int damage { get { return _damage; } }

    int _wallDamage = 0;
    public override int wallDamage { get { return _wallDamage; } }
    

    public override void PlayerAttacked()
    {
        _attacksLeft--;
    }

    public override int AttacksLeft()
    {
        return _attacksLeft;
    }

}
