using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    private Transform target;
    [SerializeField] float detectionRange = 30f;
    [SerializeField] float rotationSpeed = 5f;

    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindBase();
        agent.updateRotation = false;

    }

    void Update()
    {
        if (target != null)
        {
            RotateTowardsTarget();
            agent.destination = target.position;
        }
    }
    void RotateTowardsTarget()
    {
        // Hedefe doðru rotasyonu ayarla
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Sadece Y ekseni üzerinde döndürme iþlemi
        Vector3 rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }


    void FindBase()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        int i = 0;
        bool baseFound = false;

        while (i < hitColliders.Length && target == null && baseFound == false)
        {
            if (hitColliders[i].CompareTag("Base"))
            {
                target = hitColliders[i].transform;
                baseFound = true;
            }
            i++;
        }

    }
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
        agent.SetDestination(target.position);  // Yeni hedefe yönlendirilir
    }
}