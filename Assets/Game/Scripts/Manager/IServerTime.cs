public interface IServerTime
{
    void UpdateSeconds();
    void UpdateMinutes();
    void NewDay();
    void Reset();
}