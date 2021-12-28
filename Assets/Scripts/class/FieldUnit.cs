using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FieldUnit
{
    public GameObject gameObject { get; }
    public SpriteRenderer spriteRenderer { get; }

    public FieldUnit(GameObject gameObject, string spriteName)
    {
        this.gameObject = gameObject;
        this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = Resources.Load<Sprite>(spriteName);
    }

    public IEnumerator move(Vector2 to, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = gameObject.transform.localPosition;
        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            gameObject.transform.localPosition = Vector3.Lerp(startingPos, to, (elapsedTime / seconds));
            yield return null;
        }
    }
}