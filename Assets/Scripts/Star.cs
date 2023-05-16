using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public int ID = 0;
    public Color Color;
    public string Name;
    public string Owner;
    public int Fleet;
    public Client client;

    void Start()
    {
      Client client =GameObject.FindWithTag("Client").GetComponent<Client>();
      if(Owner != "none" && Owner != "pirate")
      {
        string colHex = client.playerList[Owner]["color"];
        ColorUtility.TryParseHtmlString(colHex, out Color);
      }


      UpdateColor();
      UpdateFleet();
      UpdateName();
    }

    void UpdateColor()
    {
      gameObject.GetComponent<Renderer>().material.color = Color;
      gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color);
    }

    public TextMesh NameText;
    void UpdateName()
    {
      NameText.text = Name;
    }

    public TextMesh FleetText;
    void UpdateFleet()
    {
      FleetText.text = Fleet.ToString();
    }
}
