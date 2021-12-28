using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class NPC : Unit
{
    private Action<List<Unit>> unitSkill;

    public NPC(GameObject gameObject, UnitData unitData, Action<List<Unit>> unitSkill)
        : base(gameObject, unitData)
    {
        this.unitSkill = unitSkill;
    }
}