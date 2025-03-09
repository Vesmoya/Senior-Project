using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace BaseDef.Weapons
{
    public class EnemyWeaponManager : MonoBehaviour
    {
        [SerializeField] GameObject weaponObject1;
        [SerializeField] float fireRate = 2f; // Ateþ etme aralýðý (saniye)
        

        private Weapon weapon1;
        private Transform target;
        private Coroutine fireCoroutine;
        


        void Start()
        {

            if (!PhotonNetwork.IsMasterClient)
            {
                return; // Master Client deðilse script'i çalýþtýrma
            }
            weapon1 = weaponObject1.GetComponent<Weapon>();
            FindBase();

            StartFiring();

        }

        void FindBase()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 45f); // Varsayýlan detectionRange
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Base"))
                {
                    target = hitCollider.transform;
                    break;
                }
            }
        }

       

        IEnumerator FireAtIntervals()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);

                if (target != null)
                {
                    weapon1.ShootAction();
                }

                
                
            }
        }
        private void StartFiring()
        {
            if (target != null)
            {
                fireCoroutine = StartCoroutine(FireAtIntervals());
            }
        }
        public void StopShooting()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
                
            }
        }
        public void RestartShooting()
        {
            if (fireCoroutine == null)
            {
                StartFiring();
                
            }
        }
    }
}
