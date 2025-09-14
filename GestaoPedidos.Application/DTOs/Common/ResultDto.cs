namespace GestaoPedidos.Application.DTOs.Common
{
    public class ResultDto<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }

        public static ResultDto<T> SucessoComDados(T dados, string mensagem = "")
        {
            return new ResultDto<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

        public static ResultDto<T> Erro(string mensagem)
        {
            return new ResultDto<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Dados = default
            };
        }
    }

    public class ResultDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public object? Dados { get; set; }

        public static ResultDto Ok(string mensagem = "")
        {
            return new ResultDto
            {
                Sucesso = true,
                Mensagem = mensagem
            };
        }

        public static ResultDto Erro(string mensagem)
        {
            return new ResultDto
            {
                Sucesso = false,
                Mensagem = mensagem
            };
        }
    }
}
