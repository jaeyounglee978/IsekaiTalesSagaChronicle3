using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Field : MonoBehaviour
{
    public GameObject unitParent;
    public GameObject test;
    private List<FieldUnit> playerFieldUnits;

    private float initPositionX = 100f;
    private float initPositionY = 100f;
    private float unitMoveTime = 0.25f;
    private float unitMovementDelta = 100f;

    private float blockUserInputUntil = -1;
    private bool canUserMove = true;

    // Start is called before the first frame update
    void Start()
    {
        InitPlayerUnits();
    }

    private void InitPlayerUnits()
    {
        // test code
        playerFieldUnits = new List<FieldUnit>
        {
            new FieldUnit(
                GameObject.Instantiate(test),
                "black"
            ),
            new FieldUnit(
                GameObject.Instantiate(test),
                "red"
            ),
            new FieldUnit(
                GameObject.Instantiate(test),
                "blue"
            ),
            new FieldUnit(
                GameObject.Instantiate(test),
                "green"
            )
        };

        var z = 4;
        playerFieldUnits.ForEach(u =>
        {
            u.gameObject.SetActive(true);
            u.gameObject.transform.SetParent(unitParent.transform);
            u.gameObject.transform.localPosition = new Vector2(initPositionX, initPositionY);
            u.gameObject.transform.localScale = new Vector2(100f, 100f);
            u.spriteRenderer.sortingOrder = z;
            z -= 1;
        });
    }

    void Update()
    {
        var canMove = blockUserInputUntil < Time.time;

        if (!canUserMove && canMove)
        {
            Debug.Log("move end");
            if (IsEnemyEncountered())
            {
                LoadEnemyEncounterScene();
            }
            canUserMove = true;
        }
        if (canUserMove)
        {
            UserInput();
        }
    }

    private bool IsEnemyEncountered()
    {
        return UnityEngine.Random.value < 0.3;
    }

    private void LoadEnemyEncounterScene()
    {
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }

    private void UserInput()
    {
        var currentLeaderCoordinate = playerFieldUnits.First().gameObject.transform.localPosition;

        Vector2 deltaPos = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            deltaPos = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaPos = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            deltaPos = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            deltaPos = Vector2.right;
        }

        if (deltaPos != Vector2.zero)
        {
            var targetCoordinate = CalculateTargetPosition(currentLeaderCoordinate, deltaPos * unitMovementDelta);
            blockUserInputUntil = Time.time + unitMoveTime;
            PlayerUnitMove(targetCoordinate);
            canUserMove = false;
        }
    }

    private Vector2 CalculateTargetPosition(Vector2 currentPosition, Vector2 delta)
    {
        var p = currentPosition + delta;
        p.x = Clamp(p.x, 100, 1080);
        p.y = Clamp(p.y, 100, 860);
        return p;
    }

    private void PlayerUnitMove(Vector2 targetCoordinate)
    {
        if (targetCoordinate == null) return;

        var tc = targetCoordinate;
        playerFieldUnits.ForEach(pu =>
        {
            var tempTc = pu.gameObject.transform.localPosition;
            StartCoroutine(pu.move(tc, unitMoveTime));
            tc = tempTc;
        });
    }

    private static float Clamp(float value, float min, float max)
    {
        return Mathf.Max(Mathf.Min(value, max), min);
    }
}
