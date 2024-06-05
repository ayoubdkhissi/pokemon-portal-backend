namespace Tests.TestSupport;
public abstract class GivenWhenThenTest : IDisposable
{
    private bool _disposed;

    protected GivenWhenThenTest()
    {
        Setup();
    }

    private void Setup()
    {
        Given();
        When();
    }

    protected abstract void Given();
    protected abstract void When();
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                CleanUp();
            }
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void CleanUp() { }
}
