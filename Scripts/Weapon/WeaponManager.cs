using UnityEngine;
using BaseDef.Weapons;
using Photon.Pun;

public enum WeaponMode
{
    Single = 1,
    Dual = 2
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject weaponObject1;
    [SerializeField] GameObject weaponObject2;

    private Weapon weapon1;
    private Weapon weapon2;
    private PhotonView view;

    private WeaponMode currentWeaponMode = WeaponMode.Single;

    void Start()
    {
        view = GetComponent<PhotonView>();
        weapon1 = weaponObject1.GetComponent<Weapon>();
        weapon2 = weaponObject2.GetComponent<Weapon>();
    }

    void Update()
    {
        if (!view.IsMine) return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (currentWeaponMode == WeaponMode.Single)
            {
                
                weapon1.ShootAction();
            }
            else if (currentWeaponMode == WeaponMode.Dual)
            {
                
                weapon1.ShootAction();
                weapon2.ShootAction();
            }
        }
    }
    public void SetWeaponMode(WeaponMode mode)
    {
        currentWeaponMode = mode;

        if (mode == WeaponMode.Single)
        {
            weapon1.fireRate = 0.2f; // Hýzlý Atýþ
            weapon2.fireRate = 0.4f; // Çift Silah kullanýlmadýðýnda yavaþlat

        }
        else if (mode == WeaponMode.Dual)
        {
            weapon1.fireRate = 0.4f; // Yavaþ Atýþ
            weapon2.fireRate = 0.4f;
        }

        Debug.Log($"Weapon mode set to: {mode}");
    }


}
