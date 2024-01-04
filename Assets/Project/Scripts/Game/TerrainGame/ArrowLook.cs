using System.Reflection;
using UnityEngine;

public class ArrowLook : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public Transform LookAtTarget { get { return _target; } }

    [SerializeField] private Transform _spinner;
    public Transform Spinner { get { return _spinner; } }

    [SerializeField] private Transform _scaler;
    public Transform Scaler { get { return _scaler; } }

    public float speed;

    private void SetTarget(Transform target = null)
    {
        _target = target;
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (LookAtTarget) transform.LookAt(LookAtTarget);
        if (Spinner) Spinner.transform.Rotate(0, 0, speed * Time.deltaTime);
    }



    private void OnEnable()
    {
        KrishnaInteractionManager.FindTheMantra += OnGetMission;
        ScrollInteractionManager.HideDirectionArrow += OnEndMission;
    }

    private void OnDestroy()
    {
        KrishnaInteractionManager.FindTheMantra -= OnGetMission;
        ScrollInteractionManager.HideDirectionArrow -= OnEndMission;
    }

    private void OnEndMission()
    {
        SetTarget(null);
        this.gameObject.SetActive(false);
    }

    private void OnGetMission(Transform target)
    {
        this.gameObject.SetActive(true);
        SetTarget(target);
    }
}
