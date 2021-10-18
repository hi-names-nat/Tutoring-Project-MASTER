using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoCamera : MonoBehaviour
{
    float _originalTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        _originalTimeScale = Time.timeScale;
        //Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Time.timeScale = _originalTimeScale;
    }

    public void OnMove(InputValue val)
    {
        val.Get<Vector2>()
    }
}
