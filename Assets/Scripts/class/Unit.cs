using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit {
    public GameObject gameObject {
        get;
        private set;
    }

    public Outline outline {
        get;
    }

    public string name {
        get;
        private set;
    }

    public int speed {
        get;
        private set;
    }

    public int attack {
        get;
        private set;
    }

    public bool isInGuradPosition {
        get;
        private set;
    }
    public void SetGuardPosition(bool set) {
        this.isInGuradPosition = set;
    }
    public void SetGuardPosition() {
        this.isInGuradPosition = true;
    }

    public int skillCost {
        get;
        private set;
    }

    public abstract Action<List<Unit>> skill {
        get;
    }

    public void UseSkill(List<Unit> targetUnitList) {
        this.skill.Invoke(targetUnitList);
    }
    public TargetRangeType targetRangeType {
        get;
        private set;
    }

    public TargetSelectionType skillTargetType {
        get;
        private set;
    }

    public int maxHealth {
        get;
        private set;
    }
    
    private int _health;

    public int health {
        get
        {
            return _health;
        }
        private set
        {
            if (value > maxHealth) {
                _health = maxHealth;
            } else {
                _health = value;
            }
        }
    }
    public void GetDamage(int damage) {
        this.health -= damage;
    }

    public Unit(GameObject gameObject,
                string name,
                int speed,
                int attack,
                bool isInGuradPosition,
                int skillCost,
                TargetRangeType targetRangeType,
                TargetSelectionType skillTargetType,
                int health) {
        this.gameObject = gameObject;
        this.outline = this.gameObject.GetComponent<Outline>();
        this.name = name;
        this.speed = speed;
        this.attack = attack;
        this.isInGuradPosition = isInGuradPosition;
        this.skillCost = skillCost;
        this.targetRangeType = targetRangeType;
        this.skillTargetType = skillTargetType;
        this.maxHealth = health;
        this.health = health;
        this.isInGuradPosition = false;
    }

public Unit(GameObject gameObject,
                string name,
                int speed,
                int attack,
                bool isInGuradPosition,
                int skillCost,
                TargetRangeType targetRangeType,
                TargetSelectionType skillTargetType,
                int maxHealth, int health) {
        this.gameObject = gameObject;
        this.outline = this.gameObject.GetComponent<Outline>();
        this.name = name;
        this.speed = speed;
        this.attack = attack;
        this.isInGuradPosition = isInGuradPosition;
        this.skillCost = skillCost;
        this.targetRangeType = targetRangeType;
        this.skillTargetType = skillTargetType;
        this.maxHealth = maxHealth;
        this.health = health;
        this.isInGuradPosition = false;
    }
}