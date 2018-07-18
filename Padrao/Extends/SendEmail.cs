using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Services.Functions;
using Domain.Entity;
using Data.Context;
using b2yweb_mvc4.Extends;
using System.Net;
using System.Net.Mail;



using System.Net.Mime;
using System.Configuration;

namespace b2yweb_mvc4.Extends
{
    public class SendEmail
    {
        private b2yweb_entities db = null;
        

        public void EnviarEmail(int cd_procedimento, string tipo)
        {
          

            db = new b2yweb_entities("oracle");
            ProcedimentoAdm procedimento = db.ProcedimentoAdm.Find(cd_procedimento);
            int cd_departamento = 0;
            

        


            //string obsOriginal = procedimento.OBS;
            string obsOriginal = String.Concat(procedimento.Clientes.CD_CADASTRO.ToString(), "-", procedimento.Clientes.RAZAO); ;
            string dtaUltimaInteracao = "";
            string msgUltimaInteracao = "";
            string userUltimaInteracao = "";
            string usuarioAbertura = procedimento.Usuario.NOME;


            if (tipo == "Edit")
            {
                if (db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == procedimento.CD_PROCEDIMENTO).Count() > 0)
                {
                    dtaUltimaInteracao = (from a in db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == procedimento.CD_PROCEDIMENTO).OrderByDescending(a => a.NUM_SEQ) select a.DTA_ENTRADA_DEP_NOVA).FirstOrDefault().ToString();
                    msgUltimaInteracao = (from a in db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == procedimento.CD_PROCEDIMENTO).OrderByDescending(a => a.NUM_SEQ) select a.OBS).FirstOrDefault().ToString();
                    userUltimaInteracao = (from a in db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == procedimento.CD_PROCEDIMENTO).OrderByDescending(a => a.NUM_SEQ) select a.Usuario.NOME).FirstOrDefault().ToString();
                    cd_departamento = (from a in db.pa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == procedimento.CD_PROCEDIMENTO).OrderByDescending(a => a.NUM_SEQ) select a.CD_DEPARTAMENTO_NOVA).FirstOrDefault();

                }
                else
                {
                    dtaUltimaInteracao = "";
                    userUltimaInteracao = "";
                    cd_departamento = 0;
                }
            }
            else
            {
                dtaUltimaInteracao = "";
                userUltimaInteracao = "";
                cd_departamento = 0;
            }



            if (cd_departamento == 0)
            {
                cd_departamento = procedimento.CD_DEPARTAMENTO;
            }

            if (cd_departamento == 0)
            {
              return;
            }

            string enviaemail = (from a in db.DEPARTAMENTOes.Where(a => a.CD_DEPARTAMENTO == cd_departamento) select a.ENVIA_EMAIL).FirstOrDefault() ;
            var depUsu = db.DepartamentoUsuario.Where(a => a.CD_DEPARTAMENTO == cd_departamento).ToList();

            if (enviaemail == "N")
            {
             return;
            }
            
            //var stateList = new List<int>() {2,3,4};
            //if ((tipo == "Edit") && (!procedimento.Situacao.ToString().Contains(stateList.ToString())))
            //{
            //    return;
            //}

            var msg = new MailMessage();
            string Para = "";
            string CC = "";
            int aux = 1;
            foreach (var var in depUsu)
            {
                
                if (aux == 1)
                {
                    msg.To.Add(new MailAddress(var.Usuario.EMAIL));
                }else
                {
                    msg.CC.Add(new MailAddress(var.Usuario.EMAIL));
                }

                aux += 1;
            }

            
            
            string DepResponsavel = procedimento.DEPARTAMENTO.DESC_DEPARTAMENTO;
            string dta_abertura = procedimento.DTA_ABERTURA.ToString();
            string nfFox = procedimento.NF_FOX.ToString();

            string situacao = procedimento.Situacao.DESCRICAO;
            string DescTipo = (from a in db.TP_PROCEDIMENTO.Where(a => a.CD_TIPO == procedimento.CD_TIPO) select a.DES_TIPO).FirstOrDefault();
            string subject = "[Chamado #" + cd_procedimento +"] " + DescTipo ;



            string email = ConfigurationManager.AppSettings["MailFrom"].ToString();
            string username = ConfigurationManager.AppSettings["MailUserName"].ToString();
            string password = ConfigurationManager.AppSettings["MailPwd"].ToString();
            var loginInfo = new NetworkCredential(username, password);
            var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SMTP"].ToString(), Int32.Parse(ConfigurationManager.AppSettings["Port"]));


           


            //string message = "<b> DADOS DO CHAMADO </b>";
            //string path = System.Web.HttpContext.Current.Server.MapPath("~/Images/LogoEmail.png"); // my logo is placed in images folder
            //Path.GetFileName(Server.MapPath("~/Images"), "LogoEmail.png"); 
            //get   

            //LinkedResource logo = new LinkedResource(path);
            //logo.ContentId = "companylogo";
            #region msgbody

            string message = " <div align='center'> " ;
            message += " <table class='MsoNormalTable' border='0' cellspacing='0' cellpadding='0' width='600' style='width:450.0pt;mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt: ";
            message += " 0cm 0cm 0cm 0cm'> ";
            message += "  <tbody><tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'> ";
            message += "   <td style='padding:0cm 0cm 0cm 0cm'> ";
            message += "   <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;; ";
            message += " mso-fareast-font-family:&quot;Times New Roman&quot;;color:#797979'><img id='_x0000_i1025' src='~/Images/LogoEmail.png'><o:p></o:p></span></p> ";
            message += "   </td>";
            message += " </tr> ";
            message += " <tr style='mso-yfti-irow:1;height:15.0pt'> ";
            message += " <td style='padding:0cm 0cm 0cm 0cm;height:15.0pt'></td> ";
            message += "  </tr>";
            message += " <tr style='mso-yfti-irow:2'>";
            message += "   <td style='padding:0cm 15.0pt 0cm 15.0pt'> ";
            message += "   <h3 style='margin:0cm;margin-bottom:.0001pt'><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;; ";

            if (tipo == "Edit")
            {
                message += "   mso-fareast-font-family:&quot;Times New Roman&quot;;color:#2A2A2A'>Nova interação no ";
            }
            
            if (tipo == "Create")
            {
                message += "   mso-fareast-font-family:&quot;Times New Roman&quot;;color:#2A2A2A'>Você recebeu uma solicitação para avaliar:  ";
            }
            
            message += "   procedimento : " + cd_procedimento +"<o:p></o:p></span></h3> ";
            message += "   <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;; ";
            message += "   mso-fareast-font-family:&quot;Times New Roman&quot;;color:#797979'><o:p>&nbsp;</o:p></span></p> ";
            message += "   <p style='margin:0cm;margin-bottom:.0001pt'><span style='font-size:10.5pt; ";
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Dados do procedimento<o:p></o:p></span></p> ";
            message += "   </td>";
            message += "  </tr> ";
            message += "  <tr style='mso-yfti-irow:3'>" ;
            message += " <td style='padding:7.5pt 7.5pt 0cm 7.5pt'>" ;
            message += "   <div style='border:solid #D7D7D7 1.0pt;mso-border-alt:solid #D7D7D7 .75pt;" ;
            message += "   padding:8.0pt 8.0pt 8.0pt 8.0pt'>" ;
             message += " <p style='margin:1.5pt;background:#F7F7F7'><span style='font-size:9.0pt;" ;
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Data Abertura: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + dta_abertura + "</span></strong><o:p></o:p></span></p>" ;
            message += "   <p style='margin:1.5pt;background:#F7F7F7'><span style='font-size:9.0pt;" ;
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Usuário: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + usuarioAbertura +"</span></strong><o:p></o:p></span></p>" ;
            message += "   <p style='margin:1.5pt;background:#F7F7F7'><span style='font-size:9.0pt;" ;
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Cliente: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + obsOriginal ;
            message += "   </span></strong><o:p></o:p></span></p>" ;
            message += "   <p style='margin:1.5pt;background:#F7F7F7'><span style='font-size:9.0pt;";
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>NF Foxlux: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + nfFox;
            message += "   </span></strong><o:p></o:p></span></p>";
            message += "   <p style='margin:1.5pt;background:#F7F7F7'><span style='font-size:9.0pt;" ;
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Departamento: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + DepResponsavel + "</span></strong><o:p></o:p></span></p>" ;
            message += "   <p style='margin:1.5pt;background:#F7F7F7'><span style='font-size:9.0pt;" ;
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>No. do procedimento: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + cd_procedimento + "</span></strong><o:p></o:p></span></p>" ;
            message += "   </div>" ;
            message += "   </td>" ;
            message += "  </tr>" ;
            message += "  <tr style='mso-yfti-irow:4'>" ;
            message += "   <td style='padding:0cm 15.0pt 0cm 15.0pt'>" ;
            message += "   <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;" ;
            message += "   mso-fareast-font-family:&quot;Times New Roman&quot;;color:#797979'><o:p>&nbsp;</o:p></span></p>" ;

            //message += "   <p style='margin:0cm;margin-bottom:.0001pt'><span style='font-size:10.5pt;" ;
            //if (tipo == "Edit")
            //{
            //    message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Última interação<o:p></o:p></span></p>";
            //}

            //if (tipo == "Create")
            //{
            //    message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Dados do Chamado<o:p></o:p></span></p>";

            //}



            //message += "   <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;" ;
            //message += "   mso-fareast-font-family:&quot;Times New Roman&quot;;color:#797979'><o:p>&nbsp;</o:p></span></p>" ;
            message += "   <p style='margin:1.5pt'><span style='font-size:9.0pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;" ;

            //if (tipo == "Edit")
            //{
            //    message += "   color:#797979'>Autor: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + userUltimaInteracao;
            //}

            //if (tipo == "Create")
            //{
            //    message += "   color:#797979'>Autor: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + usuarioAbertura;
            //}
            //message += " </span></strong><o:p></o:p></span></p>" ;


            //message += "   <p style='margin:1.5pt'><span style='font-size:9.0pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;" ;
            //if (tipo == "Edit")
            //{
            //    message += " color:#797979'>Data: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + dtaUltimaInteracao;
            //}

            //if (tipo == "Create")
            //{
            //    message += " color:#797979'>Data: <strong><span style='font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;'>" + dta_abertura;
            //}
            //  message += " </span></strong><o:p></o:p></span></p>" ;



