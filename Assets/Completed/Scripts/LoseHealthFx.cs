using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseHealthFx : MonoBehaviour {

    [SerializeField]
    float speed = 2f;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TextMesh text;

    public void SetText(int damageValue)
    {
        text.text = damageValue.ToString();
    }
	
	void Update ()
    {
        Color transparancy = sprite.color;
        transparancy.a -= Time.deltaTime * speed;
        sprite.color = transparancy;

        if (transparancy.a <= 0)
        {

            Destroy(this.gameObject);
        }
    }
}
