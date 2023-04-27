namespace BlazorBLE.Services;

public sealed class DelayedActionExecutor
{
    private Timer timer;
    private CancellationTokenSource cancellationTokenSource;
    
    private Action actionToExecute;
    
    private readonly int delayInMilliseconds;
    
    private int elapsedMilliseconds;

    public bool IsRunning { get; private set; }

    public event EventHandler SecondElapsed;

    public DelayedActionExecutor(int delayInMilliseconds, Action actionToExecute)
    {
        this.delayInMilliseconds = delayInMilliseconds;
        this.actionToExecute = actionToExecute;
        elapsedMilliseconds = delayInMilliseconds;
    }

    public void Start()
    {
        if (IsRunning) return;

        IsRunning = true;
        cancellationTokenSource = new CancellationTokenSource();
        timer = new Timer(TimerCallback, null, 0, 1000);
    }

    public void Stop()
    {
        if (!IsRunning) return;

        IsRunning = false;
        elapsedMilliseconds = delayInMilliseconds;
        
        timer.Dispose();
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    private void TimerCallback(object state)
    {
        if (cancellationTokenSource.Token.IsCancellationRequested)
        {
            return;
        }

        SecondElapsed?.Invoke(this, EventArgs.Empty);

        elapsedMilliseconds -= 1000;

        if (elapsedMilliseconds <= 0)
        {
            actionToExecute?.Invoke();
            Stop();
        }
    }
}