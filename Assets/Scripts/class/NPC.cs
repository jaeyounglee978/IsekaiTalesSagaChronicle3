using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class NPC : Unit
{
    private Action<List<Unit>> unitSkill;
    
    public NPC(GameObject gameObject,
                      string name,
                      int speed, int attack, 
                      bool isInGuradPosition, 
                      int skillCost, TargetRangeType targetRangeType, TargetSelectionType skillTargetType,
                      int health,
                      Action<List<Unit>> unitSkill)
        : base(gameObject, name, speed, attack, isInGuradPosition, skillCost, targetRangeType, skillTargetType, health )
    {
        this.unitSkill = unitSkill;
    }
}