using UnityEngine;
using Photon.Pun;

namespace BaseDef.Movement
{
    public class RotateMovement : MonoBehaviour
    {
        private Transform targetObject; // Dönme merkezi olacak obje
        public float rotationSpeed = 10f; // Dönüþ hýzý
        private PhotonView view;

        private void Start()
        {
            view = GetComponent<PhotonView>();

            if (view.IsMine)
            {
                AssignTargetObject();
            }
        }

        private void AssignTargetObject()
        {
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber; // Oyuncunun sýra numarasý
            GameObject baseObject = null;

            if (playerIndex == 1) // Ýlk oyuncu
            {
                baseObject = GameObject.Find("Base1");
            }
            else if (playerIndex == 2) // Ýkinci oyuncu
            {
                baseObject = GameObject.Find("Base2");
            }

            if (baseObject != null)
            {
                targetObject = baseObject.transform; // Base objesinin Transform'u atanýr
                
            }
            else
            {
                Debug.LogError("Target object could not be assigned. Check if Base1 and Base2 exist in the scene.");
            }
        }

        private void Update()
        {
            if (!view.IsMine || targetObject == null) return;

            // Sað ok tuþu veya 'D' tuþuna basýldýðýnda saða döndür
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(targetObject.position, Vector3.up, rotationSpeed * Time.deltaTime);
            }

            // Sol ok tuþu veya 'A' tuþuna basýldýðýnda sola döndür
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(targetObject.position, Vector3.up, -rotationSpeed * Time.deltaTime);
            }
        }
    }
}
