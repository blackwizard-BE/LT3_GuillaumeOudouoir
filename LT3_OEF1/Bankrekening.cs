using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT3_OEF1
{
    internal class Bankrekening
    {

        public string Name { get; set; }
        public string Adres { get; set; }
        public int Postcode { get; set; }
        public string Woonplaats { get; set; }
        public double Saldo { get; set; }
        public string RekeningNummer { get; set; }


        public Bankrekening()
        {
            
        }
        public double Storting(double geld)
        {
            Saldo +=geld;

            return Saldo;


        }
        public double Ophaling(double geld)
        {
            Saldo -= geld;

            return Saldo;


        }
        
    }
}
