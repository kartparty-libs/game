public class Offline
{

    private bool _returnLogin;
    private bool _loading;
    public Offline()
    {

    }
    public void SetOffline()
    {
        _returnLogin = true;
        if (_loading)
        {
            return;
        }
        doReturn();
    }
    public void LoadMapStart()
    {
        _loading = true;
    }
    public void LoadMapEnd()
    {
        _loading = false;
        if (_returnLogin)
        {
            doReturn();
        }
    }
    private void doReturn()
    {
        _returnLogin = false;
        GameEntry.GameProcedure.ChangeTo(EnumDefine.GameProcedureName.GameLogin.ToString());
    }
}