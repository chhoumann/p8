using BlazorBLE.Services;

namespace BlazorBLE.Tests;

public sealed class DelayedActionExecutorTest
{
    private const int DelayInSeconds = 5;
        
    [Fact]
    public void DelayedActionExecutor_Start_InvokesSecondElapsedEvent()
    {
        int secondsElapsed = 0;
        
        DelayedActionExecutor delayedActionExecutor = new(DelayInSeconds, null);
        
        delayedActionExecutor.SecondElapsed += () => secondsElapsed++;
        delayedActionExecutor.Start();
        
        Thread.Sleep(TimeSpan.FromSeconds(DelayInSeconds + 1));
        
        Assert.Equal(5, secondsElapsed);
    }
    
    [Fact]
    public void DelayedActionExecutor_Start_ExecutesActionAfter()
    {
        bool actionExecuted = false;
        
        DelayedActionExecutor delayedActionExecutor = new (DelayInSeconds, () => actionExecuted = true);
        delayedActionExecutor.Start();
        
        Thread.Sleep(TimeSpan.FromSeconds(DelayInSeconds + 1));
        
        Assert.True(actionExecuted);
    }
}