using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Inspinia_MVC5.Models
{
    public class Usuario
    {
        [Key]
        public int ID_USUARIO { get; set; }

        [Display(Name = " Nome")]
        [Required(ErrorMessage = "Você precisa entrar com o {0}")]
        public string NOME { get; set; }

        [Display(Name = " Sobrenome")]
        [Required(ErrorMessage = "Você precisa entrar com o {0}")]
        public string SOBRENOME { get; set; }

        [Display(Name = " Email")]
        [Required(ErrorMessage = "Você precisa entrar com o {0}")]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Você precisa entrar com a {0}")]
        [DataType(DataType.Password)]
        [Display(Name = " Senha")]
        public string SENHA { get; set; }

        [Required(ErrorMessage = "Você precisa entrar com a {0}")]
        [DataType(DataType.Password)]
        [Display(Name = " Confirmação da Senha")]
        public string CONFSENHA { get; set; }

        [Display(Name = " Papel")]
        public string PAPEL { get; set; }

        [Display(Name = " Tipo de conta")]
        public string TIPO_CONTA { get; set; }

        [Display(Name = "Data Inscrição")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DATA { get; set; }

        
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }

        

    }
}