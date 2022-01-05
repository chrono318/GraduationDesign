using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryDoor : MonoBehaviour
{
    public Transform targetPos;
    public Vector2 CameraPosition;

    
    
    IEnumerator Delivery(Role role)
    {
        //
        Camera.main.GetComponent<CameraControl>().Fade();

        yield return new WaitForSeconds(0.9f);

        Camera.main.transform.position = new Vector3(CameraPosition.x, CameraPosition.y, -20);
        role.transform.position = targetPos.position;

        yield return new WaitForSeconds(1f);
        //
        Game.instance.TranslateToNewRoom(role);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Role>(out Role player))
        {
            StartCoroutine(Delivery(player));
        }
    }
    //ghost
    public void GhostDelivery(Ghost ghost)
    {
        StartCoroutine(Delivery(ghost));
    }
}
