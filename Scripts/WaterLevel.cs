using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterLevel : MonoBehaviour
{
    //Amount of ice to melt
    [Range(0.0f, 100000000.0f)]
    public float iceVolume = 22851772.7f; //In Gt; Value provided is the amount of 'effective' ice in Antarctica
    //Current water level in the simulation; Defined at the start of simulation, changes over the course of it
    [Range(1.0f, 500.0f)]
    public double waterHeight = 35.25;
    public float CO2 = 1f;
    public float temperature = 0.63f;
    //Ice change per year
    private float icePerYear = -666f; //In Gt; Value provided is the global ice change per year
    public float year = 2002;
    //Amount of water
    [Range(0, 2000000000)]
    public float waterWeight = 1367409240f; //in Gt; Value provided is the amount of ocean and sea water on earth
    private float thermal = 0;
    private float massTranition = 0;
    
    [Range(-3, 3)]
    public float deltaT = 0.18f; //In K; Average temperature increase over one year
    //Speed at which the simulation should work
    public int simulationSpeed = 1;

    private float initialIceVolume;

    private double initialWaterHeight;
    private float pre;

    void Start() {
      initialIceVolume = iceVolume;
      initialWaterHeight = waterHeight;
      SetSimulationSpeed(simulationSpeed);
      TogglePopup(false);
      StartCoroutine(WaitFor90());
    }

    void Update() {
      //Apply new Height
      float dh =  pre + ((float) (waterHeight - pre) * Time.deltaTime * 100);
      if (dh > waterHeight) dh = (float) waterHeight;
      gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("Vector1_CA004389", dh);
      pre = dh;
    }

    void UpdateYear() {
        /*deltaT = */
        icePerYear = (float) -8 * Mathf.Pow(year - 2001, 2);
        CO2 = (float) 0.0018 * Mathf.Pow(year - 2001, 1.65f);
        deltaT = (float) 0.0001 * Mathf.Pow(year - 2001, 2f) - temperature;
        temperature = (float) 0.0001 * Mathf.Pow(year - 2001, 2f);

        //Calculate new height
        if((iceVolume > Mathf.Abs(icePerYear) && icePerYear < 0) || (icePerYear > 0 && waterHeight > 1f)) {
            //Main phase
            massTranition = -icePerYear;
            double delta = massTranition * (1f / 361.8f);
            waterHeight += delta / 1000; //In m
            iceVolume -= massTranition;
            thermal += 0.08f * (deltaT + 0.375f) + 2.5f * deltaT; //frametime * (simulationSpeed * 100) * constantA(~0.08) * (T - T0) + constantB(~2.5) * temperatureChange
            waterWeight += massTranition + thermal;
        } else if(waterHeight > 1f) {
            //Secondary phase; Is more precise with small numbers
            massTranition = iceVolume * 0.9167f;
            double delta = massTranition * (1f / 361.8f); //In mm; VERY small number; Formula used: volume * simulationSpeed * density of glacier ice * constant * frameTime; *could be optimized*
            waterHeight += delta / 1000; //In m
            iceVolume = 0;
            waterWeight += massTranition;
            CancelInvoke();
            TogglePopup(true);
        }

        if (waterHeight < 1) waterHeight = 1f;

        //Update year
        year++;
        UpdateUI();
    }

    void UpdateUI() {
      Text text = GameObject.Find("WaterLevelValue").GetComponent<Text>();   
      text.text = System.Math.Round(waterHeight - initialWaterHeight, 2) + "m";   

      text = GameObject.Find("SimulationYearValue").GetComponent<Text>();  
      text.text = Mathf.Round(year).ToString();

      text = GameObject.Find("IceMeltedValue").GetComponent<Text>();
      text.text = System.Math.Round((1 - iceVolume / initialIceVolume) * 100, 1) + "%";

      text = GameObject.Find("SimulationSpeedValue").GetComponent<Text>();
      text.text = simulationSpeed + " yrs/s";

      text = GameObject.Find("IceValue").GetComponent<Text>();
      text.text = iceVolume + " Gt";

      text = GameObject.Find("TempValue").GetComponent<Text>();
      text.text = temperature + "°C";

      text = GameObject.Find("COValue").GetComponent<Text>();
      text.text = CO2 + " ppm";
    }

    public void SetSimulationSpeed(int speed) {
      simulationSpeed = speed;

      CancelInvoke();
      InvokeRepeating("UpdateYear", 0, 1f / simulationSpeed);
    }

    IEnumerator WaitFor90()
    {
      yield return new WaitForSeconds(60);
      TogglePopup(true);
      SetSimulationSpeed(0);
    }

    public void TogglePopup(bool show){
      GameObject obj = GameObject.Find("OverPopup");
      var getCanvasGroup  = obj.GetComponent<CanvasGroup>();
      GameObject.Find("FinalWaterLevel").GetComponent<Text>().text = "Final water height was: " + System.Math.Round(waterHeight, 1) + "m";

      if (show) {
          getCanvasGroup.alpha = 1;
          getCanvasGroup.interactable = true;

      } else {
          getCanvasGroup.alpha = 0;
          getCanvasGroup.interactable = false;
      }
    }
}

