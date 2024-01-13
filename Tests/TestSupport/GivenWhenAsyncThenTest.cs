namespace Tests.TestSupport;
public abstract class GivenWhenAsyncThenTest : IDisposable
{
    private bool _disposed;

    protected GivenWhenAsyncThenTest()
    {
        Task.Run(SetupAsync).GetAwaiter().GetResult();
    }
    private async Task SetupAsync()
    {
        Given();
        await WhenAsync().ConfigureAwait(false);
    }
    
    protected abstract void Given();
    protected abstract Task WhenAsync();
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Cleanup();
            }
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Cleanup()
    {
    }
}
