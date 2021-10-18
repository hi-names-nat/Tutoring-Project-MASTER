using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    [SerializeField] PlayerProto player;
    [SerializeField] float screenPercent;

    int screenEdgeW = 200;
    int screenEdgeH = 200;
    int ew;
    int eh;

    Camera _camera;
    Vector3 _followPosition = Vector3.zero;
    bool _isCameraMoving = false;

    private void Awake()
    {
        TryGetComponent(out _camera);
        _followPosition = transform.position;
        screenEdgeW = Mathf.FloorToInt((float)Screen.width * (screenPercent / 2) / 100);
        screenEdgeH = Mathf.FloorToInt((float)Screen.height *  (screenPercent / 2) / 100);
        eh = screenEdgeH;
        ew = screenEdgeW;
    }

    // Update is called once per frame
    void Update()
    {
        // worldtoscreenpoint is a ratio of where the player is on screen so its possible to use that ratio to move the camera rather than to do it like this.
        float moveAmount = 5f;
        if (_camera.WorldToScreenPoint(player.gameObject.transform.position).x > Screen.width - screenEdgeW)
        {
            _followPosition.x += moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }
        if (_camera.WorldToScreenPoint(player.gameObject.transform.position).x < screenEdgeW)
        {
            _followPosition.x -= moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }
        if (_camera.WorldToScreenPoint(player.gameObject.transform.position).y > Screen.height - screenEdgeH)
        {
            _followPosition.z += moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }
        if (_camera.WorldToScreenPoint(player.gameObject.transform.position).y < screenEdgeH)
        {
            _followPosition.z -= moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }
        transform.position = _followPosition;
    }
}
