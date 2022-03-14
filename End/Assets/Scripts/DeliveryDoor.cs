﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliveryDoor : MonoBehaviour
{
    public Transform targetPos;
    public Vector2 CameraPosition;

    public bool teshu = false;
    bool CanThrough = false; 
    IEnumerator Delivery(MoveObject role)
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
    private bool isFirst = true;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CanThrough) return;
        if (!isFirst) return;
        if (collision.transform.parent.TryGetComponent<MoveObject>(out MoveObject player))
        {
            if (!teshu)
            {
                StartCoroutine(Delivery(player));
                isFirst = false;
            }
            else
            {
                SceneManager.LoadScene("OnSea");
            }
        }
    }
    //ghost
    //public void GhostDelivery(PlayerController ghost)
    //{
    //    StartCoroutine(Delivery(ghost));
    //}
    public void Pass()
    {
        CanThrough = true;
    }
}
