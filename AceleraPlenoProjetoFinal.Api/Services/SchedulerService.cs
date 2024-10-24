using AceleraPlenoProjetoFinal.Api.Data;


namespace AceleraPlenoProjetoFinal.Api.Services;



public class SchedulerService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;
    private readonly ILogger<SchedulerService> _logger;



    public SchedulerService(IServiceProvider serviceProvider, ILogger<SchedulerService> logger)
    {
        _serviceProvider = serviceProvider;
        _httpClient = new HttpClient();
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Frequência de verficação
        _timer = new Timer(ExecuteTasks, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); 
        return Task.CompletedTask;
    }


    private void ExecuteTasks(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var parametros = context.Parametrizacao.ToList();

            foreach (var parametro in parametros)
            {
                // Ajuste de hora/minuto/segundo
                var delay = TimeSpan.FromHours(parametro.IntervaloExecucao);

                Task.Delay(delay).ContinueWith(async _ =>
                {
                    var endpoint = GetEndpointUrl(parametro.IdParametrizacao);
                    if (endpoint != null)
                    {
                        try
                        {
                            var response = await _httpClient.PostAsync(endpoint, null);
                            response.EnsureSuccessStatusCode();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Erro ao chamar o endpoint {endpoint}");
                        }
                    }
                });
            }
        }
    }


    private string? GetEndpointUrl(int id)
    {
        switch (id)
        {
            case 1:
                return "http://aceleraapicarga.brazilsouth.azurecontainer.io:8080/api/v1/CargasScheduler/UnidadeInstituicao";
            case 2:     
                return "http://aceleraapicarga.brazilsouth.azurecontainer.io:8080/api/v1/CargasScheduler/OperacaoCaixa";
            case 3:     
                return "http://aceleraapicarga.brazilsouth.azurecontainer.io:8080/api/v1/CargasScheduler/Usuario";
            case 4:     
                return "http://aceleraapicarga.brazilsouth.azurecontainer.io:8080/api/v1/CargasScheduler/Terminal";
            default:
                _logger.LogWarning($"Nenhum endpoint configurado para o ID {id}");
                throw new ArgumentException($"ID inválido: {id}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

