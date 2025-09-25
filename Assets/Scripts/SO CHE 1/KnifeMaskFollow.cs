using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KnifeMaskFollow : MonoBehaviour
{
    //public FruitCut target;

    //void Update()
    //{
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    mousePos.z = 0;
    //    transform.position = mousePos;

    //    if (Input.GetMouseButton(0))
    //    {
    //        target.CutAt(mousePos);
    //    }
    //}

    public FruitCut target;
    //private Vector2? lastPos = null;

    //void Update()
    //{
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    mousePos.z = 0;
    //    transform.position = mousePos;

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (lastPos != null)
    //        {
    //            target.CutBetween(lastPos.Value, mousePos);
    //        }
    //        lastPos = mousePos;
    //    }
    //    else
    //    {
    //        lastPos = null; // reset khi thả chuột
    //    }
    //}


    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        if (Input.GetMouseButtonDown(0)) // bấm một lần
        {
            target.CutHorizontalAt(mousePos);
        }
    }

}
