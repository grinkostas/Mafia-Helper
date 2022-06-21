
public interface IMomento<out T> where T : ISnapshot
{
    T TakeSnapshot();
}