//            message += "   <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;" ;
//            message += "   mso-fareast-font-family:&quot;Times New Roman&quot;;color:#797979'><br>" ;
//            message += "   <br style='mso-special-character:line-break'>" ;
//            message += "   <!--[if !supportLineBreakNewLine]--><br style='mso-special-character:line-break'>" ;
//            message += "   <!--[endif]--><o:p></o:p></span></p>" ;

            //message += "   <p style='margin:0cm;margin-bottom:.0001pt'><span style='font-size:9.0pt;" ;
            //message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'> Msg. <br>" ;
            //message += "   <br>" ;

            //if (tipo == "Edit")
            //{
            //    message += "   " + msgUltimaInteracao;
            //}
            //if (tipo == "Create")
            //{
            //    message += "   " + obsOriginal;

            //}
            //message += "   <br>" ;
            //message += "   Atenciosamente,<br>" ;
            //message += "   <br>" ;

            //message += "   " + ((Usuario)HttpContext.Current.Session["oUsuario"]).NOME + " <o:p></o:p></span></p>";

            message += "   <p class='MsoNormal' style='margin-bottom:12.0pt'><span style='font-size:9.0pt;" ;
            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:&quot;Times New Roman&quot;;" ;
            message += "   color:#797979'><o:p>&nbsp;</o:p></span></p>" ;
