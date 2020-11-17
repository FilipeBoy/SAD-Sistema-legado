using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Inspinia_MVC5.Models;


namespace Inspinia_MVC5.Controllers
{

    public class UsuariosController : Controller
    {
        private InspiniaContext db = new InspiniaContext();

        // GET: /Usuarios/Index
        public ActionResult Index()
        {
            return View(db.Usuarios.Where(x => x.PAPEL != "Admin").ToList());
        }

        // GET: /Usuarios/Details/5
        public ActionResult Details()
        {
            Usuario user = Session["Usuario"] as Usuario;
            if (user != null)
            {
                Usuario usuario = db.Usuarios.Find(user.ID_USUARIO);
                ViewData["Mensagem"] = TempData["Mensagem"];
                if (Session["aviso"] != null)
                {
                    ViewData["Mensagem"] = "Sua senha foi resetada, para sua segurança, cadastre uma nova senha!";
                }
                return View(usuario);
            }
            else
            {
                TempData["Mensagem"] = "Usuário não encontrado";
                return View();
            }
        }

        // GET: /Usuarios/Create
        public ActionResult CreateFree()
        {
            var user = db.Usuarios.Where(x => x.PAPEL.Equals("Admin")).FirstOrDefault();//verifica se há um administrador cadastrado
            if (user != null)
            {
                TempData["Admin"] = "sim";
                return View();
            }

            return RedirectToAction("../Usuarios/CreatePro");
        }

        // POST: /Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFree([Bind(Include = "ID_USUARIO,NOME,SOBRENOME,EMAIL,SENHA,CONFSENHA,PAPEL,TIPO_CONTA,DATA")] Usuario usuario)
        {
            if (usuario.SENHA != usuario.CONFSENHA)
            {
                ViewData["Mensagem"] = "Senha diferente de Confirmação de senha";
                return View(usuario);
            }
            if (TempData["Admin"] != null)
            {
                var flag = TempData["Admin"].ToString(); ;
                if (flag.Equals("nao"))
                {
                    usuario.PAPEL = "Admin";
                    usuario.TIPO_CONTA = "PRO";
                    usuario.DATA = DateTime.Today.Date;
                }
                else
                {
                    usuario.PAPEL = "User";
                    usuario.TIPO_CONTA = "GRATUITA";
                    usuario.DATA = DateTime.Today.Date;
                }
            }
            if (ModelState.IsValid)
            {
                var user = db.Usuarios.Where(x => x.EMAIL.Equals(usuario.EMAIL)).FirstOrDefault();//verifica se há um administrador cadastrado
                if (user != null)
                {
                    ViewData["Mensagem"] = "Email já cadastrado!";
                    return View();
                }
                using (MD5 md5Hash = MD5.Create())
                {
                    usuario.SENHA = GetMd5Hash(md5Hash, usuario.SENHA);
                    usuario.CONFSENHA = GetMd5Hash(md5Hash, usuario.CONFSENHA);
                }
                //tenta salvar o cadastro do usuario
                try
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = "Erro ao cadastrar: " + e;
                    return View(usuario);
                }
                
                Session["Nome"] = usuario.NOME;
                Session["Papel"] = usuario.PAPEL;
                Session["Conta"] = usuario.TIPO_CONTA;
                Session["Usuario"] = usuario;
                Session["sigla"] = usuario.NOME.Substring(0, 1) + usuario.SOBRENOME.Substring(0, 1);

                return RedirectToAction("../Home/Inicio");
            }
            ViewData["Mensagem"] = "Usuário inválido";
            return View(usuario);
        }

        // GET: /Usuarios/Create
        public ActionResult CreatePro()
        {
            Usuario user = db.Usuarios.Where(x => x.PAPEL.Equals("Admin")).FirstOrDefault();
            if (user != null)
            {
                TempData["Admin"] = "sim";
            }
            else
            {
                ViewData["Mensagem"] = "Olá administrador!";
                TempData["Admin"] = "nao";
            }
            return View();
        }

