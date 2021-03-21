using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject main;
    [SerializeField]
    private GameObject cultivate;
    [SerializeField]
    private GameObject culrivatePressed;
    [SerializeField]
    private GameObject temperature;
    [SerializeField]
    private GameObject temperaturePressed;
    [SerializeField]
    private GameObject humidity;
    [SerializeField]
    private GameObject humidityPressed;
    [SerializeField]
    private GameObject common;
    [SerializeField]
    private GameObject petriDish;
    [SerializeField]
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        Common(true);
        Main(true);
        Cultivate(false);
        Temperature(false);
        Humidity(false);
        
    }

    public void Main(bool toggle)
    {
        main.SetActive(toggle);
    }

    public void CheckMain()
    {
        if(!cultivate.activeSelf && !temperature.activeSelf && !humidity.activeSelf)
        {
            Main(true);
        }
    }

    public void Cultivate(bool toggle)
    {
        if(main.activeSelf)
        {
            Main(false);
        }

        if(toggle)
        {
            source.Stop();
        }
        cultivate.SetActive(toggle);
        culrivatePressed.SetActive(toggle);
        petriDish.SetActive(toggle);

        CheckMain();
    }

    public void Temperature(bool toggle)
    {
        if (main.activeSelf)
        {
            Main(false);
        }

        temperature.SetActive(toggle);
        temperaturePressed.SetActive(toggle);

        CheckMain();
    }

    public void Humidity(bool toggle)
    {
        if (main.activeSelf)
        {
            Main(false);
        }

        humidity.SetActive(toggle);
        humidityPressed.SetActive(toggle);

        CheckMain();
    }

    public void Common(bool toggle)
    {
        common.SetActive(toggle);
    }
}
