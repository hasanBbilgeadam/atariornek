using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {




            SuperMario mario = new SuperMario();
            Bomberman bomberman = new Bomberman();


            Atari atari = Atari.GetInstance();


            atari.OyunEkle(mario);
            atari.OyunEkle(bomberman);



            atari.OyunListele();
            Boşluk();

            atari.SeçVeBaşlat();

            atari.OyunDurdur();


            Boşluk();

            atari.OyunListele();

            Boşluk();




            atari.SeçVeBaşlat();

            atari.OyunDurdur();



            atari.OyunListele();
            Boşluk();




        }

        private static void Boşluk()
        {
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine();
        }
    }




    public enum Tür
    {
        boş,
        yarış,
        dövüş,
        strateji

    }
    public abstract class Oyun
    {


        public string OyunKayıtVerisi { get { return _OyunKayıtVerisi; } }
        private string _OyunKayıtVerisi;
        private string _TempData;

        public string OyunAdı
        {
            get { return _OyunAdı; }
            set
            {

                _OyunAdı = _OyunAdı == null ? value : _OyunAdı;

            }
        }

        string _OyunAdı;
        public Tür Tür
        {
            get { return _Tür; }
            set
            {
                _Tür = _Tür == Tür.boş ? value : _Tür;
            }
        }

        Tür _Tür = Tür.boş;
        public TimeSpan OyunSüresi { get { return _OyunSüresi; } }
        private TimeSpan _OyunSüresi;
        private bool _Status = false;
        private DateTime StartDate;


        public void OyunBaşlat()
        {
            if (_Status) { Console.WriteLine("oyun zaten başladı... "); return; }

            Console.WriteLine("oyun başladı");
            _Status = true;
            StartDate = DateTime.Now;
        }
        public void OyunDurdur()
        {
            if (_Status)
            {
                var time = DateTime.Now - StartDate;
                _OyunSüresi += time;
                _Status = false;
                return;
            }

            Console.WriteLine("başlatılmamış oyun durdurulamaz");
        }
        public void Oyna()
        {
            string data = "";
            while (data !="exit")
            {
                
              
                Console.WriteLine("oyun hareketi yapınız");
                data = Console.ReadLine();
                if (data == "exit") { break; }
                _TempData += data;

            }


        }
        public void Kayıt()
        {
            _OyunKayıtVerisi += _TempData;

            _TempData = "";
        }

        public string OyunAdını() => OyunAdı;

    }



    public class SuperMario : Oyun
    {
        public SuperMario()
        {
            this.Tür = Tür.yarış;
            this.OyunAdı = "süper mario";
        }
    }

    public class Bomberman : Oyun
    {
        public Bomberman()
        {
            this.Tür = Tür.yarış;
            this.OyunAdı = "Bomberman";
        }
    }


    public class Atari
    {

        private Atari()
        {
            Oyunlar = new();
        }

        private Oyun currentGame;
        private static Atari _atari;
        private List<Oyun> Oyunlar { get; set; }
        public static Atari GetInstance()
        {
            if (_atari == null) { return new Atari(); }
            return _atari;
        }
        public void OyunListele(int time=0)
        {
            if (time == 0)
            {
                Oyunlar.ForEach(x =>
                {
                    Console.WriteLine(x.OyunAdı + " " + x.OyunSüresi.TotalSeconds);
                });

                return;
            }

            Oyunlar.OrderByDescending(x=>x.OyunSüresi).ToList().ForEach(x =>
            {
                Console.WriteLine(x.OyunAdı + " " + x.OyunSüresi.TotalSeconds);

            });

        }
        public void OyunListele(Tür t)
        {
            Oyunlar.Where(x => x.Tür == t).ToList().ForEach(x =>
            {
                Console.WriteLine(Enum.GetName(typeof(Tür),x.Tür)+ " " + x.OyunAdı + " " + x.OyunSüresi.TotalSeconds);
            });
        }
        public void SeçVeBaşlat()
        {

            Oyunlar.ForEach(x =>
            {
                Console.WriteLine(x.OyunAdı + " " + x.OyunSüresi.TotalSeconds);
            });


            Console.WriteLine("başlatmak istediğin oyunun adını yaz");

            var oyun = Console.ReadLine();

             currentGame = Oyunlar.Where(x => x.OyunAdı == oyun).FirstOrDefault();

            if(currentGame == null) { Console.WriteLine("oyun bulunmadı"); return; }

            currentGame.OyunBaşlat();
            currentGame.Oyna();


        }
        public void OyunDurdur()
        {
            if (currentGame == null) { Console.WriteLine("hata"); return; }
            currentGame.OyunDurdur();
            currentGame.Kayıt();

            Console.WriteLine(currentGame.OyunAdı+"   =>  oyundaki kayıtlı verileriniz");

            Console.WriteLine(currentGame.OyunKayıtVerisi);
        }
        public void OyunEkle(Oyun oyun)
        {
            Oyunlar.Add(oyun);
        }
        public void OyunÇıkar(string name)
        {
            var data =  Oyunlar.Where(x => x.OyunAdı == name).FirstOrDefault();
            if (data !=null)
            {
                Oyunlar.Remove(data);
            }
            else { Console.WriteLine("oyun bulunamadı"); }
        }
       
    }


}
