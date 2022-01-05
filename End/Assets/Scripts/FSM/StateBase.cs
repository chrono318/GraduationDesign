/// <summary>
/// 状态的基础类：给子类提供方法
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class StateBase
{

    //给每个状态设置一个ID
    public int ID { get; set; }

    //被当前机器所控制
    public StateMachine machine;

    public StateBase(int id)
    {
        this.ID = id;
    }

    //给子类提供方法
    public virtual void OnEnter()
    {
        
    }
    public virtual void OnStay()
    {

    }
    public virtual void OnExit()
    {

    }

}
