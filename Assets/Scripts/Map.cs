using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{


  public GameObject SystemPrefab;
  public Color PirateColor;
  public Color NeutralColor;

  public void MakeMap(Dictionary<int, dynamic> galaxy)
  {
    Transform GalaxyObject = GameObject.FindWithTag("galaxy").GetComponent<Transform>();
    int length = galaxy.Count;
    //GameObject star = new GameObject("");

    for (int i = 0; i<length; i++)
    {
      string owner = galaxy[i]["owner"];
      float[] position = galaxy[i]["position"];
      float x = position[0];
      float y = position[1];

      Vector3 positionVector = new Vector3(x,0f,y);

      GameObject star =Instantiate(SystemPrefab, positionVector, Quaternion.identity);

      Star Star = star.GetComponent<Star>();
      Star.ID = i;
      Star.Name = galaxy[i]["name"];
      Star.Owner = galaxy[i]["owner"];
      Star.Fleet = galaxy[i]["fleet"];


      if (owner == "pirate")
      {
        Star.Color = PirateColor;
      }
      else if(owner == "none")
      {
        Star.Color = NeutralColor;
      }

      star.transform.SetParent(GalaxyObject);
    }
  }

}