/*double delta = iceVolume * (speed/100f) * 0.9167f * (1f / 361.8f) * Time.deltaTime; //In mm; VERY small number; Formula used: volume * simulationSpeed * density of glacier ice * constant * frameTime; *could be optimized*
waterHeight += delta / 1000; //In m
iceVolume -= iceVolume * Time.deltaTime * (speed/100f);

if(iceVolume < 0.1f) iceVolume = 0f;*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterLevel : MonoBehaviour
{
    //Amount of ice to melt
    [Range(0.0f, 100000000.0f)]
    public float iceVolume = 22851772.7f; //In Gt; Value provided is the amount of 'effective' ice in Antarctica
    //Speed at which the simulation should work
    [Range(0.0f, 100.0f)]
    public float speed = 1f;
    //Current water level in the simulation; Defined at the start of simulation, changes over the course of it
    [Range(1.0f, 500.0f)]
    public double waterHeight = 35.25;
    public float CO2 = 1f;
    public float temperature = 0.63f;
    //Ice change per year
    private float icePerYear = -666f; //In Gt; Value provided is the global ice change per year
    public float year = 2002;
    //Amount of water
    [Range(0, 2000000000)]
    public float waterWeight = 1367409240f; //in Gt; Value provided is the amount of ocean and sea water on earth
    private float thermal = 0;
    private float massTranition = 0;
    
    [Range(-3, 3)]
    public float deltaT = 0.18f; //In K; Average temperature increase over one year

    public int simulationSpeed = 1;

    private float initialIceVolume;

    private double initialWaterHeight;
    float pre;

    void Start() {
      initialIceVolume = iceVolume;
      initialWaterHeight = waterHeight;
      gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("Vector1_CA004389", (float) waterHeight);
      SetSimulationSpeed(simulationSpeed);
      pre = (float) waterHeight;
    }

    void Update() {
      //Apply new Height
      float dh =  pre + ((float) (waterHeight - pre) * Time.deltaTime * 100);
      if (dh > waterHeight) dh = (float) waterHeight;
      gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("Vector1_CA004389", dh);
      pre = dh;
    }

    void UpdateYear() {

        icePerYear = (float) -8 * Mathf.Pow(year - 2001, 2);
        CO2 = (float) 0.0018 * Mathf.Pow(year - 2001, 1.65f);
        deltaT = (float) 0.0001 * Mathf.Pow(year - 2001, 2f) - temperature;
        temperature = (float) 0.0001 * Mathf.Pow(year - 2001, 2f);

        //Calculate new height
        if((iceVolume > Mathf.Abs(icePerYear) && icePerYear < 0) || (icePerYear > 0 && waterHeight > 1f)) {
            //Main phase
            massTranition = -icePerYear;
            double delta = massTranition * (1f / 361.8f);
            waterHeight += delta / 1000; //In m
            iceVolume -= massTranition;
            thermal += 0.08f * (deltaT + 0.375f) + 2.5f * deltaT; //frametime * (simulationSpeed * 100) * constantA(~0.08) * (T - T0) + constantB(~2.5) * temperatureChange
            waterWeight += massTranition + thermal;
        } else if(waterHeight > 1f) {
            //Secondary phase; Is more precise with small numbers
            massTranition = iceVolume * 0.9167f;
            double delta = massTranition * (1f / 361.8f); //In mm; VERY small number; Formula used: volume * simulationSpeed * density of glacier ice * constant * frameTime; *could be optimized*
            waterHeight += delta / 1000; //In m
            iceVolume = 0;
            waterWeight += massTranition;
            CancelInvoke();
        }

        if (waterHeight < 1) waterHeight = 1f;

        //Update year
        year++;
        UpdateUI();
    }

    void UpdateUI() {
      Text text = GameObject.Find("WaterLevelValue").GetComponent<Text>();   
      text.text = System.Math.Round(waterHeight - initialWaterHeight, 2) + "m";   

      text = GameObject.Find("SimulationYearValue").GetComponent<Text>();  
      text.text = Mathf.Round(year).ToString();

      text = GameObject.Find("IceMeltedValue").GetComponent<Text>();
      text.text = System.Math.Round((1 - iceVolume / initialIceVolume) * 100, 1) + "%";

      text = GameObject.Find("SimulationSpeedValue").GetComponent<Text>();
      text.text = simulationSpeed + " yrs/s";

      text = GameObject.Find("IceValue").GetComponent<Text>();
      text.text = iceVolume + " Gt";

      text = GameObject.Find("TempValue").GetComponent<Text>();
      text.text = temperature + "°C";

      text = GameObject.Find("COValue").GetComponent<Text>();
      text.text = CO2 + " ppm";
    }

    public void SetSimulationSpeed(int speed) {
      simulationSpeed = speed;

      CancelInvoke();
      InvokeRepeating("UpdateYear", 0, 1f / simulationSpeed);
    }
}
*/
/*double delta = iceVolume * (speed/100f) * 0.9167f * (1f / 361.8f) * Time.deltaTime; //In mm; VERY small number; Formula used: volume * simulationSpeed * density of glacier ice * constant * frameTime; *could be optimized*
waterHeight += delta / 1000; //In m
iceVolume -= iceVolume * Time.deltaTime * (speed/100f);

if(iceVolume < 0.1f) iceVolume = 0f;*/
