using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileControler : MonoBehaviour
{

    // mobile�p����
    [Header("Mobile Control")]
    /// <summary> �ړ�������󂯕t����^�b�`�G���A </summary>
    [SerializeField]
    private DragHandler _moveController;

    /// <summary> �ړ����x�im/�b�j </summary>
    [SerializeField]
    private float _movePerSecond = 7f;

    /// <summary> �ړ�����Ƃ��ă^�b�`�J�n�����X�N���[�����W </summary>
    private Vector2 _movePointerPosBegin;

    private Vector3 _moveVector;
    public Vector2 _moveVector2;    // ���o�C���p�ɒǉ�

    /// <summary> ���g���������Ă�L�����o�X </summary>
    private Canvas _belongedCanvas;


    // Look
    /// <summary> �J����������󂯕t����^�b�`�G���A </summary>
    [SerializeField]
    private DragHandler _lookController;

    /// <summary> �J��������Ƃ��đO�t���[���Ƀ^�b�`�����L�����o�X��̍��W </summary>
    private Vector2 _lookPointerPosPre;
    public Vector2 _lookVector2;    // ���o�C���p�ɒǉ�

    /// <summary> �N���� </summary>
    private void Awake()
    {
        _moveController.OnBeginDragEvent += OnBeginDragMove;
        _moveController.OnDragEvent += OnDragMove;
        _moveController.OnEndDragEvent += OnEndDragMove;

        _lookController.OnBeginDragEvent += OnBeginDragLook;
        _lookController.OnDragEvent += OnDragLook;
        _lookController.OnEndDragEvent += OnEndDragLook;
    }

    /// <summary> �X�V���� </summary>
    private void Update()
    {
        //UpdateMove(_moveVector);
    }

    ////////////////////////////////////////////////////////////
    /// �ړ�����
    ////////////////////////////////////////////////////////////

    /// <summary> �h���b�O����J�n�i�ړ��p�j </summary>
    private void OnBeginDragMove(PointerEventData eventData)
    {
        // �^�b�`�J�n�ʒu��ێ�
        _movePointerPosBegin = eventData.position;
    }

    /// <summary> �h���b�O���쒆�i�ړ��p�j </summary>
    private void OnDragMove(PointerEventData eventData)
    {
        // �^�b�`�J�n�ʒu����̃X���C�v�ʂ��ړ��x�N�g���ɂ���
        var vector = eventData.position - _movePointerPosBegin;
        _moveVector2 = vector.normalized;   // ���o�C���p�ɒǉ�
        _moveVector = new Vector3(vector.x, 0f, vector.y);
    }

    private void UpdateMove(Vector3 vector)
    {
        // ���݌�������ɁA���͂��ꂽ�x�N�g���Ɍ������Ĉړ�
        transform.position += transform.rotation * vector.normalized * _movePerSecond * Time.deltaTime;
    }

    /// <summary> �h���b�O����I���i�ړ��p�j </summary>
    private void OnEndDragMove(PointerEventData eventData)
    {
        // �ړ��x�N�g��������
        _moveVector = Vector3.zero;
        _moveVector2 = Vector2.zero;    // ���o�C���p�ɒǉ�
    }


    ////////////////////////////////////////////////////////////
    /// �J��������
    ////////////////////////////////////////////////////////////
    /// <summary> �h���b�O����J�n�i�J�����p�j </summary>
    private void OnBeginDragLook(PointerEventData eventData)
    {
        _lookPointerPosPre = _lookController.GetPositionOnCanvas(eventData.position);
    }

    /// <summary> �h���b�O���쒆�i�J�����p�j </summary>
    private void OnDragLook(PointerEventData eventData)
    {
        var pointerPosOnCanvas = _lookController.GetPositionOnCanvas(eventData.position);
        // �L�����o�X��őO�t���[�����牽px���삵�������v�Z
        var vector = pointerPosOnCanvas - _lookPointerPosPre;
        _lookVector2 = vector;
    }

    private void OnEndDragLook(PointerEventData eventData)
    {
        // �ړ��x�N�g��������
        _lookVector2 = Vector2.zero;    // ���o�C���p�ɒǉ�
    }
}
