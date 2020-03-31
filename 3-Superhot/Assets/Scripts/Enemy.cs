using System.Collections;
using Attributes;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: CustomBehaviour {
    [Require] private NavMeshAgent _agent;
    [Require] private Animator _animator;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _follows;
    [SerializeField] private Gun _gun;
    [SerializeField] private Transform _gunOrigin;
    [SerializeField] private float _shootTimer = 3f;

    [Header("Bones")]
    [SerializeField] private Transform _chest;
    [SerializeField] private Transform _leftShoulder;
    [SerializeField] private Transform _rightShoulder;
    
    [Header("Offsets - Following")]
    [SerializeField] private Vector3 _followingChestOffset;
    [SerializeField] private Vector3 _followingLeftShoulderOffset;
    [SerializeField] private Vector3 _followingRightShoulderOffset;
    
    [Header("Offsets - Not Following")]
    [SerializeField] private Vector3 _notFollowingChestOffset;
    [SerializeField] private Vector3 _notFollowingLeftShoulderOffset;
    [SerializeField] private Vector3 _notFollowingRightShoulderOffset;
    
    private static readonly int Running = Animator.StringToHash("Running");
    private bool _following;

    private Coroutine _shootCoroutine;
    private WaitForSeconds _timer;

    private void Start() {
        _timer = new WaitForSeconds(_shootTimer);
        _shootCoroutine = StartCoroutine(Shoot());
    }

    private void LateUpdate() {
        _animator.SetBool(Running, _following);
        
        _chest.LookAt(_target);
        _chest.rotation *= Quaternion.Euler(_following ? _followingChestOffset : _notFollowingChestOffset);
        _leftShoulder.rotation *= Quaternion.Euler(_following ? _followingLeftShoulderOffset : _notFollowingLeftShoulderOffset);
        _rightShoulder.rotation *= Quaternion.Euler(_following ? _followingRightShoulderOffset : _notFollowingRightShoulderOffset);

        if (_follows && Vector3.Distance(_target.position, transform.position) > _agent.stoppingDistance) {
            _following = true;
            _agent.SetDestination(_target.position);

            if (_agent.remainingDistance < _agent.stoppingDistance) {
                _following = false;
            }
        }
    }

    private IEnumerator Shoot() {
        while (true) {
            yield return _timer;
            _gun.ShootPosition(_gunOrigin.position);
        }
    }
}