//            message += "   <p style='margin:0cm;margin-bottom:.0001pt'><span style='font-size:9.0pt;" ;
//            message += "   font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;color:#797979'>Para visualizar esse" ;
//            message += "   chamado, acesse o link: <br>" ;
//            message += "   <a href='http://centraldocliente.locaweb.com.br/tickets/19702140'><span style='color:#990000'>http://centraldocliente.locaweb.com.br/tickets/19702140</span></a><o:p></o:p></span></p>" ;
            message += "   </td>" ;
            message += "  </tr>" ;
            message += "  <tr style='mso-yfti-irow:5;height:30.0pt'>" ;
            message += "   <td style='padding:0cm 0cm 0cm 0cm;height:30.0pt'></td>" ;
            message += "  </tr>" ;
            message += "  <tr style='mso-yfti-irow:6;height:34.5pt'>" ;
            message += "   <td width='600' style='width:450.0pt;padding:0cm 0cm 0cm 0cm;height:34.5pt'>" ;
            /*message += "   <table class='MsoNormalTable' border='0' cellspacing='0' cellpadding='0' width='600' style='width:450.0pt;mso-cellspacing:0cm;background:#131313;mso-yfti-tbllook:" ;
            message += "    1184;mso-padding-alt:0cm 7.5pt 0cm 7.5pt;border-radius: 3px 3px 3px 3px'>" ;
            message += "    <tbody><tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>" ;
            message += "     <td style='padding:0cm 7.5pt 0cm 7.5pt'>" ;
            message += "     <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;" ;
            message += "     mso-fareast-font-family:&quot;Times New Roman&quot;;color:white;text-transform:uppercase'>Suporte" ;
            message += "     e Ajuda <img border='0' id='_x0000_i1026' src='https://centraldocliente.locaweb.com.br/assets/border-menu-support-footer-e0a15b2f50f910340fcb9842e80ec48f.png' style='margin-bottom:0in;margin-left:5px;margin-right:5px;margin-top:0in;" ;
            message += "     vertical-align:middle'><o:p></o:p></span></p>" ;
            message += "     </td>" ;
            message += "     <td style='padding:0cm 7.5pt 0cm 7.5pt'>" ;
            message += "     <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;" ;
            message += "     mso-fareast-font-family:&quot;Times New Roman&quot;;color:white'><img border='0' id='_x0000_i1027' src='https://centraldocliente.locaweb.com.br/assets/arrow-right-red-menu-9b5b6e2b0f40f2f19e8e05d2dfd7c251.png' style='vertical-align:middle'><a href='https://centraldocliente.locaweb.com.br/treatment' target='_blank' title='Atendimento'><span style='color:white;text-decoration:none;text-underline:" ;
            message += "     none'>Atendimento </span></a><img border='0' id='_x0000_i1028' src='https://centraldocliente.locaweb.com.br/assets/border-menu-support-footer-e0a15b2f50f910340fcb9842e80ec48f.png' style='margin-bottom:0in;margin-left:5px;margin-right:5px;margin-top:0in;" ;
            message += "     vertical-align:middle'><o:p></o:p></span></p>" ;
            message += "     </td>" ;
            message += "     <td style='padding:0cm 7.5pt 0cm 7.5pt'>" ;
            message += "     <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;" ;
            message += "     mso-fareast-font-family:&quot;Times New Roman&quot;;color:white'><img border='0' id='_x0000_i1029' src='https://centraldocliente.locaweb.com.br/assets/arrow-right-red-menu-9b5b6e2b0f40f2f19e8e05d2dfd7c251.png' style='vertical-align:middle'><a href='https://centraldocliente.locaweb.com.br/tickets' target='_blank' title='Meus Chamados'><span style='color:white;text-decoration:none;" ;
            message += "     text-underline:none'>Meus Chamados </span></a><img border='0' id='_x0000_i1030' src='https://centraldocliente.locaweb.com.br/assets/border-menu-support-footer-e0a15b2f50f910340fcb9842e80ec48f.png' style='margin-bottom:0in;margin-left:5px;margin-right:5px;margin-top:0in;" ;
            message += "     vertical-align:middle'><o:p></o:p></span></p>" ;
            message += "     </td>" ;
            message += "     <td style='padding:0cm 7.5pt 0cm 7.5pt'>" ;
            message += "     <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;" ;
            message += "     mso-fareast-font-family:&quot;Times New Roman&quot;;color:white'><img border='0' id='_x0000_i1031' src='https://centraldocliente.locaweb.com.br/assets/arrow-right-red-menu-9b5b6e2b0f40f2f19e8e05d2dfd7c251.png' style='vertical-align:middle'><a href='http://wiki.locaweb.com.br' target='_blank' title='Central de Ajuda (wiki)'><span style='color:white;" ;
            message += "     text-decoration:none;text-underline:none'>Central de Ajuda (wiki) </span></a><img border='0' id='_x0000_i1032' src='https://centraldocliente.locaweb.com.br/assets/border-menu-support-footer-e0a15b2f50f910340fcb9842e80ec48f.png' style='margin-bottom:0in;margin-left:5px;margin-right:5px;margin-top:0in;" ;
            message += "     vertical-align:middle'><o:p></o:p></span></p>" ;
            message += "     </td>" ;
            message += "     <td style='padding:0cm 7.5pt 0cm 7.5pt'>" ;
            message += "     <p class='MsoNormal'><span style='font-size:9.0pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;" ;
            message += "     mso-fareast-font-family:&quot;Times New Roman&quot;;color:white'><img border='0' id='_x0000_i1033' src='https://centraldocliente.locaweb.com.br/assets/arrow-right-red-menu-9b5b6e2b0f40f2f19e8e05d2dfd7c251.png' style='vertical-align:middle'><a href='http://statusblog.locaweb.com.br/' target='_blank'><span style='color:white;text-decoration:none;text-underline:" ;
            message += "     none'>Status blog </span></a><o:p></o:p></span></p>" ;
            message += "     </td>" ;
            message += "    </tr>" ;
            message += "   </tbody></table>" ; */
            message += "   </td>" ;
            message += "  </tr>" ;
            message += "  <tr style='mso-yfti-irow:7;mso-yfti-lastrow:yes'>" ;
            message += "   <td style='padding:11.25pt 0cm 0cm 0cm'>" ;
            message += "   <p class='MsoNormal' align='center' style='text-align:center'><span style='font-size:8.5pt;font-family:&quot;Verdana&quot;,&quot;sans-serif&quot;;mso-fareast-font-family:" ;
            message += "   &quot;Times New Roman&quot;;color:#666666'>Se você possui filtro antispam habilitado em" ;
            message += "   sua caixa postal, autorize o e-mail<br>" ;
            message += "   <a href='" + email + "'><span style='color:#990000'>" + email + "</span></a>";
            message += "   para continuar recebendo nossas mensagens <o:p></o:p></span></p>" ;
            message += "   </td>" ;
            message += "  </tr>" ;
            message += " </tbody></table>" ;
            message += " </div>";

            #endregion
            //message += "<img src='Images\\LogoEmail.png'> ";

            //AlternateView av1 = AlternateView.CreateAlternateViewFromString(
      //"<html><body><img src=cid:companylogo/>" + 
      //"<br></body></html>" + message, 
      //null, MediaTypeNames.Text.Html);

//now add the AlternateView
//av1.LinkedResources.Add(logo);

//now append it to the body of the mail
//msg.AlternateViews.Add(av1);
         

        msg.From = new MailAddress(email);
        //msg.To.Add(new MailAddress(Para));
        
        msg.Subject = subject;
        msg.Body = message;
        msg.IsBodyHtml = true;

        smtpClient.EnableSsl = false;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = loginInfo;
        smtpClient.Send(msg);
        
        }
    }
}
