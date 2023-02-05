using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private Image _statusIcon;
    [SerializeField] private Image _durationIcon;
    [SerializeField] private TextMeshProUGUI _duarionText;

    private Status _representingStatus;

    public void BindStatus(Status s) {
        if (_representingStatus != null) return;
        
        gameObject.SetActive(true);
        _representingStatus = s;
        _statusIcon.sprite = s.GetIcon();
        _durationIcon.fillAmount =  s.GetFullDuration() > 0 ? s.GetRemainingTime() / s.Duration : 0;
        s.Destroied += OnExpired;
    }

    private void Start() {
       if( _representingStatus==null)
            gameObject.SetActive(false);
    }



    private void OnExpired() {
        Destroy(gameObject);
    }

    private void OnDurationUpdated() => _durationIcon.fillAmount = _representingStatus.GetFullDuration() > 0 ? _representingStatus.GetRemainingTime() / _representingStatus.Duration : 0;


    private void LateUpdate() {
        OnDurationUpdated();
    }

    private void OnDestroy() {

    }
}
