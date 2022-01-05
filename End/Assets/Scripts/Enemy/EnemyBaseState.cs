/// <summary>
/// 状态的基础类：给子类提供方法
/// </summary>
public abstract class EnemyBaseState
{
    //给每个状态设置一个ID
    public int ID { get; set; }

    //被当前机器所控制
    public StateMachine machine;

    //public EnemyBaseState(int id)
    //{
    //    this.ID = id;
    //}
    public abstract void EnterState(Enemy enemy);//进入状态
    public abstract void OnUpdate(Enemy enemy);//当前状态下，保持运行
}
