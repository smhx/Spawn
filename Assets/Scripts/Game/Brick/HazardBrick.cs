using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBrick : MonoBehaviour
{

    [SerializeField]
    GameObject hazard;
    [SerializeField]
    GameObject warning;
    [SerializeField]
    GameObject standard;

    SpriteRenderer sr;
    float timeInSecond = 0f;
    float hazardTime = 0f;
    bool isHazard = false;
    bool isWarning = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        sr.enabled = true;
        standard.SetActive(true);
        hazard.SetActive(false);
        warning.SetActive(false);
        timeInSecond = 0f;
        hazardTime = 0f;
        isHazard = false;
        isWarning = false;
    }
    
    void OnDisable()
    {
        standard.SetActive(true);
        hazard.SetActive(false);
        warning.SetActive(false);
    }

    void Update()
    {
        if (!isWarning && !isHazard)
            return;
       
        if (isHazard)
        {
            if (hazardTime > 0)
                hazardTime -= Time.deltaTime;
            else //turn off hazard
            {
                sr.enabled = true;
                hazard.SetActive(false);
                warning.SetActive(false);
                standard.SetActive(true);
                isHazard = false;
            }
        }
        timeInSecond += Time.deltaTime;
        if (timeInSecond > 1f)
            timeInSecond = 0f;
    }

    public void StartWarning()
    {
		Audio.Instance.PlayWarning ();
        isWarning = true;
        sr.enabled = false;
        standard.SetActive(false);
        hazard.SetActive(false);
        warning.SetActive(true);
        timeInSecond = 0f;
    }

    public void StartHazard(float dur)
    {
		Audio.Instance.PlayHazard ();
        isWarning = false;
        isHazard = true;
        hazardTime = dur;

        hazard.SetActive(true);
        warning.SetActive(false);
        standard.SetActive(false);
    }

    public bool CurrentHazard() { return isHazard; }
    public bool CurrentWarning() { return isWarning; }
}
