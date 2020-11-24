using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public GameObject bullet;
    public float shootingRate;


    private float timeToFire ;
    private Transform m_transform;
    void Start()
    {
        m_transform = transform.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        Shoot();
        Move();
    }

    private void Move()
    {
        if(Input.GetAxisRaw("Horizontal")>0)
            m_transform.position += Vector3.right*moveSpeed * Time.deltaTime;
        else if(Input.GetAxisRaw("Horizontal")<0)
            m_transform.position += Vector3.left*moveSpeed * Time.deltaTime;
        if(Input.GetAxisRaw("Vertical")>0)
            m_transform.position += Vector3.up*moveSpeed * Time.deltaTime;
        else if(Input.GetAxisRaw("Vertical")<0)
            m_transform.position += Vector3.down*moveSpeed * Time.deltaTime;
    }
    private void Shoot()
    {
        
        if(!Input.GetKey(KeyCode.Space) || Time.time < timeToFire)
            return;
        
        Transform bullet = ObjectPool.TakeFormPool("Bullet");
        bullet.position = m_transform.position+m_transform.up*0.5f;
        bullet.rotation = Quaternion.identity;
        //Instantiate(bullet,m_transform.position+m_transform.up*0.5f,Quaternion.identity);
        timeToFire = Time.time + shootingRate;



    }
}
