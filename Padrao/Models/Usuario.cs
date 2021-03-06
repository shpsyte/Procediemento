//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace b2yweb_mvc4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Usuario
    {
        public short cd_usuario { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string nome { get; set; }
        [Required(ErrorMessage="Campo Obrigatorio")]
        public string login { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string senha { get; set; }
        public short cd_gusuario { get; set; }
        public int cd_empresa { get; set; }
        public int cd_ccusto { get; set; }
        public string situacao { get; set; }
        public string fone { get; set; }
        public string celular { get; set; }
        public int ramal { get; set; }
        public short cd_cidade { get; set; }
        public string altera_formulario { get; set; }
        public string altera_senha { get; set; }
        public string msg_connect { get; set; }
        public string msg_conec_desc { get; set; }
        public Nullable<short> ddd_celular { get; set; }
        public string envio_mail_market { get; set; }
        public string aviso_corporativo { get; set; }
        public string cep { get; set; }
        public string endereco { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string envia_sms { get; set; }
        public short cd_operadora { get; set; }
        public string email_particular { get; set; }
        public Nullable<short> ddd_fone_com { get; set; }
        public string fone_com { get; set; }
        public string per_mensagem_dia { get; set; }
        public Nullable<System.DateTime> dt_cadastro { get; set; }
        public Nullable<System.DateTime> dt_nascimento { get; set; }
        public string pub_lista_con_des { get; set; }
        public string permite_enviar_sms { get; set; }
        public string conf_tela_meu { get; set; }
        public string permite_relatorios { get; set; }
        public string permite_alt_config { get; set; }
        public string menu_acima { get; set; }
        public string ativa_cripto { get; set; }
        public string usa_btn_acima { get; set; }
        public string PDV { get; set; }
        public string PortaPDV { get; set; }
        public string AcessoWeb { get; set; }
        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Apelido { get; set; }
    
        public virtual GUsuario GUsuario { get; set; }
    }
}
