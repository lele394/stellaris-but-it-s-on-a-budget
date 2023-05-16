using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Text.RegularExpressions; //magic to convert floats with a dot



public class Client : MonoBehaviour
{

  TcpClient mySocket;
  public String H = "127.0.0.1";
  public Int32 P = 65432;
  public bool OverrideInputField;

  public string player;

  public InputField Port;
  public InputField IP;
  public InputField PlayerName;

  public Dictionary<string, dynamic> playerList;
  public void yolo()
  {
      DontDestroyOnLoad(this.gameObject);
      setupSocket();
      player = PlayerName.text;
      Send("PLAYERNAME:"+player);

      playerList = GetPlayerList(Send("REQ:PLAYERLIST"));


      //load scene after connection
      SceneManager.LoadScene("Game");




  }

  public NetworkStream theStream;
  StreamWriter theWriter;
  StreamReader theReader;
  public void setupSocket()
  {                            // Socket setup here

      try
      {
          Debug.Log("attempt");

          if(!OverrideInputField)
          {
            mySocket = new TcpClient((string)IP.text, Int32.Parse(Port.text));
          }
          else
          {
            mySocket = new TcpClient(H, P);
          }
          Debug.Log("connected!");
          theStream = mySocket.GetStream();
          theWriter = new StreamWriter(theStream);
          theReader = new StreamReader(theStream);
          theReader.BaseStream.ReadTimeout = 2000; //set listening timeout to 2000ms
          Debug.Log(Send("connnected"));



      }
      catch (Exception e)
      {
          Debug.Log("Socket error: " + e);                // catch any exceptions
      }
  }


  public string Send(string message) //send message, return server answer
  {
    theWriter.Write(message);
    theWriter.Flush();//works till here
    byte[] buffer = new byte[83647];
    int bufferlen = theStream.Read(buffer, 0, 83647);//mySocket.ReceiveBufferSize
    string response =Encoding.ASCII.GetString(buffer, 0, bufferlen);

    Debug.Log("issued : "+ message+"\nserver returned : " + response);
    return response;
  }

  void OnApplicationQuit()
  {
    Send("quit");
  }



  public Dictionary<string, dynamic> GetPlayerList(string input)
  {
    Dictionary<string, dynamic> playerList = new Dictionary<string, dynamic>();
    string[] s = input.Split('/');
    foreach(string player in s)
    {
      string[] split = player.Split(':');
      Dictionary<string, dynamic> playerDic = new Dictionary<string, dynamic>();
      playerDic.Add("color", split[1]);
      Debug.Log("yolo    " + split[2]);
      playerDic.Add("income", IncomeAndRessourcesFloat(split[2]));
      playerDic.Add("ressource", IncomeAndRessourcesFloat(split[3]));

      playerList.Add(split[0], playerDic);
                  // name      data
    }

    return playerList;
  }




  public float IncomeAndRessourcesFloat(string s)
  {
    string[] split = s.Split('.');
    return float.Parse(split[0] + "," + split[1]);
  }
















//temporary, saving camera coordinates here
  public float CamX = 0;
  public float CamZ = 0;









}
