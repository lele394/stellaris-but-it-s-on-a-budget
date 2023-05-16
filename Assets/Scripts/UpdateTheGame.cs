using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateTheGame : MonoBehaviour
{
  public void JustReloadTheWholeThing()
  {
    Camera.main.GetComponent<CamMovement>().SavePosition();
    SceneManager.LoadScene("Game");
  }
}
