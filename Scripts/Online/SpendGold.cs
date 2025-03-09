using UnityEngine;
using BaseDef.Healts;
using Photon.Pun;

namespace BaseDef.Enemies
{
    public class SpendGold : MonoBehaviour
    {
        [SerializeField] private GameObject enemyType1Prefab; // 1. tip düþman
        [SerializeField] private GameObject enemyType2Prefab; // 2. tip düþman
        [SerializeField] private GameObject enemyType3Prefab; // 3. tip düþman

        private Transform spawnPoint; // Düþmanlarýn spawn olacaðý nokta

        private PhotonView view;
        private Base baseScript; 

        private void Start()
        {
            view = GetComponent<PhotonView>();
            if (!view.IsMine) return;
            AssignBaseToPlayer();
            AssignSpawnLocation();
        }
        private void AssignBaseToPlayer()
        {
            // Oyuncu sýrasýna göre doðru Base objesini bul
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber; // Oyuncu sýra numarasý
            Base assignedBase = null;

            if (playerIndex == 1) // Ýlk oyuncu
            {
                assignedBase = GameObject.Find("Base1").GetComponent<Base>();
            }
            else if (playerIndex == 2) // Ýkinci oyuncu
            {
                assignedBase = GameObject.Find("Base2").GetComponent<Base>();
            }

            if (assignedBase != null)
            {
                baseScript = assignedBase;
                
            }
            else
            {
                Debug.LogError("Base could not be assigned. Check if Base1 and Base2 exist in the scene.");
            }
        }
        private void AssignSpawnLocation()
        {
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber; // Oyuncunun sýra numarasý
            GameObject spawnObject = null;

            if (playerIndex == 1) // Ýlk oyuncu
            {
                spawnObject = GameObject.Find("EnemySendLoc2");
            }
            else if (playerIndex == 2) // Ýkinci oyuncu
            {
                spawnObject = GameObject.Find("EnemySendLoc1");
            }

            if (spawnObject != null)
            {
                spawnPoint = spawnObject.transform; // Base objesinin Transform'u atanýr
            }
            else
            {
                Debug.LogError("Target object could not be assigned. Check if spawn locs exist in the scene.");
            }
        }

        private void Update()
        {
            // Sadece kendi oyuncumuz kontrol edebilir
            if (!view.IsMine) return;

            // Tuþ giriþlerine göre düþman spawn etme
            if (Input.GetKeyDown(KeyCode.Alpha1)) // "1" tuþu
            {
                SpawnEnemy(5, enemyType1Prefab);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // "2" tuþu
            {
                SpawnEnemy(8, enemyType2Prefab);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) // "3" tuþu
            {
                SpawnEnemy(10, enemyType3Prefab);
            }
        }

        private void SpawnEnemy(float cost, GameObject enemyPrefab)
        {
            if (baseScript.GetGold() < cost)
            {
                Debug.Log("Yeterli altýn yok.");
                return;
            }

            if (enemyPrefab != null)
            {
                string prefabName = enemyPrefab.name;

                // Prefab'in Resources klasöründe olduðundan emin olun
                GameObject enemy = PhotonNetwork.Instantiate(prefabName, spawnPoint.position, spawnPoint.rotation);

                // Altýn azaltma
                baseScript.DecreaseGold(cost);

                Debug.Log($"Spawned {prefabName}. Remaining Gold: {baseScript.gold}");
            }
            else
            {
                Debug.LogError("Geçersiz düþman prefab.");
            }
        }
    }
}
