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
    private float blockUserInputTime = 0;
    private float unitMovementDelta = 100f;

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
        playerFieldUnits.ForEach(u => {
            u.gameObject.SetActive(true);
            u.gameObject.transform.SetParent(unitParent.transform);
            u.gameObject.transform.localPosition = new Vector2(initPositionX, initPositionY);
            u.gameObject.transform.localScale = new Vector2(100f, 100f);
            u.spriteRenderer.sortingOrder = z;
            z -= 1;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (blockUserInputTime <= 0)
        {
            UserInput();
        }
        else
        {
            blockUserInputTime -= Time.deltaTime;
            if (blockUserInputTime <= 0)
            {
                Debug.Log("move end");
                IsEnemyEncountered();
            }
        }
        
    }
    
    private void IsEnemyEncountered() {
        var r = UnityEngine.Random.Range(0, 1001);

        if (r <= 300)
        {
            SceneManager.LoadScene("Battle", LoadSceneMode.Single);
        }
    }

    private void UserInput()
    {
        var currentLeaderCoordinate = playerFieldUnits.First().gameObject.transform.localPosition;

        Vector2 targetCoordinate = currentLeaderCoordinate;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            targetCoordinate = CalculateTargetPosition(currentLeaderCoordinate, Vector2.up * unitMovementDelta);
            blockUserInputTime = unitMoveTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            targetCoordinate = CalculateTargetPosition(currentLeaderCoordinate, Vector2.down * unitMovementDelta);
            blockUserInputTime = unitMoveTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetCoordinate = CalculateTargetPosition(currentLeaderCoordinate, Vector2.left * unitMovementDelta);
            blockUserInputTime = unitMoveTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            targetCoordinate = CalculateTargetPosition(currentLeaderCoordinate, Vector2.right * unitMovementDelta);
            blockUserInputTime = unitMoveTime;
        }

        if (blockUserInputTime > 0) PlayerUnitMove(targetCoordinate);
    }

    private Vector2 CalculateTargetPosition(Vector2 currentPosition, Vector2 delta) {
        var p = currentPosition + delta;

        if (p.x <= 100) p.x = 100;
        if (p.x >= 1080) p.x = 1080;

        if (p.y <= 100) p.y = 100;
        if (p.y >= 860) p.y = 860;

        return p;
    }

    private void PlayerUnitMove(Vector2 targetCoordinate)
    {
        if (targetCoordinate == null) return;

        var tc = targetCoordinate;
        playerFieldUnits.ForEach(pu => {
            var tempTc = pu.gameObject.transform.localPosition;
            StartCoroutine(pu.move(tc, unitMoveTime));
            tc = tempTc;
        });
    }
}
