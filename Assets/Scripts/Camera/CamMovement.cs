using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CamMovement : MonoBehaviour
{


    public float PanSpeed;
    [Space]
    public Camera cameraCamera;
    public GameObject cameraObject;
    private Vector3 originClick;
    private Vector3 originCam;



    //https://answers.unity.com/questions/967170/detect-if-pointer-is-over-any-ui-element.html
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }






    public void PanCamera()
    {

        if(IsPointerOverUIObject())
        {
          return;
        }



        if(Input.GetMouseButtonDown(0))
        {




          originCam = cameraObject.transform.position;
          originClick = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
          Vector3 diff = originClick - Input.mousePosition;
          Vector3  sub = (originCam + (new Vector3(diff.x, 1f, diff.y) * PanSpeed));
          sub.y = 1.44f;
          cameraObject.transform.position = sub;
        }


    }


    public void Start()
    {
      LoadPosition();
    }

    public void Update()
    {
      PanCamera();
    }


    public void SavePosition()
    {
      Client client = GameObject.FindWithTag("Client").GetComponent<Client>();
      client.CamX = gameObject.transform.position.x;
      client.CamZ = gameObject.transform.position.z;
    }

    public void LoadPosition()
    {
      Client client = GameObject.FindWithTag("Client").GetComponent<Client>();
      gameObject.transform.position = new Vector3(client.CamX, 1.5f, client.CamZ);
    }




}
