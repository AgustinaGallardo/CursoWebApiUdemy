using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El {0} es requerido para ingresar un nuevo autor")] // Define al nombre como requerido
        [StringLength(maximumLength:5,ErrorMessage = "El campo {0} no puede tener mas de {1} letras")]
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        //[Range(18,120)] //valida q la edad tenga un rango de 18 a 120 años
        //[NotMapped]
        //public int Edad { get; set; }
        //[CreditCard] //Valida que la tarjeta tenga un numero valido
        //[NotMapped]
        //public string TarjetaCredito { get; set; }
        //[Url]
        //[NotMapped]
        //public string URL { get; set; }
        public List<Libro> Libros { get; set; }
        public int Menor { get; set; }
        public int Mayor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           if(!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra no es mayuscula",
                        new string[] { nameof(Nombre) });
                }
            }


            if (Menor > Mayor)
            {
                yield return new ValidationResult("este valor no puede ser mas grande que el mayor",
                    new string[] {nameof(Menor)});
            }


        }

       
           


    }
}
