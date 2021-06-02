using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace E4OVDB
{
    class Szoba
    {

        int balfelsoX;
        int balfelsoY;
        int jobbalsoX;
        int jobbalsoY;

        //tulajdonság

        public int BalFX
        {
            get { return balfelsoX; }
            set { balfelsoX = value; }
        }

        public int BalFY
        {
            get { return balfelsoY; }
            set { balfelsoY = value; }
        }
        public int JobbAlX
        {
            get { return jobbalsoX; }
            set { jobbalsoX = value; }
        }
        public int JobbAlY
        {
            get { return jobbalsoY; }
            set { jobbalsoY = value; }
        }

        public Szoba(int balfelsoX_, int balfelsoY_, int jobbalsoX_, int JobbalsoY_)
        {
            this.balfelsoX = balfelsoX_;
            this.balfelsoY = balfelsoY_;
            this.jobbalsoX = jobbalsoX_;
            this.jobbalsoY = JobbalsoY_;
        }




    }
    class Futtato
    {
        //Metódusok:Feltöltés,Maxkiválasztás,Területkeresés
        //koordináták alapján ütközés keresése
        //majd szoba keresése tömbben,bővítéses ellenörzéssel
        //minimumkiválasztás koordináta alapján, ami megmondja melyik a legközelebbi szoba az üres szobához, mind Y koordináta és X koordináta alapján
        //ha talál egy üres helyet, elkezdi végigfutni onnantól
        //két minimum érték keresés párhuzamosan

        Szoba[] Valosszobak;
        Szoba[] ujszobak = new Szoba[10000];
        int ujszobaindex = 0;
        string[] hazmeret;

        private void Beolvaso()
        {
            StreamReader sr = new StreamReader("Ház.be", Encoding.Default);


            hazmeret = sr.ReadLine().Split(' ');
            Valosszobak = new Szoba[int.Parse(hazmeret[0])];

            int i = 0;
            while (!sr.EndOfStream)
            {
                string[] Szkoordinata = sr.ReadLine().Split(' ');
                Szoba R = new Szoba(int.Parse(Szkoordinata[0]), int.Parse(Szkoordinata[1]), int.Parse(Szkoordinata[2]), int.Parse(Szkoordinata[3]));
                Valosszobak[i] = R;
                i++;
            }
            sr.Close();

        }
        private int MaxTerület()
        {

            int max = ((ujszobak[0].JobbAlX - ujszobak[0].BalFX) + 1) * ((ujszobak[0].JobbAlY - ujszobak[0].BalFY) + 1);

            for (int i = 1; i < ujszobaindex; i++)
            {
                if (max < ((ujszobak[i].JobbAlX - ujszobak[i].BalFX) + 1) * ((ujszobak[i].JobbAlY - ujszobak[i].BalFY) + 1))
                {
                    max = ((ujszobak[i].JobbAlX - ujszobak[i].BalFX) + 1) * ((ujszobak[i].JobbAlY - ujszobak[i].BalFY) + 1);
                }
            }

            return max;

        }
        private void PrintOut()
        {

            StreamWriter sr = new StreamWriter("Ház.ki", false);
            sr.WriteLine(ujszobaindex);
            sr.WriteLine(MaxTerület());
            sr.Close();

        }

        private void Szobafedes(int[,] Mindenszoba, int KezdoX, int KezdoY)
        {
            int k = 0;
            int UtolsoX = 0;
            int UtolsoY = 0;
            bool Break = false;

            if (KezdoX == Mindenszoba.GetLength(1) - 1)
            {
                UtolsoX = KezdoX;
            }
            else
            {
                while (k < Valosszobak.Length && Break == false)
                {
                    if (KezdoX < Valosszobak[k].BalFX - 1 && KezdoY < Valosszobak[k].JobbAlY)
                    {
                        Break = true;
                    }
                    else if (KezdoX < Valosszobak[k].BalFX - 1)
                    {
                        if (KezdoY < Valosszobak[k].BalFY - 1 || KezdoY > Valosszobak[k].BalFY - 1)
                        {
                            k++;
                        }
                        else
                        {
                            Break = true;
                        }
                    }
                    else
                    {
                        k++;
                    }

                }
                if (k >= Valosszobak.Length)
                {
                    UtolsoX = Mindenszoba.GetLength(1) - 1;
                }
                else
                {
                    UtolsoX = Valosszobak[k].BalFX - 2;
                }

            }
            k = 0;
            Break = false;
            if (KezdoY == Mindenszoba.GetLength(0) - 1)
            {
                UtolsoY = KezdoY;
            }

            else
            {
                while (k < Valosszobak.Length && Break == false)
                {
                    if (KezdoY < Valosszobak[k].BalFY - 1 && KezdoX < Valosszobak[k].JobbAlX - 1)
                    {
                        Break = true;
                    }
                    else if (KezdoY < Valosszobak[k].BalFY - 1)
                    {
                        if (KezdoX < Valosszobak[k].BalFX - 1 || KezdoX > Valosszobak[k].BalFX - 1)
                        {
                            k++;
                        }
                        else
                        {
                            Break = true;
                        }
                    }
                    else
                    {
                        k++;
                    }
                }
                if (k >= Valosszobak.Length)
                {
                    UtolsoY = Mindenszoba.GetLength(0) - 1;
                }
                else
                {
                    UtolsoY = Valosszobak[k].BalFY - 2;
                }

            }

            for (int i = KezdoY; i <= UtolsoY; i++)
            {
                for (int j = KezdoX; j <= UtolsoX; j++)
                {
                    Mindenszoba[i, j] = 2;
                }
            }

            Szoba H = new Szoba(KezdoX, KezdoY, UtolsoX, UtolsoY);
            ujszobak[ujszobaindex] = H;
            ujszobaindex++;


        }


        private void Hazfeltoltes()
        {
            //kétdimenziós tömb lértehozása és feltöltése

            //üres hely keresése,ha talál, párhuzamos keresés az ő kezdő koordinátáinál nagyobb a többi közül minimális értékek között ami megadja az alsó koordinátákat


            int[,] osszesszoba = new int[int.Parse(hazmeret[3]) - int.Parse(hazmeret[1]) + 1, int.Parse(hazmeret[4]) - int.Parse(hazmeret[2]) + 1];


            int k = 0;
            while (k < Valosszobak.Length)
            {
                for (int i = Valosszobak[k].BalFY - 1; i < Valosszobak[k].JobbAlY; i++)
                {
                    for (int j = Valosszobak[k].BalFX - 1; j < Valosszobak[k].JobbAlX; j++)
                    {
                        osszesszoba[i, j] = 1;
                    }
                }
                k++;
            }

            Szobakereso(osszesszoba);
            PrintOut();


            string st = "";
            for (int i = 0; i < osszesszoba.GetLength(0); i++)
            {
                for (int j = 0; j < osszesszoba.GetLength(1); j++)
                {
                    st += osszesszoba[i, j];

                }
                st += "\n";
            }


            Console.WriteLine(st);




        }
        private void Szobakereso(int[,] osszesszoba)
        {

            for (int i = 0; i < osszesszoba.GetLength(0); i++)
            {

                for (int j = 0; j < osszesszoba.GetLength(1); j++)
                {
                    if (osszesszoba[i, j] == 0)
                    {
                        Szobafedes(osszesszoba, j, i);
                    }
                }
            }
        }
        public void ProgramRun()
        {
            Beolvaso();
            Hazfeltoltes();
        }


    }
    class Program
    {
        static void Main(string[] args)
        {

            Futtato futas = new Futtato();

            futas.ProgramRun();
            Console.ReadKey();


        }
    }
}
