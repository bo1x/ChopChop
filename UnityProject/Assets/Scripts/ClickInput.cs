using UnityEngine;
using UnityEngine.InputSystem;

public class ClickInput : MonoBehaviour
{
    public static ClickInput instance;
    [SerializeField]  private bool clickActive;
    InputSystem_Actions controls;
    [SerializeField] Camera cam;
    public static ClickInput Instance
    {
        get { return instance; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
          if(instance != null && instance == this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        controls = new InputSystem_Actions();
        controls.Player.Enable();
        controls.Player.Click.performed += ClickPerformed;
        controls.Player.Click.canceled += ClickCanceled;


    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    public bool GetAttack()
    {
        return clickActive;
    }
    private void ClickPerformed(InputAction.CallbackContext context)
    {
        clickActive = true;
        Debug.Log("CLICK");
    }
    private void ClickCanceled(InputAction.CallbackContext context)
    {
        clickActive = false;
        Debug.Log("CLICKrelease");
    }

    public Vector2 GetMousePos()
    {
        Vector2 pos = Mouse.current.position.ReadValue();
        return cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10));
    }
}
