using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPun
{
    [SerializeField] GameObject playerPrefab1;
    [SerializeField] GameObject playerPrefab2;
    private GameObject player1;
    private GameObject player2;
    private WeaponManager weaponManager;
    [SerializeField] private GameObject weaponSelectionUI1;
    [SerializeField] private GameObject weaponSelectionUI2;
    [SerializeField] GameObject enemySpawner1;
    [SerializeField] GameObject enemySpawner2;


    public float p1X;
    public float p1Y;
    public float p1Z;
    public float p2X;
    public float p2Y;
    public float p2Z;
    void Start()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber; // Oyuncunun sýra numarasý
        if (playerIndex == 1)
        {
            Vector3 p1Position = new Vector3(p1X, p1Y, p1Z);
            player1 = PhotonNetwork.Instantiate(playerPrefab1.name, p1Position, Quaternion.identity);
            weaponManager = player1.GetComponent<WeaponManager>();
            weaponSelectionUI1.SetActive(true);

        }
        else if (playerIndex == 2)
        {
            Vector3 p2Position = new Vector3(p2X, p2Y, p2Z);
            player2 = PhotonNetwork.Instantiate(playerPrefab2.name, p2Position, Quaternion.identity);
            weaponManager = player2.GetComponent<WeaponManager>();
            weaponSelectionUI2.SetActive(true);
            photonView.RPC("ActivateEnemySpawners", RpcTarget.All);
        }
    }
    [PunRPC]
    void ActivateEnemySpawners()
    {
        enemySpawner1.SetActive(true);
        enemySpawner2.SetActive(true);
    }
    public void SelectWeaponMode(int mode)
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        if (mode == 1)
        {
            
            weaponManager.SetWeaponMode(WeaponMode.Single);
            if (playerIndex == 1)
            {
                Destroy(weaponSelectionUI1);
            } else if (playerIndex == 2)
            {
                Destroy(weaponSelectionUI2);
            }
                
        }
        else if (mode == 2)
        {
            weaponManager.SetWeaponMode(WeaponMode.Dual);
            if (playerIndex == 1)
            {
                Destroy(weaponSelectionUI1);
            }
            else if (playerIndex == 2)
            {
                Destroy(weaponSelectionUI2);
            }
        }
        


    }
    


}
