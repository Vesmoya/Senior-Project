using Photon.Pun;
using UnityEngine;

namespace BaseDef.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] GameObject bullet;
        [SerializeField] Transform bulletSpawn;
        [SerializeField] float bulletSpeed = 10f;
        public float fireRate = 0.5f;
        private float shotTimer = 0f;
        

        
        public void ShootAction()
        {
            
            if (Time.time >= shotTimer + fireRate)
            {
                Shoot();
                shotTimer = Time.time;
            }
        }

        public void Shoot()
        {
            
                GameObject bulletObj = PhotonNetwork.Instantiate(bullet.name, bulletSpawn.position, bulletSpawn.rotation);
                Bullet bulletFile = bullet.GetComponent<Bullet>();
                bulletFile.speed = bulletSpeed;
            
            
        }
    }
}
