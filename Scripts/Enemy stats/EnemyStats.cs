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
        [SerializeField] public float spawnHP = 10f; // D��man do�du�unda sa�l�k
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
            spawnHP = enemyHP; // Do�al sa�l�k de�eri
            AssignSpecialStatus();
        }

        void AssignSpecialStatus()
        {
            // %10 ihtimalle �zel d��man yap
            if (Random.Range(0f, 1f) <= 0.1f)
            {
                isSpecial = true;
                
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                // Sadece MasterClient hasar i�lemeli
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 1f);

                    // Bullet yok etme i�lemi
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
            // Collider'� devre d��� b�rak ve yeni hedefe do�ru hareket et
            enemyCollider.enabled = false;

            // Yeni hedefi (en uzak Base) bulup y�nlendir
            SetSpecialTarget();

            // Hedefe do�ru hareket et
            enemyMovement.ChangeTarget(specialTarget);
            if (isRange)
            {
                enemyMovement.agent.speed = 2f;
                enemyWeaponManager.StopShooting();
            }

            // Respawn i�lemini ba�lat
            StartCoroutine(HandleSpecialEnemyRespawn());
        }

        void SetSpecialTarget()
        {
            // Sahnedeki t�m "Base" etiketine sahip objeleri bul
            GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");

            // En uzak Base objesini bulmak i�in ba�lang��ta bir referans olu�turuyoruz
            GameObject furthestBase = null;
            float maxDistance = 0f;

            // �u anki d��man�n pozisyonuna g�re en uzak base objesini se�iyoruz
            foreach (GameObject baseObj in bases)
            {
                float distanceToBase = Vector3.Distance(transform.position, baseObj.transform.position);

                if (distanceToBase > maxDistance)
                {
                    maxDistance = distanceToBase;
                    furthestBase = baseObj;
                }
            }

            // En uzak Base'i specialTarget olarak ayarl�yoruz
            if (furthestBase != null)
            {
                specialTarget = furthestBase.transform;
            }
        }

        IEnumerator HandleSpecialEnemyRespawn()
        {
            // D��man�n yeni hedefe hareket etmesini bekleyelim
            while (Vector3.Distance(transform.position, specialTarget.position) > 29f)
            {
                yield return null; // D��man hedefe hareket ederken bu d�ng� devam eder
            }

            if (isRange)
            {
                enemyMovement.agent.speed = 0f;
                enemyWeaponManager.RestartShooting();
            }

            // Hedefe 30f mesafeye geldi�inde collider'� tekrar aktif yap
            enemyCollider.enabled = true;

            // Sa�l�k s�f�rlan�r
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
