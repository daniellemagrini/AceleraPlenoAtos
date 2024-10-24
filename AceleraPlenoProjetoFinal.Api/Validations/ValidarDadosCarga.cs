namespace AceleraPlenoProjetoFinal.Api.Validations;

public class ValidarDadosCarga
{
    public int? VerificarDadosInteiros(string? valor)
    {
        if (String.IsNullOrEmpty(valor) || valor.ToUpper().ToString() == "NULL")
            return null;

        return int.Parse(valor.ToString());
    }

    public string? VerificarDadosTexto(string? valor)
    {
        if (String.IsNullOrEmpty(valor) || valor.ToUpper().ToString() == "NULL")
            return null;

        return valor.ToString();
    }
    public DateTime? VerificarDadosData(string? valor)
    {
        if (String.IsNullOrEmpty(valor) || valor.ToUpper().ToString() == "NULL")
            return null;

        return DateTime.Parse(valor);
    }

    public Decimal? VerificarDadosMonetario(string? valor)
    {
        if (String.IsNullOrEmpty(valor) || valor.ToUpper().ToString() == "NULL")
            return null;

        return Decimal.Parse(valor);
    }
}