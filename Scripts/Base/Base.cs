using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseDef.Healts
{
    public class Base : MonoBehaviour
    {
        [SerializeField] float baseHealth = 100f;
        [SerializeField] float detectionRadius = 30f;
        [SerializeField] private GameObject endGameUI;
        public float gold = 0f;
        private PhotonView photonView;
        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            EnemyStats.OnEnemyDeath += CheckEnemyProximity; // Ölüm olayýný dinlemeye baþla
        }
        private void OnDisable()
        {
            EnemyStats.OnEnemyDeath -= CheckEnemyProximity; // Ölüm olayýný dinlemeyi býrak
        }
        private void CheckEnemyProximity(Vector3 enemyPosition)
        {
            // Düþmanýn mesafesini kontrol et
            float distance = Vector3.Distance(transform.position, enemyPosition);
            if (distance <= detectionRadius)
            {
                gold += 1; // Altýn miktarýný artýr
                photonView.RPC("UpdateGoldRPC", RpcTarget.All, gold);
                
            }
        }

        public void TakeDamage(float damage)
        {

            if (PhotonNetwork.IsMasterClient)
            {
                baseHealth -= damage;  // Saðlýðý düþür

                
                photonView.RPC("UpdateHealthRPC", RpcTarget.All, baseHealth);

                

                if (baseHealth <= 0)
                {
                    EndGame();//Oyunu bitir
                }
            }
            
   
        }
        [PunRPC]
        private void UpdateHealthRPC(float newHealth)
        {
            baseHealth = newHealth; // Diðer oyuncularýn saðlýk deðerini güncelle
        }

        [PunRPC]
        private void UpdateGoldRPC(float newGold)
        {
            gold = newGold; // Diðer oyuncularýn gold deðerini güncelle
        }


        private void OnTriggerEnter(Collider other)
        {
            // Düþman objesiyle çarpýþtýysa
            if (other.CompareTag("Enemy"))
            {
                // Düþmanýn EnemyStats scriptini al
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();

                if (enemyStats != null)
                {
                    // Düþmanýn verdiði hasarý Base objesine uygula
                    TakeDamage(enemyStats.enemyDMG);
                }
                PhotonView enemyPhotonView = other.GetComponent<PhotonView>();
                if (enemyPhotonView != null && enemyPhotonView.IsMine)
                {
                    PhotonNetwork.Destroy(other.gameObject);
                }
            }
            else if(other.CompareTag("BulletEnemy"))
            {
                
                if (PhotonNetwork.IsMasterClient)
                {
                    baseHealth--;
                    photonView.RPC("UpdateHealthRPC", RpcTarget.All, baseHealth);
                    PhotonNetwork.Destroy(other.gameObject);
                }
                
                if(baseHealth <= 0)
                {
                    EndGame();
                }
            }
        }
        public float GetHealth()
        {
            return baseHealth;
        }
        public float GetGold()
        {
            return gold;
        }
        public void DecreaseGold(float decreaseAmount)
        {
            gold = gold - decreaseAmount;
            photonView.RPC("UpdateGoldRPC", RpcTarget.All, gold);
        }
        private void EndGame()
        {
            // Oyunu durdur
            Time.timeScale = 0;

            // EndGame UI'ýný aktif hale getir
            if (endGameUI != null)
            {
                endGameUI.SetActive(true);
            }
        }
        public void QuitGameButton()
        {
            Application.Quit(); // Oyunu kapat
        }
    }
}

