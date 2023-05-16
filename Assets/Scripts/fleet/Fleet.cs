using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fleet : MonoBehaviour
{

      public int ID = 0;
      public Color Color;
      public string Owner;
      public int FleetStrength;
      public int Destination;
      public Client client;
      [Space]
      public TextMesh FleetText;
      public GameObject Capsule;


      void Start()
      {
        Client client =GameObject.FindWithTag("Client").GetComponent<Client>();

        string colHex = client.playerList[Owner]["color"];
        ColorUtility.TryParseHtmlString(colHex, out Color);


        FleetText.text = FleetStrength.ToString();


        UpdateColor();
        UpdateRay(Destination);
        UpdateRotation();
      }

      void UpdateColor()
      {
        Capsule.GetComponent<Renderer>().material.color = Color;
        Capsule.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color);
      }

      void UpdateRotation()
      {
        //stuff to update the Capsule rotation towards the target
      }


      public LineRenderer ray;
      void UpdateRay(int dest)
      {
        MainGame mg = GameObject.FindWithTag("GameManager").GetComponent<MainGame>();
        float x = mg.galaxy[dest]["position"][0];
        float y = mg.galaxy[dest]["position"][1];
        ray.SetPosition(1, new Vector3(x, 0, y));
        ray.SetPosition(0, transform.position);
      }



}
