using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DashEnemy : Enemy
{
    public Transform effect;
    public float aimTime;
    private bool aiming;
    private Vector3 attackDir;
    private float xScale = 0;

    void Update()
    {
        Ready();
    }
    private void Ready()
    {
        if(acting)
            return;

        attackDir = (Player.Instance.m_Transform.position-m_Transform.position).normalized;
        attackDir.z = 0;
        m_Transform.up = attackDir;
        effect.localScale = new Vector3(xScale,Vector3.Distance(Player.Instance.m_Transform.position+attackDir*3,m_Transform.position)*5,1);  
        if(aiming)
            return;
        aiming = true;
        DOTween.To(()=> xScale , x=> xScale = x, 0.5f, aimTime).OnComplete(()=>
        {
            Attack();
        });
    }
    private void Attack()
    {
        acting = true;
        aiming = false;
        xScale = 0;
        Vector3 originScale = effect.localScale;
        effect.localScale = new Vector3(0,originScale.y,originScale.z);
        if(Physics2D.Raycast(Player.Instance.m_Transform.position,attackDir,3,wallLayer))
        {
            RaycastHit2D hit = Physics2D.Raycast(Player.Instance.m_Transform.position,attackDir,3,wallLayer);
            
            m_Transform.DOMove(hit.point+hit.normal*0.2f,0.15f).OnComplete(()=>
            {
                acting = false;
            });
        }
        else
        {
            m_Transform.DOMove(Player.Instance.m_Transform.position+attackDir*3,0.15f).OnComplete(()=>
            {
                acting = false;
            });
        }
    }
}
