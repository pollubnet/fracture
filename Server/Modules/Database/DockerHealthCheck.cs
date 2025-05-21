using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fracture.Server.Modules.Database;

public class DockerHealthCheck : IHealthCheck
{
    private readonly DockerClient _dockerClient;

    public DockerHealthCheck(DockerClient dockerClient)
    {
        _dockerClient = dockerClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
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
                            new Dictionary<string, bool> { { "fracturePostgres", true } }
                        },
                    },
                },
                cancellationToken
            );

            var container = containers.FirstOrDefault();
            if (container == null)
            {
                return HealthCheckResult.Unhealthy("PostgreSQL container not found");
            }

            var containerInfo = await _dockerClient.Containers.InspectContainerAsync(
                container.ID,
                cancellationToken
            );

            return containerInfo.State.Health?.Status switch
            {
                "healthy" => HealthCheckResult.Healthy(),
                "starting" => HealthCheckResult.Degraded("PostgreSQL container is starting"),
                "unhealthy" => HealthCheckResult.Unhealthy("PostgreSQL container is unhealthy"),
                null when containerInfo.State.Running => HealthCheckResult.Healthy(
                    "PostgreSQL container is running, no healthcheck"
                ),
                null => HealthCheckResult.Unhealthy("PostgreSQL container is not running"),
                _ => HealthCheckResult.Unhealthy(
                    $"Unknown status: {containerInfo.State.Health?.Status}"
                ),
            };
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy("Failed to check Docker container", e);
        }
    }
}
