using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Web;
using System.Web.Mvc;

using Inspinia_MVC5.Models;


namespace Inspinia_MVC5.Controllers
{

    public class AvaliacaosController : Controller
    {
        private InspiniaContext db = new InspiniaContext();

        // GET: /Avaliacaos/Index do usuario
        public ActionResult IndexUsuario()
        {
            Usuario user = Session["Usuario"] as Usuario;//busca usuario logado
            var avaliacaos = db.Avaliacaos.Where(a => a.ID_USUARIO.Equals(user.ID_USUARIO));//busca todas as avaliações deste usuario
            return View(avaliacaos.ToList());
        }

        // GET: /Avaliacaos/Index Administrador
        public ActionResult Index()
        {
            var avaliacaos = db.Avaliacaos.Include(a => a.Usuario);//busca todas as avaliações registradas no sistema
            return View(avaliacaos.ToList());
        }

        // GET: /Avaliacaos/Aviso de limite de avaliações atigido para usuario gratuito
        public ActionResult Aviso()
        {
            return View();
        }

        //controlador de páginas
        public ActionResult Continue(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //busca a avaliação em questão
            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            TempData["Avaliacao"] = avaliacao;//armazena temporariamente
            //busca se há formulário de negocio para esta avaliação
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();
            if (negocio == null)
            {
                //se for nulo direciona para o preenchimento
                return RedirectToAction("../Negocios/Create");
            }
            else
            {
                //se já existe o formulario do negócio, busca se há formulário de avaliação tecnica para esta avaliação
                Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();
                if (tecnico == null)
                {
                    //se for nulo direciona para o preenchimento
                    return RedirectToAction("../Tecnicoes/Create");
                }
                else
                {
                    //se já existe o formulario de qualidade tecnica, busca se há formulário de avaliação de ambiente para esta avaliação
                    Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();
                    if (ambiente == null)
                    {
                        //se for nulo direciona para o preenchimento
                        return RedirectToAction("../Ambientes/Create");
                    }

                }
            }

            return View(avaliacao);
        }

        // GET: /Avaliacaos/Create
        public ActionResult Create()
        {
            Usuario user = Session["Usuario"] as Usuario;//busca usuario logado
            if (user.TIPO_CONTA.Equals("GRATUITA"))//verifica se o tipo de conta é gratuita
            {
                int Total_avaliacao = db.Avaliacaos.Where(x => x.ID_USUARIO.Equals(user.ID_USUARIO)).Count();//busca o total de avaliações
                if (Total_avaliacao >= 3)//se o total for igual ou maior que 3, não permite mais fazer nenhuma avaliação
                {
                    return RedirectToAction("Aviso");
                }
            }
            
            Avaliacao avaliacao = new Avaliacao();//cria uma nova avaliação
            avaliacao.ID_USUARIO = user.ID_USUARIO;//insere o codigo de identificação do usuario
            avaliacao.DATA = DateTime.Today.Date;//insere a data de criação
            avaliacao.STATUS = "Iniciado";//insere o status iniciado

            return View(avaliacao);
        }

        // POST: /Avaliacaos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_AVALIACAO,NOME_SISTEMA,DATA,STATUS,RESULTADO,AV_NOTA_FINAL,ID_USUARIO")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                //tenta salvar a avaliação  
                try
                {
                    db.Avaliacaos.Add(avaliacao);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = ViewData["Message"] = "Erro ao atualizar o status da avaliação: " + e;
                    return View(avaliacao);
                }

                TempData["Avaliacao"] = avaliacao;//armazena temporariamente a avaliação

