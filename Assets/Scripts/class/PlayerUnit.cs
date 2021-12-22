using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerUnit : Unit
{
    private Action<List<Unit>> unitSkill;
    
    public PlayerUnit(GameObject gameObject,
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

    public override Action<List<Unit>> skill 
    {
        get {
            return unitSkill;
        }
    }
}