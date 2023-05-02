namespace BlazorBLE.Services;

public sealed class DelayedActionExecutor
{
    private Timer timer;
    private CancellationTokenSource cancellationTokenSource;
    
    private readonly Action actionToExecute;
    
    private readonly int delayInMilliseconds;
    
    private int timeLeftMilliseconds;

    public bool IsRunning { get; private set; }

    public event EventHandler SecondElapsed;

    public DelayedActionExecutor(int delayInMilliseconds, Action actionToExecute)
    {
        this.delayInMilliseconds = delayInMilliseconds;
        this.actionToExecute = actionToExecute;
        timeLeftMilliseconds = delayInMilliseconds;
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
        timeLeftMilliseconds = delayInMilliseconds;
        
        timer.Dispose();
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    private void TimerCallback(object state)
    {
        if (cancellationTokenSource.Token.IsCancellationRequested) return;

        timeLeftMilliseconds -= 1000;
        SecondElapsed?.Invoke(this, EventArgs.Empty);

        if (elapsedMilliseconds <= 0)
        {
            actionToExecute?.Invoke();
            Stop();
        }
    }
}