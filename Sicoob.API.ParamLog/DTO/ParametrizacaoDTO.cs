using System.ComponentModel.DataAnnotations;

namespace Acelera.API.ParamLog.DTO
{
    public class ParametrizacaoDTO
    {
        [Required(ErrorMessage = "O campo ID é obrigatório!")]
        public int IDPARAMETRIZACAO { get; set; }

        [Required(ErrorMessage = "O campo Caminho do arquivo é obrigatório!")]
        public string CAMINHOCARGA { get; set; }

        [Required(ErrorMessage = "O campo Intervalo de execução é obrigatório!")]
        public int INTERVALOEXECUCAO { get; set; }
    }
}
