namespace AceleraPlenoProjetoFinal.Api.Repositories.Interfaces;

public interface ICargaRepository
{
    public bool InserirUnidadeInstituicao(string filePath);

    public bool InserirOperacao(string filePath);

    public bool InserirUsuario(string filePath);

    public bool InserirTerminal(string filePath);
}