using BlazorBLE.Services;

namespace BlazorBLE.Tests;

public sealed class DelayedActionExecutorTest
{
    private const int DelayInMilliseconds = 5000;
        
    [Fact]
    public void DelayedActionExecutor_Start_InvokesSecondElapsedEvent()
    {
        int secondsElapsed = 0;
        
        DelayedActionExecutor delayedActionExecutor = new (DelayInMilliseconds, null);
        
        delayedActionExecutor.SecondElapsed += (_, _) => secondsElapsed++;
        delayedActionExecutor.Start();
        
        Thread.Sleep(DelayInMilliseconds + 1000);
        
        delayedActionExecutor.Stop();
        
        Assert.Equal(5, secondsElapsed);
    }
    
    [Fact]
    public void DelayedActionExecutor_Start_ExecutesActionAfter()
    {
        bool actionExecuted = false;
        
        DelayedActionExecutor delayedActionExecutor = new (DelayInMilliseconds, () => actionExecuted = true);
        delayedActionExecutor.Start();
        
        Thread.Sleep(DelayInMilliseconds + 1000);
        
        delayedActionExecutor.Stop();
        
        Assert.True(actionExecuted);
    }
}