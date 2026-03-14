using Testcontainers.PostgreSql;

namespace CardiTrack.UnitTests.Infrastructure;

public static class PostgreSqlTestContainerFactory
{
    private const string Image = "postgres:17-alpine";

    public static PostgreSqlContainer CreateStandardContainer() =>
        new PostgreSqlBuilder(Image)
            .WithCleanUp(true)
            .Build();
}
