using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class NormalEnemy : Enemy
{
    public float moveLength = 3;
    public float fireRate;
    public int bulletQuantity;
    private Vector3 moveDir;
    private int currentBulletQuantity;
    private Quaternion shootQuaternion;
    private void Update() 
    {
        Debug.DrawLine(m_Transform.position,m_Transform.position+moveDir*moveLength);
        Move();
    }
    private void Move()
    {
        if(acting)
            return;
        acting = true;
        moveDir =(Player.Instance.m_Transform.position-m_Transform.position).normalized;
        moveDir.z = 0;
        int i = Random.Range(0,5);
        if(i == 0)
            moveDir = Quaternion.Euler(0,0,90)*moveDir;
        else if(i == 1)
            moveDir = Quaternion.Euler(0,0,-90)*moveDir;
        else if(i == 2)
            moveDir = -moveDir;
        
        m_Transform.up = moveDir;
        if(Physics2D.Raycast(m_Transform.position,moveDir,moveLength,wallLayer))
        {
            RaycastHit2D hit = Physics2D.Raycast(m_Transform.position,moveDir,moveLength,wallLayer);
            m_Transform.DOMove(hit.point+hit.normal.normalized*0.3f,1).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                Attack();
            });
        }
        else
        {
            m_Transform.DOMove(m_Transform.position+moveDir*moveLength,1).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                Attack();
            });
        }
    }
    private void Attack()
    {
        currentBulletQuantity = bulletQuantity;
        Vector3 shootDir = (Player.Instance.m_Transform.position-m_Transform.position).normalized;
        shootDir.z = 0;
        if(Random.Range(0,2)==0)
            shootQuaternion = Quaternion.Euler(0,0,10);
        else
            shootQuaternion = Quaternion.Euler(0,0,-10);
        StartCoroutine(Shoot(shootDir));
    }
    private IEnumerator Shoot (Vector3 dir)
    {
        Transform bullet = ObjectPool.TakeFromPool("EnemyBullet");
        bullet.position = m_Transform.position+dir*0.5f;
        bullet.up = dir;
        m_Transform.up = dir;
        currentBulletQuantity -=1;
        yield return new WaitForSeconds(fireRate);
        if(currentBulletQuantity > 0)
            StartCoroutine(Shoot(shootQuaternion*dir));
        else
            acting = false;
    }

}
