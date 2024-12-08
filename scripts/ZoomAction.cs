using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using TMPro;

public class ZoomAction : MonoBehaviour
{
    private InputActionValue _input;
    private AudioSource audioSource;

    [SerializeField] public Camera _mainCamera;
    [Header("Cinemachine")]
    [Tooltip("ThirdPersonCamera")]
    [SerializeField] public CinemachineVirtualCamera _virtualCameraThirdPerson;
    [Tooltip("FirstPersonCamera")]
    [SerializeField] public CinemachineVirtualCamera _virtualCameraFirstPerson;

    private bool _isZoomMode;   // �J�������[�h�̔���
    private bool _isTargerLook; // �Ώە����B�eOk�̎�
    public string targetName = "Monster_";
    public float distance = 5.0f;   // ���o�\�ȋ���

    [Space(10)]
    [Tooltip("Shutter Frame Panel")]
    [SerializeField] public FlashControler _flashControler;

    [Space(10)]
    [Tooltip("Shutter Sound")]
    public AudioClip _shutterSound;
    [Range(0, 1)] public float _shutterSoundVolume = 0.5f;

    public GameObject TaskPanel;
    public GameObject ResultPanel;
    public GameObject OperationPanel;
    public GameObject JoystickLook;

    public TextMeshProUGUI _textResult;
    public TextMeshProUGUI _textTask;
    public TextMeshProUGUI _textTimelimit;
    public TextMeshProUGUI _textRestFilms;
    public TextMeshProUGUI _textShutterComment;

    public float Timelimit = 180.0f; //��������
    public int Filmlimit = 10; //�ʐ^��������
    private int _useFilm = 0;
    private int _restFilm;

    // �^�[�Q�b�g�̓^�O�����āA�����Ǘ�������������
    private int _isTarget1;
    private int _isTarget2;
    private int _isTarget3;
    private int _targetCount;
    private int _targetMax;

    private bool _isGameClear;
    private bool _isTimeUp;
    private float _CommentsTime;
    private float _forceTime;
    private bool _flagTime;
    private string _targetName;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _input = GetComponent<InputActionValue>();

        _forceTime = 0.3f;
        _CommentsTime = 2.0f;
        _isTarget1 = 1;
        _isTarget2 = 1;
        _isTarget3 = 1;

        var _targets = GameObject.FindGameObjectsWithTag("TargetFilm");
        if (_targets != null ) _targetMax = _targets.Length;

        ResultPanel.SetActive(false);
        OperationPanel.SetActive(true);
        TaskPanel.SetActive(true);
        JoystickLook.SetActive(true);

        _isGameClear = false;
        _isTimeUp = false;
        //Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        // �C���^�[�o������
        if (_forceTime <= 0)
        {
            _flagTime = false;
        }

        // �������Ԃ̃^�C�}�[&�������Ԕ���
        if (Timelimit > 0 && _isGameClear == false)
        {
            Timelimit -= Time.deltaTime;
        }
        else if (Timelimit <= 0) 
        {
            Timelimit = 0;
            _isTimeUp = true;
        }

        // �Q�[���N���A�̔���
        _targetCount = _isTarget1 + _isTarget2 + _isTarget3;
        if (_targetCount == 0)
        {
            _isGameClear = true;
        }

        _forceTime -= Time.deltaTime;
        

        // �e�L�X�g�̐���
        _textTask.text = $"���~�~�b�N���c��{_targetCount}�̎B�e����";
        _textTimelimit.text = $"�������ԁF{Timelimit.ToString("#.#")}�b";
        if (Timelimit < 20.0f)
        {
            _textTimelimit.color = Color.red;
        }

        _restFilm = Filmlimit - _useFilm;
        if (_restFilm > _targetMax)
        {
            _textRestFilms.text = $"{_restFilm}��";
            _textRestFilms.color = Color.green;
        }
        else if (_restFilm <= _targetMax && _restFilm > 0)
        {
            _textRestFilms.text = $"{_restFilm}��";
            _textRestFilms.color = Color.yellow;
        }
        else if (_restFilm == 0)
        {
            _textRestFilms.text = "EMPTY";
            _textRestFilms.color = Color.red;
        }

