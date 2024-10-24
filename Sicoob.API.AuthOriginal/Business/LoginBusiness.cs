using Acelera.API.AuthOriginal.Repository.Interface;
using System.Drawing;
using System.Drawing.Imaging;

namespace Acelera.API.AuthOriginal.Business
{
    public class LoginBusiness
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMailRepository _mailRepository;

        public LoginBusiness(ILoginRepository loginRepository, IMailRepository mailRepository)
        {
            _loginRepository = loginRepository;
            _mailRepository = mailRepository;
        }

        public void EnviarEmailPrimeiroAcesso(string nome, string login, string email, Bitmap qrCode)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCode.Save(ms, ImageFormat.Png);
                    byte[] qrCodeBytes = ms.ToArray();
                }

                var destinatario = email;
                var assunto = "Acelera - Primeiro Acesso";
                var corpoEmail = $@"
        <html>
        <head>
            <title>Primeiro Acesso ao Sistema Acelera</title>
        </head>
        <body>
            <h1>Olá, {nome}!</h1>
            <p>Segue abaixo o seu login para acessar o sistema:</p>
            <p><strong>Login:</strong> {login}</p>
            <p>Use o QR Code anexado para acessar o sistema:</p>
            <br>
            <p><a href='http://localhost:4200/redefinicaoSenha?access=true&login={login}'>Acessar o Sistema</a></p>
            <h3>Time Acelera Pleno 2024</h3>
        </body>
        </html>";

                _mailRepository.SendEmailComAnexo(destinatario, assunto, corpoEmail, qrCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível enviar email de primeiro acesso. Tente novamente mais tarde!", ex);
            }
        }



        /// <summary>
        /// Método que envia email de redefinição de senha.
        /// </summary>
        public async void EnviarEmailRedefinirSenha(string email)
        {
            try
            {
                if (_loginRepository.EsqueciMinhaSenhaMail(email))
                {
                    var login = await _loginRepository.GetLoginByEmail(email);
                    var destinatario = email;
                    var assunto = "Redefinir Senha - Acelera";
                    var corpoEmail = $@"
                        <html>
                        <head>
                            <title>Redefinir senha de Acesso ao Sistema Acelera</title>
                        </head>
                        <body>
                            <h1>Olá!</h1>
                            <p>Segue abaixo o link para redefinição de senha</p><br><br>
                            <a href='http://localhost:4200/redefinicaoSenha?access=true&login={login}'>Redefinir Senha</a>
                            <br><br><br><p>Caso não tenha solicitado este e-mail, entrar em contato imediantamente com o adm do sistema!</p>
                
                            <br><br><h3>Time Acelera Pleno 2024</h3>
                        </body>
                        </html>";

                    _mailRepository.SendEmail(destinatario, assunto, corpoEmail);
                }
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível enviar email de redefinição de senha. Tente novamente mais tarde!");
            }
        }

        public bool EsqueciMinhaSenhaMail(string email)
        {
            var existeLogin = _loginRepository.EsqueciMinhaSenhaMail(email);

            if (existeLogin)
            {
                EnviarEmailRedefinirSenha(email);
            }

            return existeLogin;
        }
    }
}
