using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
  private WaterLevel waterController;

  private int water = 20;
  private int ice = 1;
  private int year = 2002;

  private int clutter = 1;

  private bool paused = false;
  public AudioSource aSource;

  void Start() {
    DontDestroyOnLoad(gameObject);
    HandleYear(1);
    HandleWater(1);
    HandleIce(1);
    HandleClutter(1);
    if(GetComponent<AudioSource>()) {
      aSource = GetComponent<AudioSource>();
    }else {
      aSource = gameObject.AddComponent<AudioSource>();
    }
  }

  public void StartGame() {
    aSource.Play();
    var operation = SceneManager.LoadSceneAsync("TerrainTest", LoadSceneMode.Single);
    operation.completed += (s) => {
      WaterLevel waterController = GameObject.Find("Water").GetComponent<WaterLevel>();

      waterController.year = year;
      waterController.waterHeight = water;
      waterController.iceVolume = ice;

      GameObject.Find("ClutterGen").GetComponent<ClutterGen>().clutterCount = clutter;
    };
  }

  public void Exit() {
    aSource.Play();
    Application.Quit();
  }

  public void Credits() {
    aSource.Play();
    SceneManager.LoadScene("Credits");
  }

  public void Back() {
    aSource.Play();
    SceneManager.LoadScene("MainMenu");
  }

  public void HandleYear(int v) {
    if (GameObject.Find("YearSlider")) {
      year = (int) GameObject.Find("YearSlider").GetComponent<Slider>().value;
      GameObject.Find("YearText").GetComponent<Text>().text = "Starting Year: " + year;
    }
 }

  public void HandleIce(int v) {
    if (GameObject.Find("IceSlider")) {
      ice = (int) GameObject.Find("IceSlider").GetComponent<Slider>().value;
      GameObject.Find("IceText").GetComponent<Text>().text = "Starting Ice Volume: " + ice + " Gt";
    }
  }
  public void HandleWater(int v) {
    if ( GameObject.Find("WaterSlider")) {
      water = (int) GameObject.Find("WaterSlider").GetComponent<Slider>().value;
      GameObject.Find("WaterText").GetComponent<Text>().text = "Starting Water Height: " + water + "m";
    }
  }

  public void HandleClutter(int v) {
    if (GameObject.Find("ClutterSlider")) {
      clutter = (int) GameObject.Find("ClutterSlider").GetComponent<Slider>().value;
      GameObject.Find("ClutterText").GetComponent<Text>().text = "Clutter Amount: " + clutter;
    }
  }
}
