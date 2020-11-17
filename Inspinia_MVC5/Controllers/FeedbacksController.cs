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

    public class FeedbacksController : Controller
    {
        private InspiniaContext db = new InspiniaContext();

        // GET: /Feedbacks/Index
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.Avaliacao).Where(x=>x.ESTRATEGIA_ADOTADA!=null);
            return View(feedbacks.ToList());
        }

        // GET: /Feedbacks/Validacao
        public ActionResult Validacao()
        {
            return View();
        }

        // GET: /Feedbacks/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            if (avaliacao != null)
            {
                Feedback feedback = db.Feedbacks.Where(x => x.ID_AVALIACAO.Equals(avaliacao.ID_AVALIACAO)).FirstOrDefault();
                if (feedback == null)
                {
                    return RedirectToAction("Create/" + avaliacao.ID_AVALIACAO);
                }
                else
                {
                    if (avaliacao.RESULTADO != feedback.RESULTADO)
                    {
                        feedback.RESULTADO = avaliacao.RESULTADO;
                        db.Entry(feedback).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    ViewData["Mensagem"] = TempData["Mensagem"];
                    string[] estrategia = feedback.ESTRATEGIA_ADOTADA.Split(' ');

                    if (feedback.RESULTADO.ToLower().Contains(estrategia[0].ToLower()))
                    {
                        ViewData["FeedComp"] = 0;
                    }
                    else
                    {
                        ViewData["FeedComp"] = 1;
                    }
                    return View(feedback);
                }

            }
            return RedirectToAction("../Avaliacaos/IndexUsuario");
        }

        // GET: /Feedbacks/Create
        public ActionResult Create(int id)
        {
            Avaliacao avaliacao = db.Avaliacaos.Find(id);
            Feedback feedback = new Feedback();
            feedback.ID_AVALIACAO = avaliacao.ID_AVALIACAO;
            feedback.RESULTADO = avaliacao.RESULTADO;
            return View(feedback);
        }

        // POST: /Feedbacks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_FEEDBACK,ID_AVALIACAO,ESTRATEGIA_ADOTADA,RESULTADO,COMENTARIO")] Feedback feedback)
        {

            try
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Mensagem"] = "Erro ao guardar formulário: " + e;
                return View(feedback);
            }
            string[] estrategia = feedback.ESTRATEGIA_ADOTADA.Split(' ');

            if (feedback.RESULTADO.ToLower().Contains(estrategia[0].ToLower()))
            {
                return RedirectToAction("../Avaliacaos/IndexUsuario");
            }
            else
            {
                return RedirectToAction("CreateComplementar/" + feedback.ID_FEEDBACK);
            }
            
        }

        // GET: /Feedbacks complementar/Create
        public ActionResult CreateComplementar(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            return View(feedback);
        }


        // POST /Feedbacks complementar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComplementar([Bind(Include = "ID_FEEDBACK,ID_AVALIACAO,ESTRATEGIA_ADOTADA,RESULTADO,COMENTARIO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7")] Feedback feedback)
        {

            try
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Mensagem"] = "Erro ao guardar formuláriodo: " + e;
                return View(feedback);
            }

            return RedirectToAction("../Avaliacaos/IndexUsuario");

        }

        // GET: /Feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: /Feedbacks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_FEEDBACK,ID_AVALIACAO,ESTRATEGIA_ADOTADA,RESULTADO,COMENTARIO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7")] Feedback feedback)
        {

            try
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Mensagem"] = "Erro ao guardar formuláriodo: " + e;
                return View(feedback);
            }

            string[] estrategia = feedback.ESTRATEGIA_ADOTADA.Split(' ');

            if (feedback.RESULTADO.ToLower().Contains(estrategia[0].ToLower()))
            {
                return RedirectToAction("Details/" + feedback.ID_AVALIACAO);
            }
            else
            {
                return RedirectToAction("CreateComplementar/" + feedback.ID_FEEDBACK);
            }


        }

        // GET: /FeedbacksComplementares/Edit/5
        public ActionResult EditComplementar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: /Feedbacks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComplementar([Bind(Include = "ID_FEEDBACK,ID_AVALIACAO,ESTRATEGIA_ADOTADA,RESULTADO,COMENTARIO,RESPOSTA1,RESPOSTA2,RESPOSTA3,RESPOSTA4,RESPOSTA5,RESPOSTA6,RESPOSTA7")] Feedback feedback)
        {

            try
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["Mensagem"] = "Erro ao guardar formuláriodo: " + e;
                return View(feedback);
            }

            return RedirectToAction("Details/" + feedback.ID_AVALIACAO);
        }

        // GET: /FeedbacksComplementar/Details
        public ActionResult DetailsComplementar(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);

            return View(feedback);
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
