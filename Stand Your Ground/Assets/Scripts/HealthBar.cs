using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    private Image healthBarImage;
    [SerializeField]
    private Image iconGreen;
    [SerializeField]
    private Image iconRed;
    private PlayerController player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public void UpdateHealthBar(float health)
    {
        healthBarImage.fillAmount = health / 100f;

        if (health < 50f){
            iconRed.gameObject.SetActive(true);
            iconGreen.gameObject.SetActive(false);
            healthBarImage.color = Color.red;
        }
        else{
            iconRed.gameObject.SetActive(false);
            iconGreen.gameObject.SetActive(true);
            healthBarImage.color = Color.green;
        }
    }
}
