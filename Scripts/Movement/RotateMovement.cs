using UnityEngine;
using Photon.Pun;

namespace BaseDef.Movement
{
    public class RotateMovement : MonoBehaviour
    {
        private Transform targetObject; // D�nme merkezi olacak obje
        public float rotationSpeed = 10f; // D�n�� h�z�
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
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber; // Oyuncunun s�ra numaras�
            GameObject baseObject = null;

            if (playerIndex == 1) // �lk oyuncu
            {
                baseObject = GameObject.Find("Base1");
            }
            else if (playerIndex == 2) // �kinci oyuncu
            {
                baseObject = GameObject.Find("Base2");
            }

            if (baseObject != null)
            {
                targetObject = baseObject.transform; // Base objesinin Transform'u atan�r
                
            }
            else
            {
                Debug.LogError("Target object could not be assigned. Check if Base1 and Base2 exist in the scene.");
            }
        }

        private void Update()
        {
            if (!view.IsMine || targetObject == null) return;

            // Sa� ok tu�u veya 'D' tu�una bas�ld���nda sa�a d�nd�r
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(targetObject.position, Vector3.up, rotationSpeed * Time.deltaTime);
            }

            // Sol ok tu�u veya 'A' tu�una bas�ld���nda sola d�nd�r
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(targetObject.position, Vector3.up, -rotationSpeed * Time.deltaTime);
            }
        }
    }
}
