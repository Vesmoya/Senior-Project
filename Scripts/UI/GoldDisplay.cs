using System;
using UnityEngine;
using TMPro;

namespace BaseDef.Healts
{
    public class GoldDisplay : MonoBehaviour
    {
        [SerializeField] GameObject baseObj = null;
        Base gold;
        TextMeshProUGUI goldText;

        private void Awake()
        {
            gold = baseObj.GetComponent<Base>();

            goldText = GetComponent<TextMeshProUGUI>();

        }


        void Update()
        {
            goldText.text = String.Format("{0:0}", gold.GetGold()); // Gold deðerini ekranda göster
        }
    }
}
