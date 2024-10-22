namespace SharedKernel.ObjectHelper;

public static class ObjectHelperExtensions
{
    public static T As<T>(
            this object obj
        ) where T : class
    {
        return (T)obj;
    }
}
