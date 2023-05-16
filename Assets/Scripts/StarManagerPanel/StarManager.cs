using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{

    public Button SendFleetButton;
    public Button SelectDestinationButton;



    public int ID;
    public int Fleet;
    public string Owner;
    public string Name;
    public Camera cam;
    public CamStarManagerOpener CSMO;

    [Space]
    public Text starName;
    public Text FleetText;
    public Text OwnerText;
    public Text SelectDestinationText;

    [Space]
    public int DestinationID;
    public int SelectedFleetPower;
    public Slider slider;





    public void UpdatePanel()
    {
      starName.text = Name;
      FleetText.text = Fleet.ToString();
      OwnerText.text = Owner;
      SelectDestinationText.text = "Select Destination";
    }

    public void ClosePanel(GameObject panel)
    {
      panel.SetActive(false);
      ID = -1;
    }

    public void SelectDestination()
    {
      CSMO.NotSelecting = false;
    }

    void Update()
    {

      if (Input.GetMouseButtonDown(0) && !CSMO.NotSelecting) //if mouse down and during selection
      {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
          GameObject system = hit.transform.gameObject;
          Star star = system.GetComponent<Star>();


          DestinationID = star.ID;
          SelectDestinationText.text = star.Name;
          CSMO.NotSelecting = true;


        }
      }
    }



    public void UpdateSliderValues(int fleet)
    {
      slider.maxValue = fleet;
    }






    public Text SelectedFleetPowerText;

    public void UpdateSliderValue()
    {
      SelectedFleetPower = (int)slider.value;
      SelectedFleetPowerText.text = SelectedFleetPower.ToString();
    }


    public void SendFleet()
    {

    }














































}
