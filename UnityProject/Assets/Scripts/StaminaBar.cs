using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    public float maxStamina = 100;
    public float currentStamina;

    //private WaitForSeconds regenTick = new WaitForSeconds(0.5f);

    public static StaminaBar instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }
    
    public void UseStamina(float amount)
    {
        if(currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;
           
            //StartCoroutine(RegenStamina());
        }
        else
        {            
            //Debug.Log("Not enough stamina");
        }
    }

    public void RegenStamina(float regen)
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += regen;
            staminaBar.value = currentStamina;
        }
    }

    //private IEnumerator RegenStamina()
    //{
    //    yield return new WaitForSeconds(4);
    //    while (currentStamina < maxStamina)
    //    {
    //        currentStamina += maxStamina / 100;
    //        staminaBar.value = currentStamina;
    //        yield return regenTick;
    //    }
    //}
}
