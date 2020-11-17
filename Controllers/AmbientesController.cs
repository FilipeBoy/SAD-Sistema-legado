
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

    public class AmbientesController : Controller
    {
        private InspiniaContext db = new InspiniaContext();


        // GET: /Ambientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Ambientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_AVAMBIENTE,ID_AVALIACAO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7,RESPOSTA8,RESPOSTA9,RESPOSTA10,RESPOSTA11,RESPOSTA12,AMB_NOTA_FINAL")] Ambiente ambiente)

        {
            Avaliacao avaliacao = TempData["Avaliacao"] as Avaliacao;
            TempData["Avaliacao"] = avaliacao;
            ambiente.ID_AVALIACAO = avaliacao.ID_AVALIACAO;
            //calculo da nota final
            ambiente.AMB_NOTA_FINAL = (ambiente.RESPOSTA1 + ambiente.RESPOSTA2 + ambiente.RESPOSTA3 + ambiente.RESPOSTA4 + ambiente.RESPOSTA5
                     + ambiente.RESPOSTA6 + ambiente.RESPOSTA7 + ambiente.RESPOSTA8 + ambiente.RESPOSTA9 + ambiente.RESPOSTA10
                      + ambiente.RESPOSTA11 + ambiente.RESPOSTA12) / 12;
            //tenta salvar o formulário de negócio
            try
            {
                db.Ambientes.Add(ambiente);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Message"] = "Erro ao guardar formuláriodo: " + e;
                return View(ambiente);
            }

            //atualiza o status da avaliação  
            try
            {
                avaliacao.STATUS = "Concluído";
                db.Entry(avaliacao).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Message"] = ViewData["Message"] = "Erro ao atualizar o status da avaliação: " + e;
                return View(ambiente);
            }
            switch (Request.Form["Submit"])
            {
                case "sair":
                    return RedirectToAction("../Avaliacaos/IndexUsuario");

                case "continuar":
                    int id = avaliacao.ID_AVALIACAO;
                    return RedirectToAction("../Avaliacaos/Resultado/" + id);
            }

            ViewData["Mensagem"] = "Nada deu certo";
            return View();
        }

        // GET: /Ambientes/Edit/5
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Avaliacao avaliacao = db.Avaliacaos.Find(id);

            Ambiente ambiente = db.Ambientes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();

            if (ambiente == null)
            {
                return HttpNotFound();
            }

            ViewBag.ID_AVALIACAO = new SelectList(db.Avaliacaos, "ID_AVALIACAO", "NOME_SISTEMA", ambiente.ID_AVALIACAO);

            return View(ambiente);
        }

        // POST: /Ambientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_AVAMBIENTE,ID_AVALIACAO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7,RESPOSTA8,RESPOSTA9,RESPOSTA10,RESPOSTA11,RESPOSTA12,AMB_NOTA_FINAL")] Ambiente ambiente)
        {
            if (ModelState.IsValid)
            {
                //calculo da nota final
                ambiente.AMB_NOTA_FINAL = (ambiente.RESPOSTA1 + ambiente.RESPOSTA2 + ambiente.RESPOSTA3 + ambiente.RESPOSTA4 + ambiente.RESPOSTA5
                     + ambiente.RESPOSTA6 + ambiente.RESPOSTA7 + ambiente.RESPOSTA8 + ambiente.RESPOSTA9 + ambiente.RESPOSTA10
                      + ambiente.RESPOSTA11 + ambiente.RESPOSTA12) / 12;
                //tenta salvar o formulário de negócio
                try
                {
                    db.Entry(ambiente).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = "Erro ao guardar formuláriodo: " + e;
                    return View(ambiente);
                }

                int id = ambiente.ID_AVALIACAO;
                return RedirectToAction("../Avaliacaos/Resultado/" + id);
            }
            ViewData["Mensagem"] = "Ocorreu um erro ao tentar alterar.";
            return View(ambiente);
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
