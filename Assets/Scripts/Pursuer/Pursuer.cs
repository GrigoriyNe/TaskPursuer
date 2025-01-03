using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pursuer : MonoBehaviour
{
    [SerializeField] private Player _target;

    private Rigidbody _rigidbody;
    private float _maxDistance = 3f;
    private float _speed = 2f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void Movement()
    {
        float distance = Vector3.Distance(_target.transform.position, transform.position);

        if (distance > _maxDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        }

        transform.LookAt(_target.transform);
    }
}
