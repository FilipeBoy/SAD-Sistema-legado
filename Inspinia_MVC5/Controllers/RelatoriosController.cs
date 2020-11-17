using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inspinia_MVC5.Models;

namespace Inspinia_MVC5.Controllers
{
    public class RelatoriosController : Controller
    {
        private InspiniaContext db = new InspiniaContext();
        // GET: Relatorios
        public ActionResult Index()
        {
            return View();
        }

        //----------------------------------------------------------------
        //Gráfico tipos de usuários
        [HttpPost]
        public JsonResult GraficoDoug1()
        {
            var users = db.Usuarios.Where(x => x.PAPEL != "Admin").ToList();
            int free = 0;
            int pro = 0;
            if (users != null)
            {
                ViewData["Totalmsg"] = "user não é nulo";
                foreach (var item in users)
                {
                    if (item.TIPO_CONTA.Equals("GRATUITA"))
                    {
                        free++;
                    }
                    else if (item.TIPO_CONTA.Equals("PRO"))
                    {
                        pro++;
                    }
                };
            }
            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("Tipo", System.Type.GetType("System.String"));
            dt.Columns.Add("Qtd", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Tipo"] = "GRATUITA";
            dr["Qtd"] = free;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Tipo"] = "PRO";
            dr["Qtd"] = pro;
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

        //----------------------------------------------------------------
        //Gráfico cadastros por mes
        [HttpPost]
        public JsonResult GraficoLine1()
        {
            var users = db.Usuarios.Where(x => x.PAPEL != "Admin").ToList();
            int jan = 0;
            int fev = 0;
            int mar = 0;
            int abr = 0;
            int mai = 0;
            int jun = 0;
            int jul = 0;
            int ago = 0;
            int set = 0;
            int outu = 0;
            int nov = 0;
            int dez = 0;

            if (users != null)
            {
                foreach (var item in users)
                {
                    if (item.DATA.Month == 01 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        jan++;
                    }
                    else if (item.DATA.Month == 02 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        fev++;
                    }
                    else if (item.DATA.Month == 03 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        mar++;
                    }
                    else if (item.DATA.Month == 04 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        abr++;
                    }
                    else if (item.DATA.Month == 05 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        mai++;
                    }
                    else if (item.DATA.Month == 06 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        jun++;
                    }
                    else if (item.DATA.Month == 07 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        jul++;
                    }
                    else if (item.DATA.Month == 08 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        ago++;
                    }
                    else if (item.DATA.Month == 09 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        set++;
                    }
                    else if (item.DATA.Month == 10 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        outu++;
                    }
                    else if (item.DATA.Month == 11 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        nov++;
                    }
                    else if (item.DATA.Month == 12 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        dez++;
                    }
                };
            }

            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("mes", System.Type.GetType("System.String"));
            dt.Columns.Add("Qtd", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["mes"] = "Jan";
            dr["Qtd"] = jan;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Fev";
            dr["Qtd"] = fev;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Mar";
            dr["Qtd"] = mar;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Abr";
            dr["Qtd"] = abr;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Mai";
            dr["Qtd"] = mai;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Jun";
            dr["Qtd"] = jun;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Jul";
            dr["Qtd"] = jul;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Ago";
            dr["Qtd"] = ago;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Set";
            dr["Qtd"] = set;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Out";
            dr["Qtd"] = outu;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Nov";
            dr["Qtd"] = nov;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Dez";
            dr["Qtd"] = dez;
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

        //----------------------------------------------------------------
        //Gráfico avaliações por mes
        [HttpPost]
        public JsonResult GraficoBar1()
        {
            var avaliacoes = db.Avaliacaos.ToList();
            int jan = 0;
            int fev = 0;
            int mar = 0;
            int abr = 0;
            int mai = 0;
            int jun = 0;
            int jul = 0;
            int ago = 0;
            int set = 0;
            int outu = 0;
            int nov = 0;
            int dez = 0;

            if (avaliacoes != null)
            {
                foreach (var item in avaliacoes)
                {
                    if (item.DATA.Month == 01 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        jan++;
                    }
                    else if (item.DATA.Month == 02 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        fev++;
                    }
                    else if (item.DATA.Month == 03 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        mar++;
                    }
                    else if (item.DATA.Month == 04 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        abr++;
                    }
                    else if (item.DATA.Month == 05 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        mai++;
                    }
                    else if (item.DATA.Month == 06 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        jun++;
                    }
                    else if (item.DATA.Month == 07 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        jul++;
                    }
                    else if (item.DATA.Month == 08 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        ago++;
                    }
                    else if (item.DATA.Month == 09 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        set++;
                    }
                    else if (item.DATA.Month == 10 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        outu++;
                    }
                    else if (item.DATA.Month == 11 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        nov++;
                    }
                    else if (item.DATA.Month == 12 & item.DATA.Year == DateTime.Today.Date.Year)
                    {
                        dez++;
                    }
                };
            }
            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("mes", System.Type.GetType("System.String"));
            dt.Columns.Add("Qtd", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["mes"] = "Jan";
            dr["Qtd"] = jan;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Fev";
            dr["Qtd"] = fev;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Mar";
            dr["Qtd"] = mar;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Abr";
            dr["Qtd"] = abr;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Mai";
            dr["Qtd"] = mai;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Jun";
            dr["Qtd"] = jun;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Jul";
            dr["Qtd"] = jul;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Ago";
            dr["Qtd"] = ago;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Set";
            dr["Qtd"] = set;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Out";
            dr["Qtd"] = outu;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Nov";
            dr["Qtd"] = nov;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["mes"] = "Dez";
            dr["Qtd"] = dez;
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

        //----------------------------------------------------------------
        //Gráfico avaliações por status
        [HttpPost]
        public JsonResult GraficoBar2()
        {
            var avaliacoes = db.Avaliacaos.ToList();
            int ini = 0;
            int and = 0;
            int con = 0;

            if (avaliacoes != null)
            {
                foreach (var item in avaliacoes)
                {
                    if (item.STATUS.Equals("Iniciado"))
                    {
                        ini++;
                    }
                    else if (item.STATUS.Equals("Andamento"))
                    {
                        and++;
                    }
                    else if (item.STATUS.Equals("Concluído"))
                    {
                        con++;
                    }
                };
            }

            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("status", System.Type.GetType("System.String"));
            dt.Columns.Add("Qtd", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["status"] = "Iniciado";
            dr["Qtd"] = ini;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["status"] = "Andamento";
            dr["Qtd"] = and;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["status"] = "Concluído";
            dr["Qtd"] = con;
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

        //----------------------------------------------------------------
        //Gráfico de acertos de decisão
        [HttpPost]
        public JsonResult GraficoDoug2()
        {
            var feedbacks = db.Feedbacks.ToList();
            int sim = 0;
            int nao = 0;
            
            if (feedbacks != null)
            {
                foreach (var item in feedbacks)
                {
                    if (item.ESTRATEGIA_ADOTADA != null)
                    {
                        string[] estrategia = item.ESTRATEGIA_ADOTADA.Split(' ');

                        if (item.RESULTADO.ToLower().Contains(estrategia[0].ToLower()))
                        {
                            sim++;
                        }
                        else 
                        {
                            nao++;
                        }
                    }
                };
            }

            List<object> iDados = new List<object>();
            //Criando dados de exemplo
            DataTable dt = new DataTable();
            dt.Columns.Add("Tipo", System.Type.GetType("System.String"));
            dt.Columns.Add("Qtd", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Tipo"] = "Acertos";
            dr["Qtd"] = sim;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Tipo"] = "Erros";
            dr["Qtd"] = nao;
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
    }
}