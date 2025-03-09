using UnityEngine;
using Photon.Pun;

public class WeaponSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject weaponSelectionUI; // Silah Seçim UI Prefab
    private WeaponManager weaponManager;
    

    void Start()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject playerObj = GameObject.Find("Player (" + playerIndex + ")");
        if (playerObj != null)
        {
            weaponManager = playerObj.GetComponent<WeaponManager>();
        }
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.IsLocal)
        {

            weaponSelectionUI.SetActive(true);
        }
    }


    public void SelectWeaponMode(int mode)
    {
        if (mode == 1)
        {
            
            weaponManager.SetWeaponMode(WeaponMode.Single);
        }
        else if (mode == 2)
        {
            weaponManager.SetWeaponMode(WeaponMode.Dual);
        }

        Destroy(weaponSelectionUI);
    }
}
