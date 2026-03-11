using Testcontainers.MsSql;

namespace CardiTrack.UnitTests.Infrastructure;

public static class SqlServerTestContainerFactory
{
    private const string Image = "mcr.microsoft.com/mssql/server:2022-latest";

    public static MsSqlContainer CreateStandardContainer() =>
        new MsSqlBuilder(Image)
            .WithCleanUp(true)
            .Build();
}
