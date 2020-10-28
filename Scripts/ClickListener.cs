using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickListener : MonoBehaviour
{
  private WaterLevel waterController;

  private bool paused = false;
  public AudioSource aSource;

  void Start() {
    if(GameObject.Find("Water")) waterController = GameObject.Find("Water").GetComponent<WaterLevel>();
    if(GetComponent<AudioSource>()) {
      aSource = GetComponent<AudioSource>();
    }else {
      aSource = gameObject.AddComponent<AudioSource>();
    }
  }

  public void IncreaseSpeed() {
    waterController.SetSimulationSpeed(Mathf.Min(512, waterController.simulationSpeed * 2));
    aSource.Play();
  }
  public void DecreaseSpeed() {
    waterController.SetSimulationSpeed(Mathf.Max(1, waterController.simulationSpeed / 2));
    aSource.Play();
  }

  public void Reset() {
    SceneManager.LoadScene("MainMenu");
    aSource.Play();
  }

  public void Back() {
    SceneManager.LoadScene("MainMenu");
    aSource.Play();
  }

  public void Pause() {
    aSource.Play();
    paused = !paused;
    waterController.SetSimulationSpeed(paused ? 0 : 1);

    if (gameObject.transform.GetChild(0).gameObject.GetComponent<Text>()) {
      gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = paused ? "Resume" : "Pause";
    }
  }
}
