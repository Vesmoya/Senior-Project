using BaseDef.Weapons;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseDef.Healts
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] public float enemyHP = 10f;
        [SerializeField] public float spawnHP = 10f; // Düþman doðduðunda saðlýk
        [SerializeField] public float enemyDMG = 5f;
        [SerializeField] bool isSpecial = false;
        [SerializeField] bool isRange = false;

        private EnemyMovement enemyMovement;
        private EnemyWeaponManager enemyWeaponManager;
        private Collider enemyCollider;
        private Transform specialTarget; // Special durumdaki yeni hedef
        private PhotonView photonView;

        public static event System.Action<Vector3> OnEnemyDeath;

        void Start()
        {
            photonView = GetComponent<PhotonView>();
            enemyMovement = GetComponent<EnemyMovement>();
            enemyWeaponManager = GetComponent<EnemyWeaponManager>();
            enemyCollider = GetComponent<Collider>();
            spawnHP = enemyHP; // Doðal saðlýk deðeri
            AssignSpecialStatus();
        }

        void AssignSpecialStatus()
        {
            // %10 ihtimalle özel düþman yap
            if (Random.Range(0f, 1f) <= 0.1f)
            {
                isSpecial = true;
                
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                // Sadece MasterClient hasar iþlemeli
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 1f);

                    // Bullet yok etme iþlemi
                    PhotonView bulletPhotonView = other.GetComponent<PhotonView>();
                    if (bulletPhotonView != null && bulletPhotonView.IsMine)
                    {
                        PhotonNetwork.Destroy(other.gameObject);
                    }
                }
            }
        }

        [PunRPC]
        void TakeDamage(float damage)
        {
            enemyHP -= damage;
            

            if (enemyHP <= 0)
            {
                OnEnemyDeath?.Invoke(transform.position);

                if (isSpecial)
                {
                    HandleSpecialEnemyLogic();
                }
                else
                {
                    if (PhotonNetwork.IsMasterClient || photonView.IsMine)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }

        void HandleSpecialEnemyLogic()
        {
            // Collider'ý devre dýþý býrak ve yeni hedefe doðru hareket et
            enemyCollider.enabled = false;

            // Yeni hedefi (en uzak Base) bulup yönlendir
            SetSpecialTarget();

            // Hedefe doðru hareket et
            enemyMovement.ChangeTarget(specialTarget);
            if (isRange)
            {
                enemyMovement.agent.speed = 2f;
                enemyWeaponManager.StopShooting();
            }

            // Respawn iþlemini baþlat
            StartCoroutine(HandleSpecialEnemyRespawn());
        }

        void SetSpecialTarget()
        {
            // Sahnedeki tüm "Base" etiketine sahip objeleri bul
            GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");

            // En uzak Base objesini bulmak için baþlangýçta bir referans oluþturuyoruz
            GameObject furthestBase = null;
            float maxDistance = 0f;

            // Þu anki düþmanýn pozisyonuna göre en uzak base objesini seçiyoruz
            foreach (GameObject baseObj in bases)
            {
                float distanceToBase = Vector3.Distance(transform.position, baseObj.transform.position);

                if (distanceToBase > maxDistance)
                {
                    maxDistance = distanceToBase;
                    furthestBase = baseObj;
                }
            }

            // En uzak Base'i specialTarget olarak ayarlýyoruz
            if (furthestBase != null)
            {
                specialTarget = furthestBase.transform;
            }
        }

        IEnumerator HandleSpecialEnemyRespawn()
        {
            // Düþmanýn yeni hedefe hareket etmesini bekleyelim
            while (Vector3.Distance(transform.position, specialTarget.position) > 29f)
            {
                yield return null; // Düþman hedefe hareket ederken bu döngü devam eder
            }

            if (isRange)
            {
                enemyMovement.agent.speed = 0f;
                enemyWeaponManager.RestartShooting();
            }

            // Hedefe 30f mesafeye geldiðinde collider'ý tekrar aktif yap
            enemyCollider.enabled = true;

            // Saðlýk sýfýrlanýr
            photonView.RPC("RespawnEnemy", RpcTarget.AllBuffered);
        }

        [PunRPC]
        void RespawnEnemy()
        {
            enemyHP = spawnHP;
            isSpecial = false;
            
        }
    }
}
