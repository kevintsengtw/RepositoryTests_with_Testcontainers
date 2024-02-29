using Sample.WebApplication.Infrastructure.Helpers;

namespace Sample.WebApplicationTests_MSTest;

public class StubRepository<T> where T : class
{
    private readonly DatabaseHelper _databaseHelper;

    public StubRepository(DatabaseHelper databaseHelper)
    {
        this._databaseHelper = databaseHelper;
    }

    private T _systemUnderTest;

    internal T SystemUnderTest
    {
        get
        {
            this._systemUnderTest = Activator.CreateInstance(typeof(T), this._databaseHelper) as T;
            return this._systemUnderTest;
        }
        set => this._systemUnderTest = value;
    }
}