using Framework;

public class NetMgr
{
    public INetworkHandler MainHandler;
    public INetworkHandler BattleHandler;
    public NetMgr()
    {
        
    }
    public void Connect(NetworkAddress i_pNetworkAddress)
    {
        NetworkManager.Instance.Connect(i_pNetworkAddress);
    }
    public void Send(params object[] i_pArgs)
    {
        CSharpNetworkHandler.Instance.Send(i_pArgs);
    }
    public void Close()
    {
        NetworkManager.Instance.Close();
    }
    public void Update(float deltaTime)
    {

    }
}