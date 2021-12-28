using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit
{
    public GameObject gameObject { get; private set; }
    public Outline outline { get; }
    public abstract Action<List<Unit>> skill { get; }

    public string name { get; private set; }
    public int speed { get; private set; }
    public int attack { get; private set; }
    public bool isInGuardPosition { get; private set; }
    public TargetRangeType targetRangeType { get; private set; }
    public TargetSelectionType skillTargetType { get; private set; }
    public int skillCost { get; private set; }
    public int maxHealth { get; private set; }

    public void SetGuardPosition(bool set = true)
    {
        this.isInGuardPosition = set;
    }

    public void UseSkill(List<Unit> targetUnitList)
    {
        this.skill.Invoke(targetUnitList);
    }

    private int _health;
    public int health
    {
        get
        {
            return _health;
        }
        private set
        {
            if (value > maxHealth)
            {
                _health = maxHealth;
            }
            else
            {
                _health = value;
            }
        }
    }

    public void GetDamage(int damage)
    {
        this.health -= damage;
    }

    public Unit(GameObject gameObject, UnitData unitData)
    {
        this.gameObject = gameObject;
        this.outline = this.gameObject.GetComponent<Outline>();

        this.name = unitData.name;
        this.speed = unitData.speed;
        this.attack = unitData.attack;
        this.isInGuardPosition = unitData.isInGuardPosition;
        this.skillCost = unitData.skillCost;
        this.targetRangeType = unitData.targetRangeType;
        this.skillTargetType = unitData.skillTargetType;
        this.maxHealth = unitData.maxHealth;
        this._health = unitData.initHealth;
    }
}
