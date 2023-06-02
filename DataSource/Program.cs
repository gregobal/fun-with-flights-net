using System.Text;
using CliWrap;
using CliWrap.Exceptions;

const string container = "flights-db";
const string sqlDataPath = "demo.sql";
const string postgresUser = "postgres";
const string postgresPassword = "p@ssw0rd";
const int postgresPort = 5432;

await using var stdOut = Console.OpenStandardOutput();
await using var stdErr = Console.OpenStandardError();

var cmd = Cli.Wrap("docker")
              .WithArguments(
                  $"run --name {container} -p {postgresPort}:{postgresPort} -e POSTGRES_PASSWORD={postgresPassword} -d postgres")
          | (stdOut, stdErr);

await cmd.ExecuteAsync();

var stdOutBuffer = new StringBuilder();

cmd = Cli.Wrap("docker")
          .WithArguments($"exec {container} pg_isready")
      | stdOutBuffer;

while (!stdOutBuffer.ToString().Contains("accepting connections"))
{
    stdOutBuffer.Clear();
    Thread.Sleep(500);

    try
    {
        await cmd.ExecuteAsync();
    }
    catch (CommandExecutionException)
    {
        if (!stdOutBuffer.ToString().Contains("no response"))
        {
            throw;
        }
    }
}

await using var sqlData = File.OpenRead(sqlDataPath);

cmd = sqlData
      | Cli.Wrap("docker")
          .WithArguments($"exec -i {container} psql -U {postgresUser}")
      | (stdOut, stdErr);

await cmd.ExecuteAsync();

Console.WriteLine("Done!");