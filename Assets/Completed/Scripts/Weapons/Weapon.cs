using Completed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon
{
    public virtual string name { get; set; }
    public virtual int damage { get; set; }
    public virtual int attacksLeft { get; set; }
    public virtual int totalAttacks { get; set; }
    public virtual int wallDamage { get; set; }

    public virtual void PlayerAttacked()
    {
        //Do nothing.
    }

    public virtual int AttacksLeft()
    {
        //Do nothing.
        Debug.LogWarning("Something went wrong!");
        return 0;
    }
}
