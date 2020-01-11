//------------------------------------------------------------------------------
// <gerado automaticamente>
//     Esse código foi gerado por uma ferramenta.
//     //
//     As alterações no arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </gerado automaticamente>
//------------------------------------------------------------------------------

namespace WSCorreios
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WSCorreios.CalcPrecoPrazoWSSoap")]
    public interface CalcPrecoPrazoWSSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrecoPrazo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoPrazoAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrecoPrazoData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoPrazoDataAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento, string sDtCalculo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrecoPrazoRestricao", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoPrazoRestricaoAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento, string sDtCalculo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPreco", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrecoData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoDataAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento, string sDtCalculo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrazo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrazoAsync(string nCdServico, string sCepOrigem, string sCepDestino);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrazoData", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrazoDataAsync(string nCdServico, string sCepOrigem, string sCepDestino, string sDtCalculo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrazoRestricao", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrazoRestricaoAsync(string nCdServico, string sCepOrigem, string sCepDestino, string sDtCalculo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcPrecoFAC", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoFACAsync(string nCdServico, string nVlPeso, string strDataCalculo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CalcDataMaxima", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultadoObjeto> CalcDataMaximaAsync(string codigoObjeto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ListaServicos", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultadoServicos> ListaServicosAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ListaServicosSTAR", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultadoServicos> ListaServicosSTARAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/VerificaModal", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.cResultadoModal> VerificaModalAsync(string nCdServico, string sCepOrigem, string sCepDestino);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getVersao", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<WSCorreios.versao> getVersaoAsync();
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cResultado
    {
        
        private cServico[] servicosField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public cServico[] Servicos
        {
            get
            {
                return this.servicosField;
            }
            set
            {
                this.servicosField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cServico
    {
        
        private int codigoField;
        
        private string valorField;
        
        private string prazoEntregaField;
        
        private string valorMaoPropriaField;
        
        private string valorAvisoRecebimentoField;
        
        private string valorValorDeclaradoField;
        
        private string entregaDomiciliarField;
        
        private string entregaSabadoField;
        
        private string erroField;
        
        private string msgErroField;
        
        private string valorSemAdicionaisField;
        
        private string obsFimField;
        
        private string dataMaxEntregaField;
        
        private string horaMaxEntregaField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int Codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string PrazoEntrega
        {
            get
            {
                return this.prazoEntregaField;
            }
            set
            {
                this.prazoEntregaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string ValorMaoPropria
        {
            get
            {
                return this.valorMaoPropriaField;
            }
            set
            {
                this.valorMaoPropriaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string ValorAvisoRecebimento
        {
            get
            {
                return this.valorAvisoRecebimentoField;
            }
            set
            {
                this.valorAvisoRecebimentoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string ValorValorDeclarado
        {
            get
            {
                return this.valorValorDeclaradoField;
            }
            set
            {
                this.valorValorDeclaradoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string EntregaDomiciliar
        {
            get
            {
                return this.entregaDomiciliarField;
            }
            set
            {
                this.entregaDomiciliarField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string EntregaSabado
        {
            get
            {
                return this.entregaSabadoField;
            }
            set
            {
                this.entregaSabadoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string Erro
        {
            get
            {
                return this.erroField;
            }
            set
            {
                this.erroField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string MsgErro
        {
            get
            {
                return this.msgErroField;
            }
            set
            {
                this.msgErroField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string ValorSemAdicionais
        {
            get
            {
                return this.valorSemAdicionaisField;
            }
            set
            {
                this.valorSemAdicionaisField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string obsFim
        {
            get
            {
                return this.obsFimField;
            }
            set
            {
                this.obsFimField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string DataMaxEntrega
        {
            get
            {
                return this.dataMaxEntregaField;
            }
            set
            {
                this.dataMaxEntregaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string HoraMaxEntrega
        {
            get
            {
                return this.horaMaxEntregaField;
            }
            set
            {
                this.horaMaxEntregaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class versao
    {
        
        private string versaoAtualField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string versaoAtual
        {
            get
            {
                return this.versaoAtualField;
            }
            set
            {
                this.versaoAtualField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cModal
    {
        
        private string codigoField;
        
        private string modalField;
        
        private string obsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string modal
        {
            get
            {
                return this.modalField;
            }
            set
            {
                this.modalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string obs
        {
            get
            {
                return this.obsField;
            }
            set
            {
                this.obsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cResultadoModal
    {
        
        private cModal[] servicosModalField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public cModal[] ServicosModal
        {
            get
            {
                return this.servicosModalField;
            }
            set
            {
                this.servicosModalField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cServicosCalculo
    {
        
        private string codigoField;
        
        private string descricaoField;
        
        private string calcula_precoField;
        
        private string calcula_prazoField;
        
        private string erroField;
        
        private string msgErroField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string descricao
        {
            get
            {
                return this.descricaoField;
            }
            set
            {
                this.descricaoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string calcula_preco
        {
            get
            {
                return this.calcula_precoField;
            }
            set
            {
                this.calcula_precoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string calcula_prazo
        {
            get
            {
                return this.calcula_prazoField;
            }
            set
            {
                this.calcula_prazoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string erro
        {
            get
            {
                return this.erroField;
            }
            set
            {
                this.erroField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string msgErro
        {
            get
            {
                return this.msgErroField;
            }
            set
            {
                this.msgErroField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cResultadoServicos
    {
        
        private cServicosCalculo[] servicosCalculoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public cServicosCalculo[] ServicosCalculo
        {
            get
            {
                return this.servicosCalculoField;
            }
            set
            {
                this.servicosCalculoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cObjeto
    {
        
        private string codigoField;
        
        private string servicoField;
        
        private string cepOrigemField;
        
        private string cepDestinoField;
        
        private int prazoEntregaField;
        
        private string dataPostagemField;
        
        private string dataPostagemCalculoField;
        
        private string dataMaxEntregaField;
        
        private string postagemDHField;
        
        private string dataUltimoEventoField;
        
        private string codigoUltimoEventoField;
        
        private string tipoUltimoEventoField;
        
        private string descricaoUltimoEventoField;
        
        private string erroField;
        
        private string msgErroField;
        
        private string nuTicketField;
        
        private string formaPagamentoField;
        
        private string valorPagamentoField;
        
        private string nuContratoField;
        
        private string nuCartaoPostagemField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string servico
        {
            get
            {
                return this.servicoField;
            }
            set
            {
                this.servicoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string cepOrigem
        {
            get
            {
                return this.cepOrigemField;
            }
            set
            {
                this.cepOrigemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string cepDestino
        {
            get
            {
                return this.cepDestinoField;
            }
            set
            {
                this.cepDestinoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int prazoEntrega
        {
            get
            {
                return this.prazoEntregaField;
            }
            set
            {
                this.prazoEntregaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string dataPostagem
        {
            get
            {
                return this.dataPostagemField;
            }
            set
            {
                this.dataPostagemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string dataPostagemCalculo
        {
            get
            {
                return this.dataPostagemCalculoField;
            }
            set
            {
                this.dataPostagemCalculoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string dataMaxEntrega
        {
            get
            {
                return this.dataMaxEntregaField;
            }
            set
            {
                this.dataMaxEntregaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string postagemDH
        {
            get
            {
                return this.postagemDHField;
            }
            set
            {
                this.postagemDHField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string dataUltimoEvento
        {
            get
            {
                return this.dataUltimoEventoField;
            }
            set
            {
                this.dataUltimoEventoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string codigoUltimoEvento
        {
            get
            {
                return this.codigoUltimoEventoField;
            }
            set
            {
                this.codigoUltimoEventoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string tipoUltimoEvento
        {
            get
            {
                return this.tipoUltimoEventoField;
            }
            set
            {
                this.tipoUltimoEventoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string descricaoUltimoEvento
        {
            get
            {
                return this.descricaoUltimoEventoField;
            }
            set
            {
                this.descricaoUltimoEventoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string erro
        {
            get
            {
                return this.erroField;
            }
            set
            {
                this.erroField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public string msgErro
        {
            get
            {
                return this.msgErroField;
            }
            set
            {
                this.msgErroField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=15)]
        public string nuTicket
        {
            get
            {
                return this.nuTicketField;
            }
            set
            {
                this.nuTicketField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=16)]
        public string formaPagamento
        {
            get
            {
                return this.formaPagamentoField;
            }
            set
            {
                this.formaPagamentoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=17)]
        public string valorPagamento
        {
            get
            {
                return this.valorPagamentoField;
            }
            set
            {
                this.valorPagamentoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=18)]
        public string nuContrato
        {
            get
            {
                return this.nuContratoField;
            }
            set
            {
                this.nuContratoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=19)]
        public string nuCartaoPostagem
        {
            get
            {
                return this.nuCartaoPostagemField;
            }
            set
            {
                this.nuCartaoPostagemField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class cResultadoObjeto
    {
        
        private cObjeto[] objetosField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public cObjeto[] Objetos
        {
            get
            {
                return this.objetosField;
            }
            set
            {
                this.objetosField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface CalcPrecoPrazoWSSoapChannel : WSCorreios.CalcPrecoPrazoWSSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class CalcPrecoPrazoWSSoapClient : System.ServiceModel.ClientBase<WSCorreios.CalcPrecoPrazoWSSoap>, WSCorreios.CalcPrecoPrazoWSSoap
    {
        
    /// <summary>
    /// Implemente este método parcial para configurar o ponto de extremidade de serviço.
    /// </summary>
    /// <param name="serviceEndpoint">O ponto de extremidade a ser configurado</param>
    /// <param name="clientCredentials">As credenciais do cliente</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public CalcPrecoPrazoWSSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(CalcPrecoPrazoWSSoapClient.GetBindingForEndpoint(endpointConfiguration), CalcPrecoPrazoWSSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalcPrecoPrazoWSSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(CalcPrecoPrazoWSSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalcPrecoPrazoWSSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(CalcPrecoPrazoWSSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public CalcPrecoPrazoWSSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoPrazoAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento)
        {
            return base.Channel.CalcPrecoPrazoAsync(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoPrazoDataAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento, string sDtCalculo)
        {
            return base.Channel.CalcPrecoPrazoDataAsync(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento, sDtCalculo);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoPrazoRestricaoAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento, string sDtCalculo)
        {
            return base.Channel.CalcPrecoPrazoRestricaoAsync(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento, sDtCalculo);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento)
        {
            return base.Channel.CalcPrecoAsync(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoDataAsync(string nCdEmpresa, string sDsSenha, string nCdServico, string sCepOrigem, string sCepDestino, string nVlPeso, int nCdFormato, decimal nVlComprimento, decimal nVlAltura, decimal nVlLargura, decimal nVlDiametro, string sCdMaoPropria, decimal nVlValorDeclarado, string sCdAvisoRecebimento, string sDtCalculo)
        {
            return base.Channel.CalcPrecoDataAsync(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento, sDtCalculo);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrazoAsync(string nCdServico, string sCepOrigem, string sCepDestino)
        {
            return base.Channel.CalcPrazoAsync(nCdServico, sCepOrigem, sCepDestino);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrazoDataAsync(string nCdServico, string sCepOrigem, string sCepDestino, string sDtCalculo)
        {
            return base.Channel.CalcPrazoDataAsync(nCdServico, sCepOrigem, sCepDestino, sDtCalculo);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrazoRestricaoAsync(string nCdServico, string sCepOrigem, string sCepDestino, string sDtCalculo)
        {
            return base.Channel.CalcPrazoRestricaoAsync(nCdServico, sCepOrigem, sCepDestino, sDtCalculo);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultado> CalcPrecoFACAsync(string nCdServico, string nVlPeso, string strDataCalculo)
        {
            return base.Channel.CalcPrecoFACAsync(nCdServico, nVlPeso, strDataCalculo);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultadoObjeto> CalcDataMaximaAsync(string codigoObjeto)
        {
            return base.Channel.CalcDataMaximaAsync(codigoObjeto);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultadoServicos> ListaServicosAsync()
        {
            return base.Channel.ListaServicosAsync();
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultadoServicos> ListaServicosSTARAsync()
        {
            return base.Channel.ListaServicosSTARAsync();
        }
        
        public System.Threading.Tasks.Task<WSCorreios.cResultadoModal> VerificaModalAsync(string nCdServico, string sCepOrigem, string sCepDestino)
        {
            return base.Channel.VerificaModalAsync(nCdServico, sCepOrigem, sCepDestino);
        }
        
        public System.Threading.Tasks.Task<WSCorreios.versao> getVersaoAsync()
        {
            return base.Channel.getVersaoAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.CalcPrecoPrazoWSSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.CalcPrecoPrazoWSSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Não foi possível encontrar o ponto de extremidade com o nome \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.CalcPrecoPrazoWSSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://ws.correios.com.br/calculador/CalcPrecoPrazo.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.CalcPrecoPrazoWSSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://ws.correios.com.br/calculador/CalcPrecoPrazo.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Não foi possível encontrar o ponto de extremidade com o nome \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            CalcPrecoPrazoWSSoap,
            
            CalcPrecoPrazoWSSoap12,
        }
    }
}
