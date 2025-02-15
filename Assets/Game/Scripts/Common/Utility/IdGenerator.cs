using System.Threading;

public static class IdGenerator
{
    static int counter = 0;

    public static int GenerateId()
    {
        return Interlocked.Increment(ref counter);
    }
}
