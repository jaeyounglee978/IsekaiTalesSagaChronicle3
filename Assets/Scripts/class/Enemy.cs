using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Unit
{
    private Action<List<Unit>> unitSkill;
    public Action<List<Unit>, List<Unit>> actionLogic;

    public Enemy(GameObject gameObject,
                string name,
                int speed, int attack,
                bool isInGuradPosition,
                int skillCost, TargetRangeType targetRangeType, TargetSelectionType skillTargetType,
                int health,
                Action<List<Unit>> unitSkill,
                Action<List<Unit>, List<Unit>> actionLogic)
        : base(gameObject, name, speed, attack, isInGuradPosition, skillCost, targetRangeType, skillTargetType, health)
    {
        this.unitSkill = unitSkill;
        this.actionLogic = actionLogic;
    }
    public override Action<List<Unit>> skill { get { return unitSkill; } }
}