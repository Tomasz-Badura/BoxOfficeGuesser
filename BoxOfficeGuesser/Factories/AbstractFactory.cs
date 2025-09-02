namespace BoxOfficeGuesser.Factories;

public abstract class AbstractFactory<T>
{
    protected readonly IServiceProvider provider;

    protected void ValidateParams(object[] parameters, params Type[] expectedTypes)
    {
        if(parameters.Length != expectedTypes.Length)
        {
            throw new ArgumentException($"Create takes {expectedTypes.Length} parameter(s).", nameof(parameters));
        }

        for(int i = 0; i < parameters.Length; i++)
        {
            if(!expectedTypes[i].IsInstanceOfType(parameters[i]))
            {
                throw new ArgumentException($"Parameter {i} is not of type {expectedTypes[i].Name}.", nameof(parameters));
            }
        }
    }

    protected AbstractFactory(IServiceProvider provider)
    {
        this.provider = provider;
    }

    /// <summary>
    /// Creates an instance of T with given parameters
    /// </summary>
    /// <param name="parameters">Parameters for constructing the object</param>
    /// <returns>Created object</returns>
    public abstract T Create(params object[] parameters);
}
