using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 10f;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (targetPosition -transform.position).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetPosition);

        if(distance < 0.2f)
        {
            Destroy(gameObject);
        }
    }
}
