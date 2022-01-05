using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态机器类：由Role类控制，完成状态的存储、切换，和状态的保持
/// </summary>
public class StateMachine
{
    //存储当前机器所有的状态
    public Dictionary<int, StateBase> m_StateCache;

    //上一个状态
    public StateBase m_previousState;

    //当前状态
    public StateBase m_currentState;

    //机器初始化
    public StateMachine(StateBase begineState)
    {
        m_previousState = null;
        m_currentState = begineState;

        m_StateCache = new Dictionary<int, StateBase>();
        //添加初始状态
        AddState(begineState);
        m_currentState.OnEnter();
    }
    //player
    public bool isplayer = false;
    public Player player;

    public void AddState(StateBase state)
    {
        if (!m_StateCache.ContainsKey(state.ID))
        {
            m_StateCache.Add(state.ID, state);
            state.machine = this;
        }
    }

    //通过ID来切换状态
    public void TranslateToState(int stateID)
    {
        if (isKilled) return;
        if (!m_StateCache.ContainsKey(stateID)) return;

        m_currentState.OnExit();
        m_previousState = m_currentState;
        m_currentState = m_StateCache[stateID];
        m_currentState.OnEnter();
    }

    public void Update()
    {
        if(m_currentState != null)
        {
            m_currentState.OnStay();
        }
    }
    //杀死状态机
    private bool isKilled = false;
    public void KillStateMachine()
    {
        isKilled = true;
    }
}
