using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndStamina : MonoBehaviour, IDamageable
{
    //health
    public Image healthBar;
    public float healthAmount = 100f;

    //stamina
    public Image stamBar;
    public float stamAmount = 100f;

    // Time to wait before stamina starts to refill
    public float staminaRefillDelay = 1f;

    // Rate at which stamina refills per second
    public float staminaRefillRate = 2.5f;
    private Coroutine refillCoroutine;
    private bool isStaminaRefilling = false;


    // Update is called once per frame
    void Update()
    {
        if (stamAmount < 100 && !isStaminaRefilling && refillCoroutine == null)
        {
          Debug.Log("Starting StaminaRefill coroutine");
          refillCoroutine = StartCoroutine(StaminaRefill());
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }

    public void TakeStamina(float Stamina)
    {
        stamAmount -= Stamina;
        stamBar.fillAmount = stamAmount / 100;
    }

    

    IEnumerator StaminaRefill()
    {
        Debug.Log("StaminaRefill coroutine started");
        yield return new WaitForSeconds(2f);

        //refills stamina
        while (stamAmount < 100)
        {
            stamAmount += staminaRefillRate * Time.deltaTime;
            stamAmount = Mathf.Clamp(stamAmount, 0, 100);

            stamBar.fillAmount = stamAmount / 100f;

            yield return null;
        }

        isStaminaRefilling = false;
        refillCoroutine = null; // Reset the coroutine reference
        Debug.Log("StaminaRefill coroutine completed");
    }

   
}
