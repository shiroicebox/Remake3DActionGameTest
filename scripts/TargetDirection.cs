using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDirection : MonoBehaviour
{
    public GameObject Player;

    private Transform _playerTransform;
    private Transform _targetTransform;

    public Vector3 _forward = Vector3.forward;

    public float _spinSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _playerTransform = Player.transform;
        transform.position = _playerTransform.position + new Vector3(0,2,0);

        _targetTransform = GetNearTargetTag("TargetFilm");
        if ( _targetTransform == null ) return;
        
        var dir = _targetTransform.position - _playerTransform.position;
        var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
        var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);
        var offsetLookAtRotation = lookAtRotation * offsetRotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, offsetLookAtRotation, _spinSpeed);
    }

    private Transform GetNearTargetTag(string tagName)
    {
        // 該当タグが1つしか無い場合はそれを返す
        var targets = GameObject.FindGameObjectsWithTag(tagName);
        if (targets.Length == 0) return null;
        if (targets.Length == 1) return targets[0].transform;

        GameObject result = null;
        var minTargetDistance = float.MaxValue;
        foreach (var target in targets)
        {
            // 前回計測したオブジェクトよりも近くにあれば記録
            var targetDistance = Vector3.Distance(transform.position, target.transform.position);
            if (!(targetDistance < minTargetDistance)) continue;
            minTargetDistance = targetDistance;
            result = target.transform.gameObject;
        }

        // 最後に記録されたオブジェクトを返す
        return result?.transform;
    }
}
