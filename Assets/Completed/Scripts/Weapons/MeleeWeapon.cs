using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeapon : Weapon
{
    string _name = "MeleeWeapon";
    public override string name { get { return _name; } }

    int _attacksLeft = 4;
    public override int attacksLeft { get { return _attacksLeft; } }

    int _totalAttacks = 4;
    public override int totalAttacks { get { return _totalAttacks; } }

    int _damage = 2;
    public override int damage { get { return _damage; } }

    int _wallDamage = 2;
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
