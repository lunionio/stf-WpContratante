using Admin.Helppser;
using Admin.Models;
using Admin.Models.Financeiro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Admin.Helppers
{
    public static class FinanceiroHelper
    {
        public static bool VerifcaSaldoCliente(decimal valorVaga, LoginViewModel usuario)
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpFinanceiro/BuscarSaldo/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                destino = usuario.idEmpresa
            };

            var helper = new ServiceHelper();
            var result = helper.Post<object>(url, envio);

            var saldo = Convert.ToDecimal(result);

            return saldo >= valorVaga;
        }

        public static void LancaTransacoes(OportunidadeViewModel vaga, LoginViewModel usuario)
        {
            IList<Extrato> extratos = new List<Extrato>();
            for (int i = 0; i < vaga.Quantidade; i++)
            {
                var valor1 = (vaga.Valor) * -1;

                var extrato1 = new Extrato(valor1, 2, 1, usuario.idEmpresa.ToString(),
                    usuario.idEmpresa.ToString(), vaga.ID, Status.Aprovado)
                {
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow,
                    DataEdicao = DateTime.UtcNow,
                    Descricao = "Debitando valor da vaga.",
                    IdCliente = usuario.idCliente,
                    Nome = "Débito",
                    Status = 1,
                    UsuarioCriacao = usuario.IdUsuario,
                    UsuarioEdicao = usuario.IdUsuario,
                    TipoOrigem = 3,
                    TipoDestino = 3,
                };

                var valor2 = vaga.Valor;

                var extrato2 = new Extrato(valor2, 2, 1, usuario.idEmpresa.ToString(),
                    "16", vaga.ID, Status.Bloqueado)
                {
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow,
                    DataEdicao = DateTime.UtcNow,
                    Descricao = "Disponibilizando valor da vaga.",
                    IdCliente = usuario.idCliente,
                    Nome = "Pagamento",
                    Status = 1,
                    UsuarioCriacao = usuario.IdUsuario,
                    UsuarioEdicao = usuario.IdUsuario,
                    TipoOrigem = 3,
                    TipoDestino = 2,
                };

                extratos.Add(extrato1);
                extratos.Add(extrato2);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpFinanceiro/AlocarCredito/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var envio = new
            {
                extratos,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<object>(url, envio);
        }
    }
}