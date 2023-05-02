namespace BlazorBLE.Services;

public sealed class DelayedActionExecutor
{
    public int SecondsRemaining { get; private set; }
    
    public bool IsRunning { get; private set; }

    private Timer timer;
    private CancellationTokenSource cancellationTokenSource;
    
    private readonly Action actionToExecute;
    
    private readonly int delayInSeconds;
    
    public event Action SecondElapsed;

    public DelayedActionExecutor(int delayInSeconds, Action actionToExecute)
    {
        this.delayInSeconds = delayInSeconds;
        this.actionToExecute = actionToExecute;
        SecondsRemaining = delayInSeconds;
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
        SecondsRemaining = delayInSeconds;
        
        timer.Dispose();
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    private void TimerCallback(object state)
    {
        if (cancellationTokenSource.Token.IsCancellationRequested) return;

        SecondsRemaining--;
        SecondElapsed?.Invoke();

        if (SecondsRemaining <= 0)
        {
            actionToExecute?.Invoke();
            Stop();
        }
    }
}