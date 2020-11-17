
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

    public class TecnicoesController : Controller
    {
        private InspiniaContext db = new InspiniaContext();


        // GET: /Tecnicoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Tecnicoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_AVTECNICO,ID_AVALIACAO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7,RESPOSTA8,RESPOSTA9,RESPOSTA10,RESPOSTA11,RESPOSTA12,RESPOSTA13,RESPOSTA14,RESPOSTA15,RESPOSTA16,RESPOSTA17,RESPOSTA18,RESPOSTA19,RESPOSTA20,TEC_NOTA_FINAL")] Tecnico tecnico)
        {
            Avaliacao avaliacao = TempData["Avaliacao"] as Avaliacao;
            TempData["Avaliacao"] = avaliacao;
            tecnico.ID_AVALIACAO = avaliacao.ID_AVALIACAO;
            tecnico.TEC_NOTA_FINAL = (tecnico.RESPOSTA1 + tecnico.RESPOSTA2 + tecnico.RESPOSTA3 + tecnico.RESPOSTA4 + tecnico.RESPOSTA5
                     + tecnico.RESPOSTA6 + tecnico.RESPOSTA7 + tecnico.RESPOSTA9 + tecnico.RESPOSTA10
                      + tecnico.RESPOSTA11 + tecnico.RESPOSTA12 + tecnico.RESPOSTA13 + tecnico.RESPOSTA14 + tecnico.RESPOSTA15
                       + tecnico.RESPOSTA16 + tecnico.RESPOSTA17 +tecnico.RESPOSTA18 + tecnico.RESPOSTA19 + tecnico.RESPOSTA20) / 19;
            //tenta salvar o formulário de negócio
            try
            {
                db.Tecnicoes.Add(tecnico);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Message"] = "Erro ao guardar formuláriodo: " + e;
                return View(tecnico);
            }
            switch (Request.Form["Submit"])
            {
                case "sair":
                    return RedirectToAction("../Avaliacaos/IndexUsuario");

                case "continuar":
                    return RedirectToAction("../Ambientes/Create");
            }

            ViewData["Mensagem"] = "Nada deu certo";
            return View();
        }

        // GET: /Tecnicoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            Tecnico tecnico = db.Tecnicoes.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();

            if (tecnico == null)
            {
                return HttpNotFound();
            }

            return View(tecnico);
        }

        // POST: /Tecnicoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_AVTECNICO,ID_AVALIACAO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7,RESPOSTA8,RESPOSTA9,RESPOSTA10,RESPOSTA11,RESPOSTA12,RESPOSTA13,RESPOSTA14,RESPOSTA15,RESPOSTA16,RESPOSTA17,RESPOSTA18,RESPOSTA19,RESPOSTA20,TEC_NOTA_FINAL")] Tecnico tecnico)
        {
            if (ModelState.IsValid)
            {
                //calculo da nota final
                tecnico.TEC_NOTA_FINAL = (tecnico.RESPOSTA1 + tecnico.RESPOSTA2 + tecnico.RESPOSTA3 + tecnico.RESPOSTA4 + tecnico.RESPOSTA5
                     + tecnico.RESPOSTA6 + tecnico.RESPOSTA7 + tecnico.RESPOSTA9 + tecnico.RESPOSTA10
                      + tecnico.RESPOSTA11 + tecnico.RESPOSTA12 + tecnico.RESPOSTA13 + tecnico.RESPOSTA14 + tecnico.RESPOSTA15
                       + tecnico.RESPOSTA16 + tecnico.RESPOSTA17 +tecnico.RESPOSTA18 + tecnico.RESPOSTA19 + tecnico.RESPOSTA20) / 19;
                //tenta alterar o formulário de negócio
                try
                {
                    db.Entry(tecnico).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["Message"] = "Erro ao guardar formuláriodo: " + e;
                    return View(tecnico);
                }

                int id = tecnico.ID_AVALIACAO;
                switch (Request.Form["Submit"])
                {
                    case "sair":
                        return RedirectToAction("../Avaliacaos/Resultado/" + id);

                    case "continuar":
                        return RedirectToAction("../Ambientes/Edit/" + id);
                }
            }
            ViewData["Mensagem"] = "Nada deu certo";
            return View();
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
