using System;
using UnityEngine;
using TMPro;

namespace BaseDef.Healts
{
    public class BaseHpDisplay : MonoBehaviour
    {
        [SerializeField] GameObject baseObj = null;
        Base baseHP;
        TextMeshProUGUI healthText;

        private void Awake()
        {
            baseHP = baseObj.GetComponent<Base>();
            
            healthText = GetComponent<TextMeshProUGUI>();
            
        }

 
        void Update()
        {
            healthText.text = String.Format("BaseHP: {0:0}", baseHP.GetHealth()); // Saðlýk deðerini ekranda göster
        }
    }
}
