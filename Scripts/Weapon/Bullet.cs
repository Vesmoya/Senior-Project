using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseDef.Weapons
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        [SerializeField] float maxDistance = 32f; // Maksimum mesafe
        private Vector3 startPosition; // Merminin ba�lang�� konumu
        private PhotonView photonView;


        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            startPosition = transform.position;
            
        }
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            
            // Gitti�i toplam mesafeyi hesapla
            float traveledDistance = Vector3.Distance(startPosition, transform.position);

            // E�er toplam mesafe maksimum mesafeyi a�arsa mermiyi yok et
            if (traveledDistance >= maxDistance)
            {
                if (photonView.IsMine || PhotonNetwork.IsMasterClient) // Sadece objeyi olu�turan client yok eder
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                if (photonView.IsMine) // Sadece objeyi olu�turan client yok eder
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
                
        }
    }
}
