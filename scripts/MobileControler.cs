using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileControler : MonoBehaviour
{

    // mobile用操作
    [Header("Mobile Control")]
    /// <summary> 移動操作を受け付けるタッチエリア </summary>
    [SerializeField]
    private DragHandler _moveController;

    /// <summary> 移動速度（m/秒） </summary>
    [SerializeField]
    private float _movePerSecond = 7f;

    /// <summary> 移動操作としてタッチ開始したスクリーン座標 </summary>
    private Vector2 _movePointerPosBegin;

    private Vector3 _moveVector;
    public Vector2 _moveVector2;    // モバイル用に追加

    /// <summary> 自身が所属してるキャンバス </summary>
    private Canvas _belongedCanvas;


    // Look
    /// <summary> カメラ操作を受け付けるタッチエリア </summary>
    [SerializeField]
    private DragHandler _lookController;

    /// <summary> カメラ操作として前フレームにタッチしたキャンバス上の座標 </summary>
    private Vector2 _lookPointerPosPre;
    public Vector2 _lookVector2;    // モバイル用に追加

    /// <summary> 起動時 </summary>
    private void Awake()
    {
        _moveController.OnBeginDragEvent += OnBeginDragMove;
        _moveController.OnDragEvent += OnDragMove;
        _moveController.OnEndDragEvent += OnEndDragMove;

        _lookController.OnBeginDragEvent += OnBeginDragLook;
        _lookController.OnDragEvent += OnDragLook;
        _lookController.OnEndDragEvent += OnEndDragLook;
    }

    /// <summary> 更新処理 </summary>
    private void Update()
    {
        //UpdateMove(_moveVector);
    }

    ////////////////////////////////////////////////////////////
    /// 移動操作
    ////////////////////////////////////////////////////////////

    /// <summary> ドラッグ操作開始（移動用） </summary>
    private void OnBeginDragMove(PointerEventData eventData)
    {
        // タッチ開始位置を保持
        _movePointerPosBegin = eventData.position;
    }

    /// <summary> ドラッグ操作中（移動用） </summary>
    private void OnDragMove(PointerEventData eventData)
    {
        // タッチ開始位置からのスワイプ量を移動ベクトルにする
        var vector = eventData.position - _movePointerPosBegin;
        _moveVector2 = vector.normalized;   // モバイル用に追加
        _moveVector = new Vector3(vector.x, 0f, vector.y);
    }

    private void UpdateMove(Vector3 vector)
    {
        // 現在向きを基準に、入力されたベクトルに向かって移動
        transform.position += transform.rotation * vector.normalized * _movePerSecond * Time.deltaTime;
    }

    /// <summary> ドラッグ操作終了（移動用） </summary>
    private void OnEndDragMove(PointerEventData eventData)
    {
        // 移動ベクトルを解消
        _moveVector = Vector3.zero;
        _moveVector2 = Vector2.zero;    // モバイル用に追加
    }


    ////////////////////////////////////////////////////////////
    /// カメラ操作
    ////////////////////////////////////////////////////////////
    /// <summary> ドラッグ操作開始（カメラ用） </summary>
    private void OnBeginDragLook(PointerEventData eventData)
    {
        _lookPointerPosPre = _lookController.GetPositionOnCanvas(eventData.position);
    }

    /// <summary> ドラッグ操作中（カメラ用） </summary>
    private void OnDragLook(PointerEventData eventData)
    {
        var pointerPosOnCanvas = _lookController.GetPositionOnCanvas(eventData.position);
        // キャンバス上で前フレームから何px操作したかを計算
        var vector = pointerPosOnCanvas - _lookPointerPosPre;
        _lookVector2 = vector;
    }

    private void OnEndDragLook(PointerEventData eventData)
    {
        // 移動ベクトルを解消
        _lookVector2 = Vector2.zero;    // モバイル用に追加
    }
}