        _targetName = CheckTargetInCamera();
        if (_CommentsTime > 0 && _CommentsTime < 2.0f)
        {
            _CommentsTime -= Time.deltaTime;
        }
        else if(_CommentsTime <= 0) 
        {
            _CommentsTime = 2.0f;
        }

        if (!_isZoomMode)
        {
            _textShutterComment.text = "";
        }
        else if (_isZoomMode && !_isTargerLook && _CommentsTime == 2.0f)
        {
            _textShutterComment.text = "�Ώۂ͔͈͊O�ł�";
            _textShutterComment.color = Color.red;
        }
        else if (_isZoomMode && _isTargerLook && _CommentsTime == 2.0f)
        {
            _textShutterComment.text = "�B�e�\�ł�";
            _textShutterComment.color = Color.green;
        }

        // Zoom�؂�ւ�
        if (_input.zoom && !_flagTime)
        {
            Zoom();
            _input.zoom = false;
        }
        
        // �V���b�^�[
        if (_input.shutter && !_flagTime && _restFilm > 0)
        {
            Shutter();
            if (_isTargerLook && _targetName != null)
            {
                CheckTargetIsHit();
            }
            _input.shutter = false;
        }

        // �Q�[���N���A�� or �^�C���A�b�v��
        if (_isGameClear || _isTimeUp || _restFilm == 0)
        {
            ResultPanel.SetActive(true);
            OperationPanel.SetActive(false);
            TaskPanel.SetActive(false);
            JoystickLook.SetActive(false);
            //Cursor.lockState = CursorLockMode.None;
            _input.move = Vector2.zero;
            _input.look = Vector2.zero;
            if (_isGameClear)
            {
                _textResult.text = "Game Clear";
                _textResult.color = Color.blue;
            }
            else if (_isTimeUp || _restFilm == 0) 
            {
                _textResult.text = "Game Over";
                _textResult.color = Color.red;
            }
        }

    }

    /// <summary>
    /// �����Ă�������փf�o�b�O�p���[�U�[�𔭎˂��B�e�ΏۂɐG�ꂽ�ꍇ�A����Ƀ`�F�b�N����
    /// </summary>
    string CheckTargetInCamera()
    {
        var rayStartPosition = _mainCamera.transform.position;
        var rayDirection = _mainCamera.transform.forward.normalized;
        RaycastHit raycastHit;
        var isHit = Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance);
        // Debug.DrawRay (Vector3 start(ray���J�n����ʒu), Vector3 dir(ray�̕����ƒ���), Color color(���C���̐F));
        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

        if (isHit && _isZoomMode)
        {
            if (raycastHit.collider.gameObject.name.Contains($"{targetName}"))
            {
                _isTargerLook = true;
                return raycastHit.collider.gameObject.name;
            }
            else
            {
                _isTargerLook = false;
                return null;
            }
        }
        else
        {
            _isTargerLook = false;
            return null;
        }
    }

    private void CheckTargetIsHit()
    {
        switch (_targetName)
        {
            case "Monster_Mimic_1":
                _isTarget1 = 0;
                ShutterInTargetEvent();
                break;
            case "Monster_Mimic_2":
                _isTarget2 = 0;
                ShutterInTargetEvent();
                break;
            case "Monster_Mimic_3":
                _isTarget3 = 0;
                ShutterInTargetEvent();
                break;
            default: break;
        }
    }

    private void ShutterInTargetEvent()
    {
        _CommentsTime -= 0.1f;
        _textShutterComment.text = "Good!";
        _textShutterComment.color = Color.green;
    }

    void Zoom()
    {
        if (_isZoomMode == false)
        {
            _virtualCameraFirstPerson.Priority = 11;
            _isZoomMode = true;
        }
        else
        {
            _virtualCameraFirstPerson.Priority = 9;
            _isZoomMode = false;
        }
        _forceTime = 0.3f;
        _flagTime = true;
    }

    void Shutter()
    {
        if (_isZoomMode)
        {
            audioSource.PlayOneShot(_shutterSound);
            _flashControler.ShutterEffect();
            _useFilm++;
        }
        _forceTime = 0.2f;
        _flagTime = true;
    }
}
