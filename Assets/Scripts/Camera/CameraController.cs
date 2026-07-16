using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        GameFacade.Instance.EventCenter.OnCameraFollow += OnCameraFollow;
    }

    private void OnCameraFollow(Transform player)
    {
        vcam.Follow = player;
    }

    private void OnDestroy()
    {
        if (GameFacade.Instance is not null)
        {
            GameFacade.Instance.EventCenter.OnCameraFollow -= OnCameraFollow;
        }
    }
}
