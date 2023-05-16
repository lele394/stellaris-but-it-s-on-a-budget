using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class test : MonoBehaviour
{

    public InputField sendStuff;
    public void click()
    {
      Client client =GameObject.FindWithTag("Client").GetComponent<Client>();
      client.Send(sendStuff.text);
    }

}
