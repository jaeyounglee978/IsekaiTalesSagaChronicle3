using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Unit
{
    private Action<List<Unit>> unitSkill;
    public Action<List<Unit>, List<Unit>> actionLogic;

    public Enemy(GameObject gameObject, UnitData unitData, Action<List<Unit>> unitSkill, Action<List<Unit>, List<Unit>> actionLogic)
        : base(gameObject, unitData)
    {
        this.unitSkill = unitSkill;
        this.actionLogic = actionLogic;
    }

    public override Action<List<Unit>> skill { get { return unitSkill; } }
}