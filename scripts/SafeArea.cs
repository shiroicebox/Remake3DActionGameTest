using UnityEngine;

namespace Main.Systems.Utilities.Infrastructure
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeArea : MonoBehaviour
    {
        private RectTransform _rectTransform;

        //################################################
        void Awake()
        {
            if (TryGetComponent<RectTransform>(out RectTransform rectTransform))
            {
                _rectTransform = rectTransform;
            }
            ApplySafeArea();    // セーフエリア適用
        }
        //////////////////////////////////////////////////
        /// <summary>
        /// セーフエリア適用
        /// </summary>
        void ApplySafeArea()
        {
            // デバイスがモバイルの時はノッチなどで表示できない恐れがあるのでセーフエリアを考慮する
            if (UnityEngine.Device.SystemInfo.deviceType == DeviceType.Handheld)
            {
                // スマホのSafeAreaを取得
                Rect safeArea = Screen.safeArea;

                // Safe Areaに基づいてアンカーを再設定
                Vector2 anchorMin = safeArea.position;
                Vector2 anchorMax = safeArea.position + safeArea.size;

                // デバイスの解像度に対して正規化（0〜1範囲に変換）
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                // RectTransformのアンカーをSafe Areaに合わせて調整
                _rectTransform.anchorMin = anchorMin;
                _rectTransform.anchorMax = anchorMax;

                // オフセットをゼロにリセット
                _rectTransform.offsetMin = Vector2.zero;
                _rectTransform.offsetMax = Vector2.zero;
            }
            else
            {
                // モバイル以外：DeviceType.Desktopなど
                _rectTransform.anchorMin = Vector2.zero;
                _rectTransform.anchorMax = Vector2.one;
            }
        }
        //////////////////////////////////////////////////
    }
}
