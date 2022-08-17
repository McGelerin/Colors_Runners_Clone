using System.Collections.Generic;
using Data.ValueObject;
using Keys;
using Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        public void MouseButtonUp(bool isTouching)
        {
            isTouching = false;
            InputSignals.Instance.onInputReleased?.Invoke();
        }

        public void MouseButtonDown(bool isTouching,bool isFirstTimeTouchTaken,Vector2? mousePosition)
        {
            isTouching = true;
            InputSignals.Instance.onInputTaken?.Invoke();
            if (!isFirstTimeTouchTaken)
            {
                isFirstTimeTouchTaken = true;
                InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
            }
                
            mousePosition = Input.mousePosition;
        }

        public void HoldMouseButton(bool isTouching,Vector2? mousePosition,InputData data,Vector3 moveVector,float currentVelocity)
        {
            if (isTouching)
            {
                if (mousePosition != null) 
                { 
                    Vector2 mouseDeltaPos = (Vector2) Input.mousePosition - mousePosition.Value;
                            
                    if (mouseDeltaPos.x > data.HorAndVerInputSpeed)
                        moveVector.x = data.HorAndVerInputSpeed / 10f * mouseDeltaPos.x;
                    else if (mouseDeltaPos.x < -data.HorAndVerInputSpeed)
                        moveVector.x = -data.HorAndVerInputSpeed / 10f * -mouseDeltaPos.x;
                    else
                        moveVector.x = Mathf.SmoothDamp(moveVector.x, 0f, ref currentVelocity,
                            data.ClampSpeed);
                             
                    mousePosition = Input.mousePosition;
                             
                    InputSignals.Instance.onRunnerInputDragged?.Invoke(new RunnerInputParams()
                    {
                        XValue = moveVector.x,
                        ClampValues = new Vector2(data.ClampSides.x, data.ClampSides.y)
                    });
                }
            }
        }

        public void SetFloatingInput(Vector3 moveVector,FloatingJoystick floatingJoystick)
        {
            moveVector.x = floatingJoystick.Horizontal;
            moveVector.z = floatingJoystick.Vertical;
                        
            InputSignals.Instance.onJoystickDragged?.Invoke(new IdleInputParams()
            {
                ValueX = moveVector.x,
                ValueZ = moveVector.z
            });
        }
        public bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}