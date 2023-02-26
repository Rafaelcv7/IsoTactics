using System.Collections;
using IsoTactics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int cameraSpeed;
    private BaseCharacter _activeCharacter;
    private Camera _camera;
    private bool _isCameraFree;
    
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        var step = cameraSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.RightShift)) {
            _isCameraFree = !_isCameraFree;
        }
        
        var up = Input.GetKey(KeyCode.UpArrow);
        var left = Input.GetKey(KeyCode.LeftArrow);
        var right = Input.GetKey(KeyCode.RightArrow);
        var down = Input.GetKey(KeyCode.DownArrow);
        
        if ((up || left || right || down) && _isCameraFree)
        {
            var cameraNewPos = _camera.transform.position;
            if (right) cameraNewPos.x += 10f;
            if (left) cameraNewPos.x -= 10f;
            if (up) cameraNewPos.y += 10f;
            if (down) cameraNewPos.y -= 10f;
            //Clamp to limit camera travel.
            cameraNewPos.x = Mathf.Clamp(cameraNewPos.x, -8f, 8f);
            cameraNewPos.y = Mathf.Clamp(cameraNewPos.y, -8f, 8f);
            _camera.transform.position = Vector2.MoveTowards(_camera.transform.position, cameraNewPos, step);
        }
        else
        {
            if (_activeCharacter && !_isCameraFree)
            {
                _camera.transform.position = Vector2.MoveTowards(_camera.transform.position, _activeCharacter.transform.position, step);   
            } 
        }
    }

    public void SetActiveCharacter(Component sender, object data)
    {
        if (data is BaseCharacter newCharacter)
        {
            _activeCharacter = newCharacter;
        }
    }
}