        // POST: /Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePro([Bind(Include = "ID_USUARIO,NOME,SOBRENOME,EMAIL,SENHA,CONFSENHA,PAPEL,TIPO_CONTA,DATA")] Usuario usuario)
        {
            if (usuario.SENHA != usuario.CONFSENHA)
            {
                ViewData["Mensagem"] = "Senha diferente de Confirmação de senha";
                return View(usuario);
            }
            if (TempData["Admin"] != null)
            {
                var flag = TempData["Admin"].ToString();
                if (flag.Equals("nao"))
                {
                    usuario.PAPEL = "Admin";
                    usuario.TIPO_CONTA = "PRO";
                    usuario.DATA = DateTime.Today.Date;
                }
                else
                {
                    usuario.PAPEL = "User";
                    usuario.TIPO_CONTA = "PRO";
                    usuario.DATA = DateTime.Today.Date;
                }
            }
            if (ModelState.IsValid)
            {
                var user = db.Usuarios.Where(x => x.EMAIL.Equals(usuario.EMAIL)).FirstOrDefault();//verifica se há um administrador cadastrado
                if (user != null)
                {
                    ViewData["Mensagem"] = "Email já cadastrado!";
                    return View();
                }
                using (MD5 md5Hash = MD5.Create())
                {
                    usuario.SENHA = GetMd5Hash(md5Hash, usuario.SENHA);
                    usuario.CONFSENHA = GetMd5Hash(md5Hash, usuario.CONFSENHA);
                }
                //tenta salvar o cadastro do usuario
                try
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = "Erro ao cadastrar: " + e;
                    return View(usuario);
                }
                Session["Nome"] = usuario.NOME;//abre sessão com o nome
                Session["Papel"] = usuario.PAPEL;//abre sessão com o papel do usuario
                Session["Conta"] = usuario.TIPO_CONTA;//abre sessão com o tipo de conta
                Session["Usuario"] = usuario;////abre sessão com o usuario
                Session["sigla"] = usuario.NOME.Substring(0, 1) + usuario.SOBRENOME.Substring(0, 1);
                return RedirectToAction("../Home/Inicio");
            }
            ViewData["Mensagem"] = "Usuário inválido";
            return View(usuario);
        }

        // GET: /Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            TempData["senha_atual"] = usuario.SENHA;
            TempData["conta"] = usuario.TIPO_CONTA;
            if (usuario.TIPO_CONTA.Equals("PRO"))
            {
                ViewBag.TIPO_CONTA = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = "PRO",Value = "PRO"},
                    new SelectListItem { Selected = false, Text = "GRATUITA",Value = "GRATUITA"},
                }, "Value", "Text");
            }
            else
            {
                ViewBag.TIPO_CONTA = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = "GRATUITO",Value = "GRATUITA"},
                    new SelectListItem { Selected = false, Text = "PRO",Value = "PRO"},
                }, "Value", "Text");
            }
            TempData["Usuario"] = usuario;
            return View(usuario);
        }

        // POST: /Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_USUARIO,NOME,SOBRENOME,EMAIL,SENHA,CONFSENHA,PAPEL,TIPO_CONTA,DATA")] Usuario usuario)
        {
            if (usuario.SENHA.Equals(usuario.CONFSENHA))
            {
                string senha_antiga = TempData["senha_atual"].ToString(); //recupera a senha armazenada
                if (usuario.SENHA != senha_antiga)//confere se a senha recebida é diferente da armazenada
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        if (VerifyMd5Hash(md5Hash, "inicial123", senha_antiga))////confere se a senha armazenada foi resetada
                        {
                            Session["aviso"] = null;//retira o aviso
                        }
                        //se a senha recebida for diferente da já armazenada, a aplicação altera
                        usuario.SENHA = GetMd5Hash(md5Hash, usuario.SENHA);
                        usuario.CONFSENHA = GetMd5Hash(md5Hash, usuario.CONFSENHA);

                    }
                }
                if (usuario.TIPO_CONTA != TempData["conta"].ToString())
                {
                    Session["Conta"] = usuario.TIPO_CONTA;
                }

                if (ModelState.IsValid)
                {
                    //tenta alterar o cadastro do usuario
                    try
                    {
                        db.Entry(usuario).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        ViewData["Message"] = "Erro ao cadastrar: " + e;
                        return View(usuario);
                    }
                    
                    TempData["Mensagem"] = "Alterado com sucesso.";
                    return RedirectToAction("../Usuarios/Details");
                }
                if (usuario.TIPO_CONTA.Equals("PRO"))
                {
                    ViewBag.TIPO_CONTA = new SelectList(
                    new List<SelectListItem>
                    {
                    new SelectListItem { Selected = true, Text = "PRO",Value = "PRO"},
                    new SelectListItem { Selected = false, Text = "GRATUITA",Value = "GRATUITA"},
                    }, "Value", "Text");
                }
                else
                {
                    ViewBag.TIPO_CONTA = new SelectList(
                    new List<SelectListItem>
                    {
                    new SelectListItem { Selected = true, Text = "GRATUITO",Value = "GRATUITA"},
                    new SelectListItem { Selected = false, Text = "PRO",Value = "PRO"},
                    }, "Value", "Text");
                }
                ViewData["Mensagem"] = "Não foi possível alterar, por favor tente mais tarde.";
                return View(usuario);
            }
            else
            {
                if (usuario.TIPO_CONTA.Equals("PRO"))
                {
                    ViewBag.TIPO_CONTA = new SelectList(
                    new List<SelectListItem>
                    {
                    new SelectListItem { Selected = true, Text = "PRO",Value = "PRO"},
                    new SelectListItem { Selected = false, Text = "GRATUITA",Value = "GRATUITA"},
                    }, "Value", "Text");
                }
                else
                {
                    ViewBag.TIPO_CONTA = new SelectList(
                    new List<SelectListItem>
                    {
                    new SelectListItem { Selected = true, Text = "GRATUITO",Value = "GRATUITA"},
                    new SelectListItem { Selected = false, Text = "PRO",Value = "PRO"},
                    }, "Value", "Text");
                }
                ViewData["Mensagem"] = "A senha não confere com a confirmação da senha.";
                return View(usuario);
            }

        }

        // GET: /Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: /Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            usuario.PAPEL = "Desativado";

            //tenta alterar o cadastro do usuario
            try
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Message"] = "Erro ao cadastrar: " + e;
                return View(usuario);
            }
            Session["Nome"] = null;//limpa usuario
            Session["Conta"] = null;//limpar nome
            Session["Papel"] = null;//limpa papel
            Session["Usuario"] = null;//limpa usuario
            Session["imagem"] = null;//limpa imagem
            Session["aviso"] = null;//limpa aviso
            return RedirectToAction("../Home/Index");
        }

        //GET - Login
        public ActionResult Login()
        {
            var user = db.Usuarios.Where(x => x.PAPEL.Equals("Admin")).FirstOrDefault();//verifica se há um administrador cadastrado
            if (user != null)
            {
                return View();
            }
            return RedirectToAction("../Usuarios/CreatePro");

        }

        //POST - login - recebe dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "EMAIL,SENHA")]Usuario usuario)
        {
            var TempUser = db.Usuarios.Where(x => x.EMAIL.Equals(usuario.EMAIL)).FirstOrDefault();//busca o email cadastrado
            if (TempUser != null)
            {
                if (TempUser.PAPEL.Equals("Desativado"))//verifica se a conta está desativada
                {
                    ViewData["Mensagem"] = "Conta desativada, clique em 'Esqueceu a senha' e forneça seu email que reativaremos sua conta.";
                    return View(usuario);
                }
                using (MD5 md5Hash = MD5.Create())
                {

                    if (VerifyMd5Hash(md5Hash, usuario.SENHA, TempUser.SENHA))//compara a senha inserida criptografada com a encontrada
                    {
                        if (usuario.SENHA.Equals("inicial123"))
                        {
                            Session["aviso"] = "Nova senha";
                        }
                        
                        Session["Nome"] = TempUser.NOME;//abre sessão com o nome
                        Session["Papel"] = TempUser.PAPEL;//abre sessão com o papel do usuario
                        Session["Conta"] = TempUser.TIPO_CONTA;//abre sessão com o tipo de conta
                        Session["Usuario"] = TempUser;////abre sessão com o usuario
                        Session["sigla"] = TempUser.NOME.Substring(0, 1) + TempUser.SOBRENOME.Substring(0, 1);
                        return RedirectToAction("../Home/Inicio");
                    }
                    else
                    {
                        ViewData["Mensagem"] = "Senha inválida";
                        return View();
                    }
                }
            }
            else
            {
                ViewData["Mensagem"] = "Email inválido";
                return View();
            }
        }

        //Encerrar sessão
        public ActionResult Logoff()
        {
            Session["Nome"] = null;//limpa usuario
            Session["Conta"] = null;//limpar nome
            Session["Papel"] = null;//limpa papel
            Session["Usuario"] = null;//limpa usuario
            Session["sigla"] = null;//limpa imagem
            Session["aviso"] = null;//limpa aviso
            return RedirectToAction("../Home/Index");
        }

        //função para criptografar
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

        //comparação de variaveis criptografadas.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
