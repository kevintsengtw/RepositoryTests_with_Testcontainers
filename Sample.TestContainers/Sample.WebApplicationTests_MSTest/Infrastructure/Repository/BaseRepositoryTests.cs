using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Sample.WebApplicationTests_MSTest.Infrastructure.Repository;

public abstract class BaseRepositoryTests<T> where T : class
{
    private IFixture _fixture;

    protected IFixture Fixture => this._fixture ??= new Fixture().Customize(new AutoNSubstituteCustomization())
                                                                 .Customize(new DatabaseHelperCustomization());

    protected string ConnectionString => TestHook.SampleDbConnectionString;

    protected StubRepository<T> Stub;

    [TestInitialize]
    public void TestInitialize()
    {
        this.Stub = this.Fixture.Create<StubRepository<T>>();
    }
}