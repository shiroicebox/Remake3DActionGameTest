using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetMoveConroler : MonoBehaviour
{
    private Animator _animator;

    public GameObject Player;
    public GameObject Target;

    private Transform _playerTransform;
    private Transform _targetTransform;

    private Quaternion _firstPos;
    public Vector3 _forward = Vector3.forward;

    public float _nearDis = 4.5f;
    public float _farDis = 20.0f;
    public float _maxDistance = 20.0f;
    public float _spinSpeed = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _playerTransform = Player.transform;
        _targetTransform = Target.transform;
        _firstPos = _targetTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float _dis = Vector3.Distance(_playerTransform.position, _targetTransform.position);
        Vector3 _playerDirection = new Vector3(_playerTransform.transform.position.x,
                                                _targetTransform.transform.position.y,
                                                _playerTransform.transform.position.z);
        var dir = _playerDirection - _targetTransform.position;
        var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
        var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
        var offsetLookAtRotation = lookAtRotation * offsetRotation;

        if (_dis < _maxDistance)
        {
            _targetTransform.rotation = Quaternion.Slerp(_targetTransform.rotation, offsetLookAtRotation, _spinSpeed);
        }
        else 
        {
            _targetTransform.rotation = Quaternion.Slerp(_targetTransform.rotation, _firstPos, _spinSpeed);
        }

        if (_dis < _nearDis)
        {
            _animator.SetBool("NearPos", true);
        }
        else if (_dis > _nearDis && _dis < _farDis)
        {
            _animator.SetBool("FarPos", true);
            _animator.SetBool("NearPos", false);
        }
        else 
        {
            _animator.SetBool("FarPos", false);
        }



    }
}
