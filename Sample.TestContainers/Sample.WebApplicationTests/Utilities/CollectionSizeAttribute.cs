using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;

namespace Sample.WebApplicationTests.Utilities;

/// <summary>
/// Class CollectionSizeAttribute
/// </summary>
/// <seealso cref="CustomizeAttribute"/>
public class CollectionSizeAttribute : CustomizeAttribute
{
    /// <summary>
    /// The size
    /// </summary>
    private readonly int _size;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionSizeAttribute"/> class
    /// </summary>
    /// <param name="size">The size</param>
    public CollectionSizeAttribute(int size)
    {
        this._size = size;
    }

    /// <summary>
    /// Gets the customization using the specified parameter
    /// </summary>
    /// <param name="parameter">The parameter</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException">{nameof(CollectionSizeAttribute)} specified for type incompatible with List: {parameter.ParameterType} {parameter.Name}</exception>
    /// <returns>The customization</returns>
    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        if (parameter is null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        var objectType = parameter.ParameterType.GetGenericArguments()[0];

        var isTypeCompatible = parameter.ParameterType.IsGenericType
                               &&
                               parameter.ParameterType.GetGenericTypeDefinition().MakeGenericType(objectType).IsAssignableFrom(typeof(List<>).MakeGenericType(objectType));

        if (isTypeCompatible is false)
        {
            throw new InvalidOperationException($"{nameof(CollectionSizeAttribute)} specified for type incompatible with List: {parameter.ParameterType} {parameter.Name}");
        }

        var customizationType = typeof(CollectionSizeCustomization<>).MakeGenericType(objectType);

        return (ICustomization)Activator.CreateInstance(customizationType, parameter, this._size);
    }

    /// <summary>
    /// Class CollectionSizeCustomization
    /// </summary>
    /// <seealso cref="ICustomization"/>
    private class CollectionSizeCustomization<T> : ICustomization
    {
        /// <summary>
        /// The parameter
        /// </summary>
        private readonly ParameterInfo _parameter;

        /// <summary>
        /// The repeat count
        /// </summary>
        private readonly int _repeatCount;

        /// <summary>
        /// Initializes a new instance of the <see>
        ///     <cref>CollectionSizeCustomization</cref>
        /// </see>
        /// class
        /// </summary>
        /// <param name="parameter">The parameter</param>
        /// <param name="repeatCount">The repeat count</param>
        public CollectionSizeCustomization(ParameterInfo parameter, int repeatCount)
        {
            this._parameter = parameter;
            this._repeatCount = repeatCount;
        }

        /// <summary>
        /// Customizes the fixture
        /// </summary>
        /// <param name="fixture">The fixture</param>
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add
            (
                new FilteringSpecimenBuilder
                (
                    new FixedBuilder(fixture.CreateMany<T>(this._repeatCount).ToList()),
                    new EqualRequestSpecification(this._parameter)
                )
            );
        }
    }
}