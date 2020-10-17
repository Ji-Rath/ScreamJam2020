using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ItemSway : MonoBehaviour
{

    public Transform targetLocation;
    [HideInInspector]
    public GameObject TargetItem;
    public float smoothTime = 3f;
    public float maxSway = 0.2f;
    public float swaySensitivity = 0.1f;
    
    private Vector3 velocity = Vector3.zero;
    
    void LateUpdate()
    {
        //Item Sway
        if (TargetItem != null && targetLocation != null)
        {
            //Get values of mouse input
            float moveX = CrossPlatformInputManager.GetAxis("Mouse X") * swaySensitivity;
            float moveY = CrossPlatformInputManager.GetAxis("Mouse Y") * swaySensitivity;

            //Clamp values
            moveX = Mathf.Clamp(moveX, -maxSway, maxSway);
            moveY = Mathf.Clamp(moveY, -maxSway, maxSway);

            //Update position of object
            Vector3 desiredLocation = new Vector3(moveX, moveY, 0f);
            TargetItem.transform.localPosition = Vector3.Lerp(TargetItem.transform.localPosition, desiredLocation, smoothTime * Time.deltaTime);
        }
    }
}
