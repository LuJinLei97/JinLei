namespace JinLei.Classes;
public class InitializableObject
{
    public InitializableObject() => Initialize();

    protected virtual void Initialize()
    {
    }
}