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
        private Vector3 startPosition; // Merminin baþlangýç konumu
        private PhotonView photonView;


        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            startPosition = transform.position;
            
        }
        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            
            // Gittiði toplam mesafeyi hesapla
            float traveledDistance = Vector3.Distance(startPosition, transform.position);

            // Eðer toplam mesafe maksimum mesafeyi aþarsa mermiyi yok et
            if (traveledDistance >= maxDistance)
            {
                if (photonView.IsMine || PhotonNetwork.IsMasterClient) // Sadece objeyi oluþturan client yok eder
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                if (photonView.IsMine) // Sadece objeyi oluþturan client yok eder
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
                
        }
    }
}
