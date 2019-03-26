using Microsoft.MixedReality.Toolkit.Core.EventDatum.Input;
using Microsoft.MixedReality.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MRTKEditorControl : MonoBehaviour {

    public float speed = 1f;
    public float rotSpeed = 1f;

    public bool UprightViewpoint = true;

    private bool Upright = true;
    private bool move = true;

    public static InputEventData IED;
    // Use this for initialization
    void Awake () {
        IED = new InputEventData(EventSystem.current);
    }

#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            move = false;
        }
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            move = true;
        }

        //Simulate Mouse Click (Work In Progess)
        if (Input.GetMouseButton(0))
        {            
            RaycastHit[] hit = Physics.RaycastAll(new Ray(CameraCache.Main.transform.position, CameraCache.Main.transform.forward), ~(1<<2));
            if (hit.Length>0)
            {
                foreach (RaycastHit h in hit)
                {
                    h.transform.SendMessage("OnInputDown", IED, SendMessageOptions.RequireReceiver);
                }
                
            }
        }

        //Look Around With Mouse
        if (move)
        {
            transform.RotateAround(CameraCache.Main.transform.position, transform.TransformVector(Vector3.up), Time.deltaTime * 100 * rotSpeed * Input.GetAxis("Mouse X"));
            transform.RotateAround(CameraCache.Main.transform.position, transform.TransformVector(Vector3.left), Time.deltaTime * 100 * rotSpeed * Input.GetAxis("Mouse Y"));
        }

        //Roll Axis/Rise & Fall
        if (Input.GetKey(KeyCode.Q))
        {
            if (!UprightViewpoint)
            {
                transform.RotateAround(Camera.main.transform.position, transform.TransformVector(Vector3.forward), Time.deltaTime * 100 * rotSpeed * 1);
                Upright = false;
            }else{
                transform.position += Time.deltaTime * 2 * speed * CameraCache.Main.transform.TransformDirection(new Vector3(0,-1,0));
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (!UprightViewpoint)
            {
                transform.RotateAround(Camera.main.transform.position, transform.TransformVector(Vector3.forward), Time.deltaTime * 100 * rotSpeed * -1);
            Upright = false;
            }else{
                transform.position += Time.deltaTime * 2 * speed * CameraCache.Main.transform.TransformDirection(new Vector3(0, 1, 0));
            }
        }
        if(Upright)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
        }

        //WASD Movement
        transform.position += Time.deltaTime * 5 * speed * CameraCache.Main.transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }
#endif
}
