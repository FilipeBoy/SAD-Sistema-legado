
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

    public class NegociosController : Controller
    {
        private InspiniaContext db = new InspiniaContext();


        // GET: /Negocios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Negocios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_AVNEGOCIO,ID_AVALIACAO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7,RESPOSTA8,RESPOSTA9,RESPOSTA10,RESPOSTA11,RESPOSTA12,NEG_NOTA_FINAL")] Negocio negocio)
        {
            Avaliacao avaliacao = TempData["Avaliacao"] as Avaliacao;//busca a avaliação em questão
            TempData["Avaliacao"] = avaliacao;
            negocio.ID_AVALIACAO = avaliacao.ID_AVALIACAO;
            //calculo da nota final
            negocio.NEG_NOTA_FINAL = (negocio.RESPOSTA1 + negocio.RESPOSTA2 + negocio.RESPOSTA3 + negocio.RESPOSTA4 + negocio.RESPOSTA5
                     + negocio.RESPOSTA6 + negocio.RESPOSTA7 + negocio.RESPOSTA8 + negocio.RESPOSTA9 + negocio.RESPOSTA10
                      + negocio.RESPOSTA11 + negocio.RESPOSTA12) / 12;
            //tenta salvar o formulário de negócio
            try
            {
                db.Negocios.Add(negocio);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Message"] = "Erro ao guardar formuláriodo: " + e;
                return View(negocio);
            }

            //atualiza o status da avaliação  
            try
            {
                avaliacao.STATUS = "Andamento";
                db.Entry(avaliacao).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Message"] = ViewData["Message"] = "Erro ao atualizar o status da avaliação: " + e;
                return View(negocio);
            }


            switch (Request.Form["Submit"])
            {
                case "sair":
                    return RedirectToAction("../Avaliacaos/IndexUsuario");

                case "continuar":
                    return RedirectToAction("../Tecnicoes/Create");

            }

            ViewData["Mensagem"] = "Nada deu certo";
            return View();
        }

        // GET: /Negocios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            Negocio negocio = db.Negocios.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();
            if (negocio == null)
            {
                return HttpNotFound();
            }
            return View(negocio);
        }

        // POST: /Negocios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_AVNEGOCIO,ID_AVALIACAO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7,RESPOSTA8,RESPOSTA9,RESPOSTA10,RESPOSTA11,RESPOSTA12,NEG_NOTA_FINAL")] Negocio negocio)
        {
            if (ModelState.IsValid)
            {
                //calculo da nota final
                negocio.NEG_NOTA_FINAL = (negocio.RESPOSTA1 + negocio.RESPOSTA2 + negocio.RESPOSTA3 + negocio.RESPOSTA4 + negocio.RESPOSTA5
                     + negocio.RESPOSTA6 + negocio.RESPOSTA7 + negocio.RESPOSTA8 + negocio.RESPOSTA9 + negocio.RESPOSTA10
                      + negocio.RESPOSTA11 + negocio.RESPOSTA12) / 12;
                //tenta alterar o formulário de negócio
                try
                {
                    db.Entry(negocio).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = "Erro ao guardar formuláriodo: " + e;
                    return View(negocio);
                }

                int id = negocio.ID_AVALIACAO;
                switch (Request.Form["Submit"])
                {
                    case "sair":
                        return RedirectToAction("../Avaliacaos/Resultado/" + id);

                    case "continuar":
                        return RedirectToAction("../Tecnicoes/Edit/" + id);
                }
            }
            ViewData["Mensagem"] = "Ocorreu um erro ao tentar alterar.";
            return View(negocio);
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
