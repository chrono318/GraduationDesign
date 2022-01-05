/// <summary>
/// 状态拥有者（泛式）
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class StateTemplate<T> : StateBase
{
    public T owner;

    public StateTemplate(int id,T o):base(id)
    {
        owner = o;
    }
}
