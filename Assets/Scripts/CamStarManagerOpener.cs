using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CamStarManagerOpener : MonoBehaviour
{

    public GameObject StarManagerMenu;
    public StarManager sm;
    public Camera cam;
    public bool NotSelecting = true;
    Client client;

    public Button SendFleetButton;
    public Button SelectDestinationButton;
    public Slider slider;

    void Update()
    {


      if (Input.GetMouseButtonDown(0) && NotSelecting)
      {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
          GameObject system = hit.transform.gameObject;
          Star star = system.GetComponent<Star>();

          if(sm.ID == star.ID)//close if second tap on the same system
          {
            sm.ID = -1; //assign value that cannot be taken,
            StarManagerMenu.SetActive(false);
            return;
          }

          if(!StarManagerMenu.activeSelf)
          {
            StarManagerMenu.SetActive(true);
          }

          sm.ID = star.ID;
          sm.Owner = star.Owner;
          sm.Fleet = star.Fleet;
          sm.UpdateSliderValues(star.Fleet);
          sm.Name = star.Name;
          sm.DestinationID = -1;

          //activate/deasticate destination, fleetpower selector and send fleet button if not the system owner
          if(client.player == star.Owner)
          {
            SendFleetButton.interactable = true;
            SelectDestinationButton.interactable = true;
            slider.interactable = true;
          }
          else
          {
            SendFleetButton.interactable = false;
            SelectDestinationButton.interactable = false;
            slider.interactable = false;
          }

          sm.UpdatePanel();
        }
      }
    }


    void Start()
    {
      client = GameObject.FindWithTag("Client").GetComponent<Client>();
    }
}
