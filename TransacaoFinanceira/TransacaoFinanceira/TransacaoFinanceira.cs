using System;
using System.Collections.Generic;
//using System.Threading.Tasks;


//Obs: Voce é livre para implementar na linguagem de sua preferência, desde que respeite as funcionalidades e saídas existentes, além de aplicar os conceitos solicitados.

namespace TransacaoFinanceira
{
    public class Transacao
    {
       public int CorrelationId { get; set;}
       public string Datetime {get;set;}
       public uint ContaOrigem {get;set;}
       public uint ContaDestino {get;set;}
       public decimal Valor {get;set;}

       public Transacao(int correlationId, string datetime, uint contaOrigem, uint contaDestino, decimal valor)
       {
            this.CorrelationId = correlationId;
            this.Datetime = datetime;
            this.ContaOrigem = contaOrigem;
            this.ContaDestino = contaDestino;
            this.Valor = valor;
       }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Lista de transacoes
            var transacoes = new Transacao[]
            {
                new(1,"09/09/2023 14:15:00",938485762,2147483649,150), //possivel
                new(2,"09/09/2023 14:15:05",2147483649,210385733,149), //possivel after 1
                new(3,"09/09/2023 14:15:29",347586970,238596054,1100), //possivel 
                new(4,"09/09/2023 14:17:00",675869708,210385733,5300), //imp
                new(5,"09/09/2023 14:18:00",238596054,674038564,1489), //possivel after 3
                new(6,"09/09/2023 14:18:20",573659065,563856300,49), //possivel
                new(7,"09/09/2023 14:19:00",938485762,2147483649,44), //imp after 1
                new(8,"09/09/2023 14:19:01",573659065,675869708,150), //possivel
            };
            
            ExecutarTransacaoFinanceira executor = new ExecutarTransacaoFinanceira();
            Array.ForEach(transacoes, item => //processar cada transacao na ordem em que foi criada
            {
                executor.Transferir(item.CorrelationId, item.ContaOrigem, item.ContaDestino, item.Valor);
            });

        }
    }

    public class ExecutarTransacaoFinanceira: AcessoDados
    {
        public void Transferir(int correlationId, uint contaOrigem, uint contaDestino, decimal valor)
        {
            //validacao do saldo da conta de origem
            ContasSaldo contaSaldoOrigem = GetSaldo<ContasSaldo>(contaOrigem);
            if (contaSaldoOrigem.Saldo < valor)
            {
                Console.WriteLine("Transacao numero {0} foi cancelada por falta de saldo", correlationId);
            }
            else
            {
                ContasSaldo contaSaldoDestino = GetSaldo<ContasSaldo>(contaDestino);
                contaSaldoOrigem.Saldo -= valor;
                contaSaldoDestino.Saldo += valor;
                Console.WriteLine("Transacao numero {0} foi efetivada com sucesso! Novos saldos: Conta Origem: {1} | Conta Destino: {2}", correlationId, contaSaldoOrigem.Saldo, contaSaldoDestino.Saldo);
                AtualizarSaldo(contaSaldoOrigem);
                AtualizarSaldo(contaSaldoDestino);
            }
        }
    }
    
    public class ContasSaldo
    {
        public ContasSaldo(uint conta, decimal valor)
        {
            this.Conta = conta;
            this.Saldo = valor;
        }
        public uint Conta { get; set; }
        public decimal Saldo { get; set; }
    }

    public class AcessoDados
    {
        //Dictionary<uint, decimal> Saldos { get; set; }
        private List<ContasSaldo> TabelaSaldos;
        public AcessoDados()
        {
            //contas existentes e seus respectivos saldos
            TabelaSaldos = new List<ContasSaldo>
            {
                new(938485762, 180),
                new(347586970, 1200),
                new(2147483649, 0),
                new(675869708, 4900),
                new(238596054, 478),
                new(573659065, 787),
                new(210385733, 10),
                new(674038564, 400),
                new(563856300, 1200)
            };


            //Saldos = new Dictionary<uint, decimal>();
            //this.Saldos.Add(938485762, 180);
           
        }

        public T GetSaldo<T>(uint id)
        {      
            //procura o id da conta na TabelaSaldos e converte para T    
            return (T)Convert.ChangeType(TabelaSaldos.Find(x => x.Conta == id), typeof(T));
        }

        public bool AtualizarSaldo<T>(T conta)
        {
            //recebe o objeto conta com o novo saldo e tenta remover o antigo da lista TabelaSaldos e acrescentar o novo
            try
            {
                ContasSaldo item = conta as ContasSaldo;
                TabelaSaldos.RemoveAll(x => x.Conta == item.Conta);
                TabelaSaldos.Add(conta as ContasSaldo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
        }

    }
}