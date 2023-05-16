using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFleets : MonoBehaviour
{



  public GameObject MovingFleetsPrefab;


  public void PlaceMovingFleets(Dictionary<int, dynamic> fleets)
  {

    Transform FleetHolder = GameObject.FindWithTag("fleets").GetComponent<Transform>();

    int length = fleets.Count;

    for (int i = 0; i<length; i++)
    {
    float[] position = fleets[i]["position"];
    float x = position[0];
    float y = position[1];

    Vector3 positionVector = new Vector3(x,0f,y);

    GameObject fleet =Instantiate(MovingFleetsPrefab, positionVector, Quaternion.identity);

    Fleet Fleet = fleet.GetComponent<Fleet>();

    Fleet.Owner = fleets[i]["owner"];
    Fleet.FleetStrength = fleets[i]["power"];
    Fleet.Destination = fleets[i]["destination"];

    fleet.transform.SetParent(FleetHolder);



    }

  }



}
