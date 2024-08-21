namespace Domain.Functions;

public class SampleClass
{
    public string SampleMethod(int input)
    {
        return input == 1 ? "OK!" : "Failed";
    }
    public int SampleMethod(int first, int second)
    {
        return first + second;
    }
}