using Docker.DotNet;
using Docker.DotNet.Models;

namespace Fracture.Server.Modules.Database;

public class DockerService
{
    private readonly DockerClient _dockerClient;
    private readonly ILogger<DockerService> _logger;
    private const string ContainerName = "fracturePostgres";
    private const string ImageName = "postgres:latest";
    private const string VolumeName = "fracturePostgresData";
    public int? AssignedHostPort;

    public DockerService(ILogger<DockerService> logger)
    {
        _dockerClient = new DockerClientConfiguration(
            new Uri(
                OperatingSystem.IsWindows()
                    ? "npipe://./pipe/docker_engine"
                    : "unix:/var/run/docker.sock"
            )
        ).CreateClient();
        _logger = logger;
    }

    public async Task EnsurePostgresRunningAsync()
    {
        try
        {
            var containers = await _dockerClient.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    All = true,
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        {
                            "name",
                            new Dictionary<string, bool> { { ContainerName, true } }
                        },
                    },
                }
            );

            var existingContainer = containers.FirstOrDefault();

            if (existingContainer != null)
            {
                if (existingContainer.State == "running")
                {
                    _logger.LogInformation("PostgreSQL container is already running");
                    AssignedHostPort = GetHostPortFromContainer(existingContainer);
                    return;
                }

                await _dockerClient.Containers.StartContainerAsync(
                    existingContainer.ID,
                    new ContainerStartParameters()
                );
                _logger.LogInformation("Started existing PostgreSQL container");
                AssignedHostPort = GetHostPortFromContainer(existingContainer);
            }
            else
            {
                var volumes = await _dockerClient.Volumes.ListAsync();

                if (!volumes.Volumes.Any(v => v.Name == VolumeName))
                {
                    await _dockerClient.Volumes.CreateAsync(
                        new VolumesCreateParameters { Name = VolumeName }
                    );
                }

                var portBindings = await TryGetPortBindings();

                var response = await _dockerClient.Containers.CreateContainerAsync(
                    new CreateContainerParameters
                    {
                        Image = ImageName,
                        Name = ContainerName,
                        Env = new List<string>
                        {
                            "POSTGRES_USER=postgres",
                            "POSTGRES_PASSWORD=postgres",
                            "POSTGRES_DB=fracturedb",
                        },

                        HostConfig = new HostConfig
                        {
                            PortBindings = portBindings,
                            Binds = new List<string> { $"{VolumeName}:/var/lib/postgresql/data" },
                            RestartPolicy = new RestartPolicy
                            {
                                Name = RestartPolicyKind.UnlessStopped,
                            },
                        },

                        Healthcheck = new HealthConfig
                        {
                            Test = new List<string> { "CMD-SHELL", "pg_isready -U postgres" },
                            Interval = TimeSpan.FromSeconds(1),
                            Timeout = TimeSpan.FromMilliseconds(500),
                            Retries = 5,
                        },
                    }
                );

                await _dockerClient.Containers.StartContainerAsync(
                    response.ID,
                    new ContainerStartParameters()
                );
                _logger.LogInformation("Created and started PostgreSQL container");

                var containerInfo = await _dockerClient.Containers.InspectContainerAsync(
                    response.ID
                );

                AssignedHostPort = GetHostPortFromContainer(containerInfo);
            }

            await WaitForPostgresReady();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostgreSQL container failed");
            throw;
        }
    }

    private async Task<Dictionary<string, IList<PortBinding>>> TryGetPortBindings()
    {
        try
        {
            return new Dictionary<string, IList<PortBinding>>
            {
                {
                    "5432/tcp",
                    new List<PortBinding> { new PortBinding { HostPort = "5432" } }
                },
            };
        }
        catch (DockerApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            _logger.LogWarning(ex, "Port 5432 is occupied, using antoher avaiable port");
            return new Dictionary<string, IList<PortBinding>>
            {
                {
                    "5432/tcp",
                    new List<PortBinding> { new PortBinding { HostPort = "0" } }
                },
            };
        }
    }

    private int GetHostPortFromContainer(ContainerListResponse container)
    {
        if (container.Ports != null && container.Ports.Any())
        {
            return int.Parse(container.Ports[0].PublicPort.ToString());
        }

        return 5432;
    }

    private int GetHostPortFromContainer(ContainerInspectResponse container)
    {
        var portBinding = container.NetworkSettings.Ports["5432/tcp"]?.FirstOrDefault();
        return portBinding != null ? int.Parse(portBinding.HostPort) : 5432;
    }

    private async Task WaitForPostgresReady()
    {
        const int maxAttempts = 30;
        var attempt = 0;

        while (attempt < maxAttempts)
        {
            attempt++;
            _logger.LogInformation(
                $"Waiting for PostgreSQL to be ready (attempt {attempt}/{maxAttempts})"
            );

            try
            {
                var containers = await _dockerClient.Containers.ListContainersAsync(
                    new ContainersListParameters()
                    {
                        Filters = new Dictionary<string, IDictionary<string, bool>>
                        {
                            {
                                "name",
                                new Dictionary<string, bool> { { ContainerName, true } }
                            },
                            {
                                "health",
                                new Dictionary<string, bool> { { "healthy", true } }
                            },
                        },
                    }
                );

                if (containers.Any())
                {
                    _logger.LogInformation("PostgreSQL container is ready");
                    return;
                }

                var containerInfo = await _dockerClient.Containers.InspectContainerAsync(
                    containers.FirstOrDefault()?.ID ?? ""
                );

                if (containerInfo.State.Running && containerInfo.State.Health == null)
                {
                    _logger.LogInformation("PostgreSQL container is running (no health check)");
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e, "Health check failed");
            }

            await Task.Delay(1000);
        }

        throw new TimeoutException("PostgreSQL container timed out");
    }
}
