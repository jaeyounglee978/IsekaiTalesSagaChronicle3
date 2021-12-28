using System;
using UnityEngine;

[Serializable]
public class UnitData
{
    public string name;
    public int speed;
    public int attack;
    public bool isInGuardPosition;
    public int skillCost;
    public TargetRangeType targetRangeType;
    public TargetSelectionType skillTargetType;
    public int maxHealth;
    public int initHealth;
}
