using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    [SerializeField] PlayerProto player;
    [SerializeField]
    [Range(0, 100)]
    public float screenPercentX, screenPercentY;
    Vector3 desiredPositionOverride;

    int screenEdgeW = 200;
    int screenEdgeH = 200;

    Camera _camera;
    Vector3 _followPosition = Vector3.zero;
    Vector3 _playerPosition = Vector3.zero;
    bool _isCameraMoving = false;


    private void Awake()
    {
        TryGetComponent(out _camera);
        _followPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float moveAmount = 5f;
        // worldtoscreenpoint is a ratio of where the player is on screen so its possible to use that ratio to move the camera rather than to do it like this.
        _playerPosition = _camera.WorldToScreenPoint(player.gameObject.transform.position);

        screenEdgeW = Mathf.FloorToInt((float)Screen.width * (screenPercentX / 2) / 100);
        screenEdgeH = Mathf.FloorToInt((float)Screen.height * (screenPercentY / 2) / 100);

        if (_playerPosition.x > Screen.width - screenEdgeW)
        {
            _followPosition.x += moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }
        if (_playerPosition.x < screenEdgeW)
        {
            _followPosition.x -= moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }
        if (_playerPosition.y > Screen.height - screenEdgeH)
        {
            _followPosition.z += moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }
        if (_playerPosition.y < screenEdgeH)
        {
            _followPosition.z -= moveAmount * Time.deltaTime;
            _isCameraMoving = true;
        }

        transform.position = _followPosition;
    }
}
