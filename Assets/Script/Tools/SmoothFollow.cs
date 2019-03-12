using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
    //跟随目标
    public Transform target;
    //摄像机距离
    public float distance = 10.0f;
    //摄像机高度
    public float height = 15.0f;
    //摄像机角度
    public float lookAtAngle = 45.0f;
    //虚拟目标物
    private GameObject targetGameObject;
    public void Start()
    {
        targetGameObject = new GameObject();
        this.transform.LookAt(target.transform);
    }


    /// <summary>
    /// 摄像机基础跟随
    /// </summary>
    public void LateUpdate()
    {
        if (!target)
            return;
        //虚拟目标物与真实目标物 保持坐标和旋转角度一致
        targetGameObject.transform.rotation = target.rotation;
        targetGameObject.transform.position = target.position;
        //最初，摄像机与虚拟目标物保持位置相同
        this.transform.position = targetGameObject.transform.position;
        //设置高度 
        this.transform.position += Vector3.up * height;
        //设置距离
        this.transform.position -= distance * Vector3.forward;
        //设置旋转角度
        Quaternion currentRotation = Quaternion.Euler(0, lookAtAngle, 0);
        this.transform.position -= currentRotation * Vector3.forward;
        if(Time.time== 0f)
        {
            this.transform.LookAt(targetGameObject.transform);
        }
        //LookAt
        if (Input.GetKeyDown(KeyCode.F))
        { 
        this.transform.LookAt(targetGameObject.transform);
    }
    }
    
    public void Update()
    {
       /* if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("a");
            this.transform.Rotate(Vector3.left,Space.World);

        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Rotate(Vector3.right, Space.World);

        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(Vector3.down,Space.World);

        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(Vector3.up,Space.World);

        }*/
    }

}