                switch (Request.Form["Submit"])
                {
                    case "Salvar e sair":
                        return RedirectToAction("../Avaliacaos/IndexUsuario");


                    case "Salvar e continuar":
                        return RedirectToAction("../Negocios/Create");

                }
            }
            return View(avaliacao);
        }

        // GET: /Avaliacaos/Edit/5
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Avaliacao avaliacao = db.Avaliacaos.Find(id);

            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        // POST: /Avaliacaos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_AVALIACAO,NOME_SISTEMA,DATA,STATUS,RESULTADO,AV_NOTA_FINAL,ID_USUARIO")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                //atualiza a avaliação  
                try
                {
                    db.Entry(avaliacao).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = ViewData["Message"] = "Erro ao atualizar o status da avaliação: " + e;
                    return View(avaliacao);
                }
                
                return RedirectToAction("IndexUsuario");
            }
            return View(avaliacao);
        }

        // GET: /Avaliacaos/Delete/5
        public ActionResult Delete(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Avaliacao avaliacao = db.Avaliacaos.Find(id);

            if (avaliacao == null)
            {
                return HttpNotFound();
            }
            return View(avaliacao);
        }

        // POST: /Avaliacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)

        {

            Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(id)).FirstOrDefault();
            if (ambiente != null)
            {
                db.Ambientes.Remove(ambiente);
            }
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(id)).FirstOrDefault();
            if (negocio != null)
            {
                db.Negocios.Remove(negocio);
            }
            Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(id)).FirstOrDefault();
            if (tecnico != null)
            {
                db.Tecnicoes.Remove(tecnico);
            }
            Feedback feedback = db.Feedbacks.Where(x => x.ID_AVALIACAO.Equals(id)).FirstOrDefault();
            if (feedback != null)
            {
                db.Feedbacks.Remove(feedback);
            }

            //exclui a avaliação  
            try
            {
                Avaliacao avaliacao = db.Avaliacaos.Find(id);
                db.Avaliacaos.Remove(avaliacao);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Message"] = ViewData["Message"] = "Erro ao atualizar o status da avaliação: " + e;
                return View(id);
            }
            

            return RedirectToAction("IndexUsuario");
        }

        // Controlador de resultados
        public ActionResult Resultado(int id)
        {
            bool resultado = Gerar_Resultado(id);//função para calcular o resultado
            if (resultado)//se calculou resultado com sucesso
            {
                return RedirectToAction("../Avaliacaos/ViewResultado/" + id);
            }
            return RedirectToAction("../Avaliacaos/IndexUsuario");
        }

        //função que calcula resultado da avaliação
        public bool Gerar_Resultado(int id)
        {
            //recebe o código de identificação da avaliação
            Avaliacao avaliacao = db.Avaliacaos.Find(id);//encontra a avaliação
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault(); //busca as respostas do questionario de negocio da avaliação em questão
            Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de qualidade tecnica da avaliação em questão
            Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de qualidade do ambiente da avaliação em questão

            //media das qualidades= qualidade final
            float nota_final_qualidade = (tecnico.TEC_NOTA_FINAL + ambiente.AMB_NOTA_FINAL) / 2;

            // Aplicação da programação linear
            //Função Objetivo = nota final
            avaliacao.AV_NOTA_FINAL = (0.4F * nota_final_qualidade) + (0.6F * negocio.NEG_NOTA_FINAL); //Max Z = 0.4x + 0.6y


            //Aplicação das restrições
            if (negocio.NEG_NOTA_FINAL + nota_final_qualidade <= 20)//x + y≤20
            {
                if (negocio.NEG_NOTA_FINAL <= 10 & nota_final_qualidade <= 10)//x≤10,y≤10
                {
                    if (negocio.NEG_NOTA_FINAL >= 0 & nota_final_qualidade >= 0)//x≥0, y≥0
                    {
                        if (negocio.NEG_NOTA_FINAL < 5 & nota_final_qualidade < 5)//primeiro quadrante
                        {
                            avaliacao.RESULTADO = "descartar o sistema";
                        }
                        else if (negocio.NEG_NOTA_FINAL < 5 & nota_final_qualidade >= 5)//segundo quadrante
                        {
                            avaliacao.RESULTADO = "descartar ou manter o sistema";
                        }
                        else if (negocio.NEG_NOTA_FINAL >= 5 & nota_final_qualidade < 5)//terceiro quadrante
                        {
                            avaliacao.RESULTADO = "melhorar ou substituir o sistema";
                        }
                        else if (negocio.NEG_NOTA_FINAL >= 5 & nota_final_qualidade >= 5)//quarto quadrante
                        {
                            avaliacao.RESULTADO = "manter o sistema";
                        }
                    }
                }
            }
            db.Entry(avaliacao).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        // GET: /Avaliacaos/Visualização do resultado da avaliação
        public ActionResult ViewResultado(int? id)
        {
            //recebe o código de identificação da avaliação
            if (id == null)//verifica se é null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);//encontra a avaliação
            TempData["avaliacao"] = avaliacao;//armazena temporariamente
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault(); //busca as respostas do questionario de negocio da avaliação em questão
            Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de qualidade tecnica da avaliação em questão
            Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de qualidade do ambiente da avaliação em questão

            //envio de dados para a view
            ViewData["nota_final"] = avaliacao.AV_NOTA_FINAL.ToString("n2");
            ViewData["nota_neg"] = negocio.NEG_NOTA_FINAL.ToString("n2");
            ViewData["nota_tec"] = tecnico.TEC_NOTA_FINAL.ToString("n2");
            ViewData["nota_amb"] = ambiente.AMB_NOTA_FINAL.ToString("n2");
            ViewData["resultado"] = avaliacao.RESULTADO;

            return View(avaliacao);
        }

        //---------------graficos da viewResultado-----------------

        //grafico barra notas gerais
        [HttpPost]
        public JsonResult GraficoBarGeral()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de negocio
            Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario tecnico
            Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario ambiente
            List<object> iDados = new List<object>();
            //Criando tabela para enviar por Json
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Questões"] = "Negócio";
            dr["Notas"] = negocio.NEG_NOTA_FINAL;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Qualidade Técnica";
            dr["Notas"] = tecnico.TEC_NOTA_FINAL;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Qualidade do ambiente";
            dr["Notas"] = ambiente.AMB_NOTA_FINAL;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);
        }

        //grafico doug notas gerais
        public JsonResult GraficoDougGeral()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de negocio
            Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario tecnico
            Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario ambiente
            List<object> iDados = new List<object>();
            //Criando tabela para enviar por Json
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Questões"] = "Negócio";
            dr["Notas"] = negocio.NEG_NOTA_FINAL;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Qualidade Técnica";
            dr["Notas"] = tecnico.TEC_NOTA_FINAL;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Qualidade do ambiente";
            dr["Notas"] = ambiente.AMB_NOTA_FINAL;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);
        }

        //grafico barra itens relevantes do negocio
        [HttpPost]
        public JsonResult GraficoNegocio()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente
            Negocio neg = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de negocio

            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Questões"] = "Utilização";
            dr["Notas"] = neg.RESPOSTA4;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Apoia processos";
            dr["Notas"] = neg.RESPOSTA6;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Desvio de tarefa";
            dr["Notas"] = neg.RESPOSTA7;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Confiança";
            dr["Notas"] = neg.RESPOSTA8;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Saídas úteis";
            dr["Notas"] = neg.RESPOSTA9;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Custo manuntenção";
            dr["Notas"] = neg.RESPOSTA10;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Tecnologias futuras";
            dr["Notas"] = neg.RESPOSTA12;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);

        }

        //grafico barra itens relevantes da qualidade técnica
        [HttpPost]
        public JsonResult GraficoTecnico()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente
            Tecnico tec = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario tecnico
            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Questões"] = "Código";
            dr["Notas"] = tec.RESPOSTA1;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Estrutura de controles";
            dr["Notas"] = tec.RESPOSTA2;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Variáveis";
            dr["Notas"] = tec.RESPOSTA3;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Documentação";
            dr["Notas"] = tec.RESPOSTA4;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Modelo de dados";
            dr["Notas"] = tec.RESPOSTA5;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Qualidade dos dados";
            dr["Notas"] = tec.RESPOSTA6;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Desempenho da aplicação";
            dr["Notas"] = tec.RESPOSTA7;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Linguagem";
            dr["Notas"] = tec.RESPOSTA9;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Controle de versão";
            dr["Notas"] = tec.RESPOSTA10;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Dados de teste";
            dr["Notas"] = tec.RESPOSTA12;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Teste regressão";
            dr["Notas"] = tec.RESPOSTA13;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Habilidade de pessoal";
            dr["Notas"] = tec.RESPOSTA14;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Experiência de pessoal";
            dr["Notas"] = tec.RESPOSTA15;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Solicitação de mudança";
            dr["Notas"] = tec.RESPOSTA16;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Interface";
            dr["Notas"] = tec.RESPOSTA17;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Volume da dados";
            dr["Notas"] = tec.RESPOSTA18;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Disponibilidade";
            dr["Notas"] = tec.RESPOSTA19;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Customização";
            dr["Notas"] = tec.RESPOSTA20;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);

        }

        //grafico barra itens relevantes da qualidade do ambiente
        [HttpPost]
        public JsonResult GraficoAmbiente()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente
            Ambiente amb = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario ambiente
            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Questões"] = "Fornecedor";
            dr["Notas"] = amb.RESPOSTA2;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Software de apoio";
            dr["Notas"] = amb.RESPOSTA5;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Desempenho do hardware";
            dr["Notas"] = amb.RESPOSTA8;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Apoio local";
            dr["Notas"] = amb.RESPOSTA10;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Custos com licenças";
            dr["Notas"] = amb.RESPOSTA11;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Interface com outros sistemas";
            dr["Notas"] = amb.RESPOSTA12;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);

        }

        //---------------laudo-------------------------------------------------

        //GET página laudo técnico
        public ActionResult LaudoTecnico(int? id)

        {
            //recebe o código de identificação da avaliação
            if (id == null)//verifica se é null
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);//encontra a avaliação
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault(); //busca as respostas do questionario de negocio da avaliação em questão
            Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de qualidade tecnica da avaliação em questão
            Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca as respostas do questionario de qualidade do ambiente da avaliação em questão

            float nota_final_qualidade = (tecnico.TEC_NOTA_FINAL + ambiente.AMB_NOTA_FINAL) / 2;

            //Negocio
            switch (negocio.RESPOSTA1)
            {
                case 0:
                    ViewData["Q1R1"] = "0 a 9";
                    break;
                case 5:
                    ViewData["Q1R1"] = "10 a 19";
                    break;
                case 10:
                    ViewData["Q1R1"] = "mais de 20";
                    break;
            }

            switch (negocio.RESPOSTA2)
            {
                case 0:
                    ViewData["Q1R2"] = "0 a 99";
                    break;
                case 5:
                    ViewData["Q1R2"] = "100 a 499";
                    break;
                case 10:
                    ViewData["Q1R2"] = "mais que 500";
                    break;
            }


            if (negocio.RESPOSTA3 == 0)
            {
                ViewData["Q1R3"] = "menos de R$2,4 milhões";
            }
            else if (negocio.RESPOSTA3 == 2.5)
            {
                ViewData["Q1R3"] = "R$2,4 a R$16 milhões";
            }
            else if (negocio.RESPOSTA3 == 5)
            {
                ViewData["Q1R3"] = "R$16 a R$90 milhões";
            }
            else if (negocio.RESPOSTA3 == 7.5)
            {
                ViewData["Q1R3"] = "R$90 a R$300 milhões";
            }
            else if (negocio.RESPOSTA3 == 10)
            {
                ViewData["Q1R3"] = "mais de R$300 milhões";
            }
            switch (negocio.RESPOSTA4)
            {
                case 0:
                    ViewData["Q1R4"] = "poucas vezes";
                    break;
                case 5:
                    ViewData["Q1R4"] = "as vezes";
                    break;
                case 10:
                    ViewData["Q1R4"] = "muitas vezes";
                    break;
            }

            switch (negocio.RESPOSTA5)
            {
                case 0:
                    ViewData["Q1R5"] = "pequena";
                    break;
                case 5:
                    ViewData["Q1R5"] = "considerável";
                    break;
                case 10:
                    ViewData["Q1R5"] = "grande";
                    break;
            }

            switch (negocio.RESPOSTA6)
            {
                case 0:
                    ViewData["Q1R6"] = "oferece";
                    break;

                case 10:
                    ViewData["Q1R6"] = "não oferece";
                    break;
            }

            switch (negocio.RESPOSTA7)
            {
                case 0:
                    ViewData["Q1R7"] = "necessita";
                    break;

                case 10:
                    ViewData["Q1R7"] = "não necessita";
                    break;
            }

            switch (negocio.RESPOSTA9)
            {
                case 0:
                    ViewData["Q1R9"] = "são";
                    break;

                case 10:
                    ViewData["Q1R9"] = "não são";
                    break;
            }

            switch (negocio.RESPOSTA10)
            {
                case 0:
                    ViewData["Q1R10"] = "alto";
                    break;
                case 5:
                    ViewData["Q1R10"] = "médio";
                    break;
                case 10:
                    ViewData["Q1R10"] = "baixo";
                    break;
            }

            switch (negocio.RESPOSTA11)
            {
                case 0:
                    ViewData["Q1R11"] = "possui";
                    break;

                case 10:
                    ViewData["Q1R11"] = "não possui";
                    break;
            }
            switch (negocio.RESPOSTA12)
            {
                case 0:
                    ViewData["Q1R12"] = "não possui";
                    break;

                case 10:
                    ViewData["Q1R12"] = "possui";
                    break;
            }


            //Tecnico

            switch (tecnico.RESPOSTA1)
            {
                case 0:
                    ViewData["Q2R1"] = "difícil";
                    break;
                case 5:
                    ViewData["Q2R1"] = "um pouco difícil";
                    break;
                case 10:
                    ViewData["Q2R1"] = "fácil";
                    break;
            }

            switch (tecnico.RESPOSTA2)
            {
                case 0:
                    ViewData["Q2R2"] = "alto";
                    break;
                case 5:
                    ViewData["Q2R2"] = "moderado";
                    break;
                case 10:
                    ViewData["Q2R2"] = "baixo";
                    break;
            }

            switch (tecnico.RESPOSTA3)
            {
                case 0:
                    ViewData["Q2R3"] = "não possuem";
                    break;

                case 10:
                    ViewData["Q2R3"] = "possuem";
                    break;
            }

            switch (tecnico.RESPOSTA4)
            {
                case 0:
                    ViewData["Q2R4"] = "não está";
                    break;

                case 10:
                    ViewData["Q2R4"] = "está";
                    break;
            }

            switch (tecnico.RESPOSTA5)
            {
                case 0:
                    ViewData["Q2R5"] = "não é";
                    break;

                case 10:
                    ViewData["Q2R5"] = "é";
                    break;
            }

            switch (tecnico.RESPOSTA6)
            {
                case 0:
                    ViewData["Q2R6"] = "não são";
                    break;

                case 10:
                    ViewData["Q2R6"] = "são";
                    break;
            }

            switch (tecnico.RESPOSTA7)
            {
                case 0:
                    ViewData["Q2R7"] = "não é";
                    break;

                case 10:
                    ViewData["Q2R7"] = "é";
                    break;
            }

            ViewData["Q2R8"] = tecnico.RESPOSTA8;

            switch (tecnico.RESPOSTA9)
            {
                case 0:
                    ViewData["Q2R9"] = "não é";
                    break;

                case 10:
                    ViewData["Q2R9"] = "ainda é";
                    break;
            }

            switch (tecnico.RESPOSTA11)
            {
                case 0:
                    ViewData["Q2R11"] = "não possui";
                    break;

                case 10:
                    ViewData["Q2R11"] = "possui";
                    break;
            }

            switch (tecnico.RESPOSTA12)
            {
                case 0:
                    ViewData["Q2R12"] = "não possui";
                    break;

                case 10:
                    ViewData["Q2R12"] = "possui";
                    break;
            }

            switch (tecnico.RESPOSTA14)
            {
                case 0:
                    ViewData["Q2R14"] = "não tem";
                    break;

                case 10:
                    ViewData["Q2R14"] = "tem";
                    break;
            }

            switch (tecnico.RESPOSTA15)
            {
                case 0:
                    ViewData["Q2R15"] = "não possui";
                    break;

                case 10:
                    ViewData["Q2R15"] = "possui";
                    break;
            }

            switch (tecnico.RESPOSTA16)
            {
                case 0:
                    ViewData["Q2R16"] = "alta";
                    break;
                case 5:
                    ViewData["Q2R16"] = "média";
                    break;
                case 10:
                    ViewData["Q2R16"] = "baixa";
                    break;
            }

            switch (tecnico.RESPOSTA17)
            {
                case 0:
                    ViewData["Q2R17"] = "alta";
                    break;
                case 5:
                    ViewData["Q2R17"] = "média";
                    break;
                case 10:
                    ViewData["Q2R17"] = "baixa";
                    break;
            }

            switch (tecnico.RESPOSTA18)
            {
                case 0:
                    ViewData["Q2R18"] = "grande";
                    break;
                case 5:
                    ViewData["Q2R18"] = "médio";
                    break;
                case 10:
                    ViewData["Q2R18"] = "pequeno";
                    break;
            }

            switch (tecnico.RESPOSTA19)
            {
                case 0:
                    ViewData["Q2R19"] = "não atende";
                    break;

                case 10:
                    ViewData["Q2R19"] = "atende";
                    break;
            }

            switch (tecnico.RESPOSTA20)
            {
                case 0:
                    ViewData["Q2R20"] = "pequena";
                    break;
                case 5:
                    ViewData["Q2R20"] = "média";
                    break;
                case 10:
                    ViewData["Q2R20"] = "grande";
                    break;
            }

            //Ambiente

            switch (ambiente.RESPOSTA1)
            {
                case 0:
                    ViewData["Q3R1"] = "não";
                    break;

                case 10:
                    ViewData["Q3R1"] = "ainda";
                    break;
            }

            switch (ambiente.RESPOSTA2)
            {
                case 0:
                    ViewData["Q3R2"] = "não é";
                    break;

                case 10:
                    ViewData["Q3R2"] = "é";
                    break;
            }

            switch (ambiente.RESPOSTA3)
            {
                case 0:
                    ViewData["Q3R3"] = "não há";
                    break;

                case 10:
                    ViewData["Q3R3"] = "há";
                    break;
            }

            switch (ambiente.RESPOSTA4)
            {
                case 0:
                    ViewData["Q3R4"] = "gera";
                    break;

                case 10:
                    ViewData["Q3R4"] = "não gera";
                    break;
            }

            switch (ambiente.RESPOSTA5)
            {
                case 0:
                    ViewData["Q3R5"] = "ocasiona";
                    break;

                case 10:
                    ViewData["Q3R5"] = "não ocasiona";
                    break;
            }

            switch (ambiente.RESPOSTA6)
            {
                case 0:
                    ViewData["Q3R6"] = "mais de 20 anos";
                    break;
                case 5:
                    ViewData["Q3R6"] = "entre 10 e 20 anos";
                    break;
                case 10:
                    ViewData["Q3R6"] = "menos de 10 anos";
                    break;
            }

            switch (ambiente.RESPOSTA7)
            {
                case 0:
                    ViewData["Q3R7"] = "mais de 20 anos";
                    break;
                case 5:
                    ViewData["Q3R7"] = "entre 10 e 20 anos";
                    break;
                case 10:
                    ViewData["Q3R7"] = "menos de 10 anos";
                    break;
            }

            switch (ambiente.RESPOSTA8)
            {
                case 0:
                    ViewData["Q3R8"] = "não é";
                    break;

                case 10:
                    ViewData["Q3R8"] = "é";
                    break;
            }

            switch (ambiente.RESPOSTA10)
            {
                case 0:
                    ViewData["Q3R10"] = "necessita";
                    break;

                case 10:
                    ViewData["Q3R10"] = "não necessita";
                    break;
            }

            switch (ambiente.RESPOSTA11)
            {
                case 0:
                    ViewData["Q3R11"] = "alto";
                    break;
                case 5:
                    ViewData["Q3R11"] = "médio";
                    break;
                case 10:
                    ViewData["Q3R11"] = "baixo";
                    break;
            }


            //---------------------------------------

            ViewData["estrategia"] = avaliacao.RESULTADO;
            ViewData["nota_neg"] = avaliacao.AV_NOTA_FINAL;
            ViewData["nota_qua"] = nota_final_qualidade;

            return View();


        }

        //-------------------------viewComparação------------------------------

        // GET: /Avaliacaos/Visualização de comparação de resultados
        public ActionResult ViewComparacao(int? id)
        {
            if (id == null)//recebe a id da avaliação e verifica se nulo
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);//busca a avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente

            return View(avaliacao);
        }

        //---------------graficos da viewComparação-----------------

        //Grafico Resultado X Estratégia adotada
        [HttpPost]
        public JsonResult Grafico1BarComp()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca avaliação em questão 
            TempData["avaliacao"] = avaliacao;

            var feedbacks = db.Feedbacks.Where(x => x.RESULTADO.Equals(avaliacao.RESULTADO) & x.ID_AVALIACAO != avaliacao.ID_AVALIACAO).ToList(); //busca feedback  
            int descarte = 0;
            int mantem = 0;
            int melhora = 0;
            int substitui = 0;


            if (feedbacks != null)
            {
                foreach (var item in feedbacks)
                {

                    if (item.ESTRATEGIA_ADOTADA != null)
                    {

                        if (item.ESTRATEGIA_ADOTADA.Contains("Descartar"))
                        {
                            descarte++;
                        }
                        else if (item.ESTRATEGIA_ADOTADA.Contains("Manter"))
                        {
                            mantem++;
                        }
                        else if (item.ESTRATEGIA_ADOTADA.Contains("Melhorar"))
                        {
                            melhora++;
                        }
                        else if (item.ESTRATEGIA_ADOTADA.Contains("Substituir"))
                        {
                            substitui++;
                        }
                    }
                };
            }
            List<object> iDados = new List<object>();
            //Criando tabela para enviar por Jason
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            //criando linhas
            DataRow dr = dt.NewRow();
            dr["Questões"] = "Descartar o sistema";
            dr["Notas"] = descarte;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Manter do mesmo jeito";
            dr["Notas"] = mantem;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Melhorar o sistema";
            dr["Notas"] = melhora;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Substituir o sistema";
            dr["Notas"] = substitui;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);
        }

        // grafico Linguagem X estratégia adotada
        [HttpPost]
        public JsonResult Grafico2BarComp()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca avaliação em questão
            TempData["avaliacao"] = avaliacao;
            Tecnico tec = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca avaliação da qualidade tecnica da avaliação em questão
            var tecnicos = db.Tecnicoes.Where(x => x.RESPOSTA8.Contains(tec.RESPOSTA8) & x.ID_AVALIACAO != avaliacao.ID_AVALIACAO).ToList();//busca todas as avaliações de qualidade tecnicas que possuem a mesma linguagem

            int descarte = 0;
            int mantem = 0;
            int melhora = 0;
            int substitui = 0;
            
            if (tecnicos != null)
            {
                foreach (var item in tecnicos)
                {
                    var feedback = db.Feedbacks.Where(x => x.ID_AVALIACAO.Equals(item.ID_AVALIACAO)).FirstOrDefault(); //busca o feedback de cada avaliação
                    if (feedback != null)
                    {
                        if (feedback.ESTRATEGIA_ADOTADA != null)
                        {

                            if (feedback.ESTRATEGIA_ADOTADA.Contains("Descartar"))
                            {
                                descarte++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Manter"))
                            {
                                mantem++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Melhorar"))
                            {
                                melhora++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Substituir"))
                            {
                                substitui++;
                            }
                        }
                    }
                };
            }
            List<object> iDados = new List<object>();
            //Cria uma tabela nova para enviar por Json
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            //adiciona linhas na tabela
            DataRow dr = dt.NewRow();
            dr["Questões"] = "Descartar o sistema";
            dr["Notas"] = descarte;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Manter do mesmo jeito";
            dr["Notas"] = mantem;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Melhorar o sistema";
            dr["Notas"] = melhora;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Substituir o sistema";
            dr["Notas"] = substitui;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);
        }

        // grafico Idade do hardware X estratégia adotada
        [HttpPost]
        public JsonResult Grafico3BarComp()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca a avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente
            Ambiente amb = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca a referida avaliação de ambiente
            var ambientes = db.Ambientes.Where(x => x.RESPOSTA6.Equals(amb.RESPOSTA6) & x.ID_AVALIACAO != avaliacao.ID_AVALIACAO).ToList();//busca todas as avaliações de ambiente que tenham a mesma idade de hardware
            int descarte = 0;
            int mantem = 0;
            int melhora = 0;
            int substitui = 0;

            if (ambientes != null)
            {
                foreach (var item in ambientes)
                {
                    var feedback = db.Feedbacks.Where(x => x.ID_AVALIACAO.Equals(item.ID_AVALIACAO)).FirstOrDefault();//busca o feedback de cada avaliação e compara a estrategia adotada

                    if (feedback != null)
                    {
                        if (feedback.ESTRATEGIA_ADOTADA != null)
                        {

                            if (feedback.ESTRATEGIA_ADOTADA.Contains("Descartar"))
                            {
                                descarte++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Manter"))
                            {
                                mantem++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Melhorar"))
                            {
                                melhora++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Substituir"))
                            {
                                substitui++;
                            }
                        }
                    }
                }
            }
            List<object> iDados = new List<object>();
            //Criando tabela para enviar por Json
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Questões"] = "Descartar o sistema";
            dr["Notas"] =descarte;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Manter do mesmo jeito";
            dr["Notas"] =mantem;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Melhorar o sistema";
            dr["Notas"] = melhora;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Substituir o sistema";
            dr["Notas"] =substitui;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);
        }

        //grafico Idade do software X estratégia adotada
        [HttpPost]
        public JsonResult Grafico4BarComp()
        {
            Avaliacao avaliacao = TempData["avaliacao"] as Avaliacao;//busca a avaliação em questão
            TempData["avaliacao"] = avaliacao;//armazena temporariamente

            //Ambiente amb = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca a avaliação da qualidade do ambiente da avaliação em questão
            //var ambientes = db.Ambientes.Where(x => x.RESPOSTA7.Equals(amb.RESPOSTA7)).ToList();// & x.ID_AVALIACAO != avaliacao.ID_AVALIACAO //busca todos os questionários de avaliação do ambiente que tenham a mesma idade do software
            Ambiente amb = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();//busca avaliação da qualidade tecnica da avaliação em questão
            var ambientes = db.Ambientes.Where(x => x.RESPOSTA7.Equals(amb.RESPOSTA7) & x.ID_AVALIACAO != avaliacao.ID_AVALIACAO).ToList();//busca todas as avaliações de qualidade tecnicas que possuem a mesma linguagem

            int descarte = 0;
            int mantem = 0;
            int melhora = 0;
            int substitui = 0;

            if (ambientes != null)
            {
                foreach (var item in ambientes)
                {
                    var feedback = db.Feedbacks.Where(x => x.ID_AVALIACAO.Equals(item.ID_AVALIACAO)).FirstOrDefault(); //busca o feedback de cada avaliação e compara as estratégias adotadas
                    if (feedback != null)
                    {
                        if (feedback.ESTRATEGIA_ADOTADA != null)
                        {

                            if (feedback.ESTRATEGIA_ADOTADA.Contains("Descartar"))
                            {
                                descarte++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Manter"))
                            {
                                mantem++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Melhorar"))
                            {
                                melhora++;
                            }
                            else if (feedback.ESTRATEGIA_ADOTADA.Contains("Substituir"))
                            {
                                substitui++;
                            }
                        }
                    }
                };
            }
            List<object> iDados = new List<object>();
            //Criando tabela para enviar por Json
            DataTable dt = new DataTable();
            dt.Columns.Add("Questões", System.Type.GetType("System.String"));
            dt.Columns.Add("Notas", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Questões"] = "Descartar o sistema";
            dr["Notas"] = descarte;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Manter do mesmo jeito";
            dr["Notas"] = mantem;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Melhorar o sistema";
            dr["Notas"] = melhora;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Questões"] = "Substituir o sistema";
            dr["Notas"] = substitui;
            dt.Rows.Add(dr);

            //Percorrendo e extraindo cada DataColumn para List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iDados.Add(x);
            }
            //Dados retornados no formato JSON
            return Json(iDados, JsonRequestBehavior.AllowGet);
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
