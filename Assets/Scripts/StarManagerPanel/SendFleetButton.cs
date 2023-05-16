using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SendFleetButton : MonoBehaviour
{



    public Client client;

    void Start()
    {
      client = GameObject.FindWithTag("Client").GetComponent<Client>();
    }


    public StarManager StarManager;
    public void SendFleetButtonFunction()
    {

      int destination = StarManager.DestinationID;
      int origin = StarManager.ID;
      int power = StarManager.SelectedFleetPower;

      string answer = client.Send("ACT:CREATEFLEET:" + origin.ToString() + ":" + destination.ToString() + ":" + power.ToString());

      GameObject.FindWithTag("UpdateButton").GetComponent<UpdateTheGame>().JustReloadTheWholeThing();
    }






}
