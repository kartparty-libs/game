using Framework;

public class GameProcedureManager
{
    private Fsm _fsm;
    public GameProcedureManager(Fsm value)
    {
        _fsm = value;
    }
    public FsmState GetCurrFsmState()
    {
        return _fsm.CurrentState;
    }
    public void Update(float deltaTime, float unscaledDeltaTime)
    {
        _fsm.Update(deltaTime);
    }
    public void ChangeTo(string name)
    {
        _fsm.ForceChange(name);
    }
}