using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndication : MonoBehaviour
{
    public float damage = 20f;
    public void DestoryItself()
    {
        transform.parent.GetComponent<Tentacle>().overIndicaction = true;
        transform.parent.gameObject.GetComponent<Tentacle>().canAttack = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent<MoveObject>(out MoveObject mo))
        {
            mo.GetHurt(damage, new Vector2(0, 0));
        }
    }
}
