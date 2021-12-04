using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT3_OEF2
{
    internal class Bankrekening
    {//voegt alle waarden toe 

        public string Name { get; set; }
        public string Adres { get; set; }
        public int Postcode { get; set; }
        public string Woonplaats { get; set; }
        public double Saldo { get; set; }
        public string RekeningNummer { get; set; }


        public Bankrekening()
        {
            
        }
        public double Storting(double geld)//voegt het geld bij het saldo toe
        {
            Saldo +=geld;

            return Saldo;


        }
        public double Ophaling(double geld)//neemt het geld uit het saldo
        {
            Saldo -= geld;

            return Saldo;


        }
        
    }
}
