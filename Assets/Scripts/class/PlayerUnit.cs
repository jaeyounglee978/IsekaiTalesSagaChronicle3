using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerUnit : Unit
{
    private Action<List<Unit>> unitSkill;

    public PlayerUnit(GameObject gameObject, UnitData unitData, Action<List<Unit>> unitSkill)
        : base(gameObject, unitData)
    {
        this.unitSkill = unitSkill;
    }

    public override Action<List<Unit>> skill { get { return unitSkill; } }
}