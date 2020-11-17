using Inspinia_MVC5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class SendMailerController : Controller
    {
        private InspiniaContext db = new InspiniaContext();

         // GET: /SendMailer/Index  
        public ActionResult Index()
        {
            TempData["Mensagem"] = TempData["Mensagem"];
            return View();
        }

        // POST: /SendMailer/ Index 
        [HttpPost]
        public ViewResult Index(Inspinia_MVC5.Models.MailModel _objModelMail)
        {

            //primeira etapa é enviar o email para a Equipe SAD
            _objModelMail.To = "sad.sistemaslegados@gmail.com";
            if (ModelState.IsValid)
            {
                //Instância classe email
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.To);
                mail.Subject = "Recebimento de contato ";
                string Body = "<p>Email de contato: " + _objModelMail.From + "</p>" +
                    "<p>Título: " + _objModelMail.Subject + "</p>" +
                    "<p>Mensagem:</p>" + "<p>" + _objModelMail.Body + "</p>";
                mail.Body = Body;
                mail.IsBodyHtml = true;

                //Instância smtp do servidor, neste caso o gmail.
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("sad.sistemaslegados", "legados2018");// Login e senha do e-mail.
                smtp.EnableSsl = true;
                smtp.Send(mail);

                //----------------------------------------
                //segunda etapa é enviar o email para o usuário
                //------------------------------------------------

                //Instância classe email
                MailMessage mail2 = new MailMessage();
                mail2.To.Add(_objModelMail.From);
                mail2.From = new MailAddress(_objModelMail.To);
                mail2.Subject = "SAD Sistemas Legados - Confirmação de contato";
                _objModelMail.Body = "<p>Olá,</p>" +
                    "<p>Obrigado por fazer contato conosco!</p>" +
                    "<p>Vamos analisar sua mensagem e retornaremos o contato o mais breve possível.</p>" +
                    "<p>Atenciosamente,</p>" +
                    "<p>Equipe SAD Sistemas Legados</p>";
                mail2.Body = _objModelMail.Body;
                mail2.IsBodyHtml = true;

                //Instância smtp do servidor, neste caso o gmail.
                SmtpClient smtp2 = new SmtpClient();
                smtp2.Host = "smtp.gmail.com";
                smtp2.Port = 587;
                smtp2.UseDefaultCredentials = false;
                smtp2.Credentials = new System.Net.NetworkCredential
                ("sad.sistemaslegados", "legados2018");// Login e senha do e-mail.
                smtp2.EnableSsl = true;
                smtp2.Send(mail2);

                TempData["Mensagem"] = "Email enviado com sucesso e você receberá uma confirmação do envio.";
                return View("Index");
            }
            else
            {
                TempData["Mensagem"] = "Não foi possível enviar o contato, por favor tente mais tarde.";
                return View();
            }
        }

        // GET: /SendMailer/Index  
        public ActionResult Contato()
        {
            TempData["Mensagem"] = TempData["Mensagem"];
            return View();
        }

        // POST: /SendMailer/ Index 
        [HttpPost]
        public ViewResult Contato(Inspinia_MVC5.Models.MailModel _objModelMail)
        {
            Usuario user = Session["Usuario"] as Usuario;//busca usuario logado
            
            //primeira etapa é enviar o email para a Equipe SAD
            _objModelMail.From = user.EMAIL;
            _objModelMail.To = "sad.sistemaslegados@gmail.com";
            if (ModelState.IsValid)
            {
                //Instância classe email
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.To);
                mail.Subject ="Recebimento de contato ";
                string Body ="Nome: "+user.NOME + "</p>" + "<p>Email de contato: " +user.EMAIL + "</p>" +
                    "<p>Título: " + _objModelMail.Subject + "</p>" +
                    "<p>Mensagem:</p>" + "<p>" + _objModelMail.Body + "</p>";
                mail.Body = Body;
                mail.IsBodyHtml = true;

                //Instância smtp do servidor, neste caso o gmail.
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("sad.sistemaslegados", "legados2018");// Login e senha do e-mail.
                smtp.EnableSsl = true;
                smtp.Send(mail);

                //----------------------------------------
                //segunda etapa é enviar o email para o usuário
                //------------------------------------------------

                //Instância classe email
                MailMessage mail2 = new MailMessage();
                mail2.To.Add(_objModelMail.From);
                mail2.From = new MailAddress(_objModelMail.To);
                mail2.Subject = "SAD Sistemas Legados - Confirmação de contato";
                _objModelMail.Body = "<p>Olá "+user.NOME + ",</p>" +
                    "<p>Obrigado por fazer contato conosco!</p>" +
                    "<p>Vamos analisar sua mensagem e retornaremos o contato o mais breve possível.</p>" +
                    "<p>Atenciosamente,</p>" +
                    "<p>Equipe SAD Sistemas Legados</p>";
                mail2.Body = _objModelMail.Body;
                mail2.IsBodyHtml = true;

                //Instância smtp do servidor, neste caso o gmail.
                SmtpClient smtp2 = new SmtpClient();
                smtp2.Host = "smtp.gmail.com";
                smtp2.Port = 587;
                smtp2.UseDefaultCredentials = false;
                smtp2.Credentials = new System.Net.NetworkCredential
                ("sad.sistemaslegados", "legados2018");// Login e senha do e-mail.
                smtp2.EnableSsl = true;
                smtp2.Send(mail2);

                TempData["Mensagem"] = "Email enviado com sucesso e você receberá uma confirmação do envio.";
                return View("Contato");
            }
            else
            {
                TempData["Mensagem"] = "Não foi possível enviar o contato, por favor tente mais tarde.";
                return View();
            }
        }

        // GET: /SendMailer/Esqueceu senha  
        public ActionResult ForgotPassword()
        {
            TempData["Mensagem"] = TempData["Mensagem"];
            return View();
        }

        // POST: /SendMailer/Esqueceu senha  
        [HttpPost]
        public ViewResult ForgotPassword(Inspinia_MVC5.Models.MailModel _objModelMail)
        {
            Usuario user = db.Usuarios.Where(x=>x.EMAIL.Equals(_objModelMail.To)).FirstOrDefault();
            if (user != null)
            {
                if (user.PAPEL.Equals("Desativado"))//verifica se a conta está desativada
                {
                    user.PAPEL = "User";//ativa a conta novamente 
                }
                using (MD5 md5Hash = MD5.Create())
                {
                    user.SENHA = GetMd5Hash(md5Hash, "inicial123");//reseta a senha
                    user.CONFSENHA = GetMd5Hash(md5Hash, "inicial123");//reseta a confirmação da senha
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                _objModelMail.From = "sad.sistemaslegados@gmail.com";
                _objModelMail.Subject = "SAD Sistemas Legados - Reset de senha";
                _objModelMail.Body = "<p>Olá " +user.NOME+ ",</p>" +
                    "<p>Recebemos sua solicitação de reset da sua senha de acesso ao nosso sistema.</p>" +
                    "<p>Sua nova senha é: inicial123</p>" +
                    "<p>Faça o acesso com a nova senha e logo após altere para sua segurança.</p>" +
                    "<p>Atenciosamente,</p>" +
                    "<p>Equipe SAD Sistemas Legados</p>";

                if (ModelState.IsValid)
                {
                    //Instância classe email
                    MailMessage mail = new MailMessage();
                    mail.To.Add(_objModelMail.To);
                    mail.From = new MailAddress(_objModelMail.From);
                    mail.Subject = _objModelMail.Subject;
                    string Body = _objModelMail.Body;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;

                    //Instância smtp do servidor, neste caso o gmail.
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                    ("sad.sistemaslegados", "legados2018");// Login e senha do e-mail.
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    TempData["Mensagem"] = "Nova senha enviada por email.";
                    return View("ForgotPassword");
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possível enviar a nova senha para o email.";
                    return View();
                }
            }
            else
            {
                TempData["Mensagem"] = "Email não encontrado no nosso banco de dados.";
                return View(_objModelMail);
            }
            
        }

        //função para criptografar senha
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}