namespace BlazorBLE.Data;

public struct DataPoint
{
    public readonly ClassLabel Label;
    public readonly int[] Rssis;

    public DataPoint(ClassLabel label, int[] rssis)
    {
        Label = label;
        Rssis = rssis;
    }
}