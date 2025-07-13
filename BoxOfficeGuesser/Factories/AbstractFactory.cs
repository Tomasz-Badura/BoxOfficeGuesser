namespace BoxOfficeGuesser.Factories;

public abstract class AbstractFactory<T>
{
    protected IServiceProvider provider;

    /// <summary>
    /// Validate 1 parameter
    /// </summary>
    /// <typeparam name="TValue1">Param1 type</typeparam>
    /// <param name="parameters">parameters passed to the Create method</param>
    /// <exception cref="ArgumentException">Thrown when parameters didn't validate correctly</exception>
    protected void ThrowParams<TValue1>(params object[] parameters)
    {
        if(parameters.Length != 1)
        {
            throw new ArgumentException("Create takes in 1 parameter", nameof(parameters));
        }

        if(parameters[0] is not TValue1)
        {
            throw new ArgumentException($"Parameter 0 is not of type {nameof(TValue1)}", nameof(parameters));
        }
    }

    /// <summary>
    /// Validate 2 parameters
    /// </summary>
    /// <typeparam name="TValue1">Param1 type</typeparam>
    /// <typeparam name="TValue2">Param2 type</typeparam>
    /// <param name="parameters">parameters passed to the Create method</param>
    /// <exception cref="ArgumentException">Thrown when parameters didn't validate correctly</exception>
    protected void ThrowParams<TValue1, TValue2>(params object[] parameters)
    {
        if(parameters.Length != 2)
        {
            throw new ArgumentException("Create takes in 2 parameters", nameof(parameters));
        }

        if(parameters[0] is not TValue1)
        {
            throw new ArgumentException($"Parameter 0 is not of type {nameof(TValue1)}", nameof(parameters));
        }

        if(parameters[1] is not TValue2)
        {
            throw new ArgumentException($"Parameter 0 is not of type {nameof(TValue2)}", nameof(parameters));
        }
    }

    /// <summary>
    /// Validate 3 parameters
    /// </summary>
    /// <typeparam name="TValue1">Param1 type</typeparam>
    /// <typeparam name="TValue2">Param2 type</typeparam>
    /// <typeparam name="TValue3">Param3 type</typeparam>
    /// <param name="parameters">parameters passed to the Create method</param>
    /// <exception cref="ArgumentException">Thrown when parameters didn't validate correctly</exception>
    protected void ThrowParams<TValue1, TValue2, TValue3>(params object[] parameters)
    {
        if(parameters.Length != 3)
        {
            throw new ArgumentException("Create takes in 3 parameters", nameof(parameters));
        }

        if(parameters[0] is not TValue1)
        {
            throw new ArgumentException($"Parameter 0 is not of type {nameof(TValue1)}", nameof(parameters));
        }

        if(parameters[1] is not TValue2)
        {
            throw new ArgumentException($"Parameter 0 is not of type {nameof(TValue2)}", nameof(parameters));
        }

        if(parameters[2] is not TValue3)
        {
            throw new ArgumentException($"Parameter 0 is not of type {nameof(TValue3)}", nameof(parameters));
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
