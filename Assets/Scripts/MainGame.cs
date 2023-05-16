using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{

    Client client;
    public Dictionary<int, dynamic> galaxy;
    public Dictionary<int, dynamic> moving_fleets;

    void Start()
    {
      Client client =GameObject.FindWithTag("Client").GetComponent<Client>();
      GetMap(client);

      gameObject.GetComponent<Map>().MakeMap(galaxy); //makes the map

      GetMovingFleets(client);

      gameObject.GetComponent<MovingFleets>().PlaceMovingFleets(moving_fleets);



    }

















    public void GetMovingFleets(Client client)
    {
      string data = client.Send("REQ:MOVINGFLEETSLENGTH");
      int reste = int.Parse(data) % 50;
      int numberOfFifty = int.Parse(data) / 50;

      List<string> sub = new List<string>();


      for (int i = 0; i<numberOfFifty; i++)
      {
        data = client.Send("REQ:MOVINGFLEETS:" + (50 * i).ToString() + ":" + (50 * (i+1)) );
        sub = AddfleetsToStringList(data, "/", sub);
      }
      if(reste!=0)
      {
        data = client.Send("REQ:MOVINGFLEETS:"+ (numberOfFifty*50).ToString() + ":" + (numberOfFifty*50 + reste).ToString());
        sub = AddfleetsToStringList(data, "/", sub);
      }

      moving_fleets = StringListToMovingFleetsDictionnary(sub);

    }



    public List<string> AddfleetsToStringList(string input, string separator, List<string> list)
    {
      string[] s = input.Split('/');
      foreach (string fleet in s)
      {
        list.Add(fleet);
      }
      return list;
    }

    public Dictionary<int, dynamic> StringListToMovingFleetsDictionnary(List<string> list)
    {
      Dictionary<int, dynamic> mf = new Dictionary<int, dynamic>();
      int c = 0;

      foreach (string fleet in list)
      {
        string[] split = fleet.Split(':');
        Dictionary<string, dynamic> sub = new Dictionary<string, dynamic>();
        sub.Add("power",       int.Parse(split[0]));
        sub.Add("destination",      int.Parse(split[1]));
        sub.Add("position",    GetFleetPosition(split[2])); //system position parsing should work the same as for fleets, not sure tho
        sub.Add("owner",      split[3]);

        mf.Add(c, sub);
        c++;
      }
      return mf;
    }

    public float[] GetFleetPosition(string s) //different because python representation of fleets uses tuple and not lists, so uses '()' instead of '[]'
    {
      string[] split = s.Split(',');

      //makes x
      float x = 0f;
      string trimmed = split[0].Trim('(');
      string[] trimSplit = trimmed.Split('.');
      x = float.Parse(trimSplit[0].Trim('[') + ',' + trimSplit[1]);


      //makes y
      float y = 0f;
      trimmed = split[1].Trim(' ').Trim(')');
      trimSplit = trimmed.Split('.');
      y = float.Parse(trimSplit[0] + ',' + trimSplit[1].Trim(']'));

      // 54.618241960604465)
      float[] pos = {x, y};
      return pos;
    }




















    public void GetMap(Client client)
    {
      string data = client.Send("REQ:GAMEMAPLENGTH");
      int reste = int.Parse(data) % 50;
      int numberOfFifty = int.Parse(data) / 50;


      Dictionary<int, dynamic> IdkHowIshouldCallU = new Dictionary<int, dynamic>();
      List<string> sub = new List<string>();

      for (int i = 0; i<numberOfFifty; i++)
      {
        data = client.Send("REQ:GAMEMAP:" + (50 * i).ToString() + ":" + (50 * (i+1)) );
        sub = AddSystemsToStringList(data, "/", sub);
      }
      if(reste!=0)
      {
        data = client.Send("REQ:GAMEMAP:"+ (numberOfFifty*50).ToString() + ":" + (numberOfFifty*50 + reste).ToString());
        sub = AddSystemsToStringList(data, "/", sub);
      }

      galaxy = StringListToGalaxyDictionnary(sub);
    }

    public Dictionary<string, dynamic> GetSystemInfos(string s)
    {
      Dictionary<string, dynamic> system = new Dictionary<string, dynamic>();
      return system;
    }

    public List<string> AddSystemsToStringList(string input, string separator, List<string> list)
    {
      string[] s = input.Split('/');
      foreach (string system in s)
      {
        list.Add(system);
      }
      return list;

    }

    public Dictionary<int, dynamic> StringListToGalaxyDictionnary(List<string> list)
    {
      Dictionary<int, dynamic> galax = new Dictionary<int, dynamic>();
      int c = 0;

      foreach (string system in list)
      {
        string[] split = system.Split(':');
        Dictionary<string, dynamic> sub = new Dictionary<string, dynamic>();
        sub.Add("name",       split[0]);
        sub.Add("owner",      split[1]);
        sub.Add("station",    GetStationInfos(split[2]));
        sub.Add("fleet",      int.Parse(split[3]));
        sub.Add("anomaly",    split[4]);
        sub.Add("position",   GetSystemPosition(split[5]));

        galax.Add(c, sub);
        c++;
      }
      return galax;
    }

    public Dictionary<string, dynamic> GetStationInfos(string s)
    {
      Dictionary<string, dynamic> stat = new Dictionary<string, dynamic>();
      s = s.Substring(1, s.Length - 2);
      string[] split = s.Split(',');

      //==== HANDLES EXCEPTION IF = 0
      int power = 0;
      if(split[0].Length == 1)
      {
        power = int.Parse(split[0]);
      }
      //==== IF != 0
      else
      {
        power = int.Parse(split[0].Substring(1, split[0].Length - 2));
      }

      //==== HANDLES EXCEPTION IF = 0
      int level = 0;
      if(split[1].Length == 2)
      {
        level = int.Parse(split[1].Trim(' '));
      }
      //==== IF != 0
      else
      {
        level = int.Parse(split[1].Substring(2, split[1].Length - 3));// 2 because space in front of it and -3 bcuz y tf not
      }
      //int power = int.Parse(split[0]);
      //int level = int.Parse(split[1]);
      //eventually add station modules loader here
      string[] modules = {};

      stat.Add("power", power);
      stat.Add("level", level);
      stat.Add("modules", modules);

      return stat;

    }

    public float[] GetSystemPosition(string s)
    {

      string[] split = s.Split(',');

      //makes x     (-6.468975586046546, 54.618241960604465)
      float x = 0f;
      string trimmed = split[0].Trim('[').Trim('(');
      string[] trimSplit = trimmed.Split('.');
      x = float.Parse(trimSplit[0] + ',' + trimSplit[1]);
      //(-6.468975586046546


      //makes y
      float y = 0f;
      trimmed = split[1].Trim(' ').Trim(']').Trim(')');
      trimSplit = trimmed.Split('.');
      y = float.Parse(trimSplit[0] + ',' + trimSplit[1]);

      // 54.618241960604465)
      float[] pos = {x, y};
      return pos;
    }




}
