using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;


namespace paycoreodev2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private List<Staff> list;

        DateTime minDate = new DateTime(1945, 11, 11); //doğum tarihi kontrolü 1945
        DateTime maxDate = new DateTime(2002, 10, 10); //doğum tarihi kontrolü 2002

        public StaffController()
        {
            #region Manuel oluşturulmuş statik liste

            this.list = new List<Staff>();

            list.Add(new Staff { Id = 1, Name = "Birkan", LastName = "Tunçer", DateOfBirth = DateTime.ParseExact("02/05/1987", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "birkan@gmail.com", PhoneNumber = "123456789", Salary = 3000 });
            list.Add(new Staff { Id = 2, Name = "Volkan", LastName = "Tunçer", DateOfBirth = DateTime.ParseExact("19/01/1987", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "volkan@gmail.com", PhoneNumber = "987654321", Salary = 5000 });
            list.Add(new Staff { Id = 3, Name = "Ahmet", LastName = "Yıldız", DateOfBirth = DateTime.ParseExact("10/01/1965", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "ahmet@hotmail.com", PhoneNumber = "123498765", Salary = 7000 });
            list.Add(new Staff { Id = 4, Name = "Mehmet", LastName = "Yılmaz", DateOfBirth = DateTime.ParseExact("02/05/1975", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "mehmet@gmail.com", PhoneNumber = "4235235", Salary = 4000 });
            list.Add(new Staff { Id = 5, Name = "Serkan", LastName = "Gürkan", DateOfBirth = DateTime.ParseExact("19/01/1949", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "serkan@gmail.com", PhoneNumber = "7723722", Salary = 6000 });
            list.Add(new Staff { Id = 6, Name = "Ayşe", LastName = "Erden", DateOfBirth = DateTime.ParseExact("10/01/1992", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "ayse@hotmail.com", PhoneNumber = "1238765", Salary = 7500 });
            list.Add(new Staff { Id = 7, Name = "Fatma", LastName = "Yıldızer", DateOfBirth = DateTime.ParseExact("02/05/1991", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "fatma@gmail.com", PhoneNumber = "8643452", Salary = 3200 });
            list.Add(new Staff { Id = 8, Name = "Yeşim", LastName = "Çoban", DateOfBirth = DateTime.ParseExact("19/01/1990", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "yesim@gmail.com", PhoneNumber = "2357853", Salary = 5700 });
            list.Add(new Staff { Id = 9, Name = "Mahmut", LastName = "Yıldız", DateOfBirth = DateTime.ParseExact("10/01/2000", "dd/M/yyyy", CultureInfo.InvariantCulture), Email = "mahmut@hotmail.com", PhoneNumber = "5668541", Salary = 7100 });

            #endregion

        }

        
        [HttpGet]
        public List<Staff> Get() // Get methodu ile tüm listeyi tek seferde döndürüyorum
        {
            return list;
        }


        [HttpGet("GetByIdQuery")]
        public Staff GetById([FromQuery]int id) // ID üzerinden listedeki elemanı yakalama
        {
            var selected_Staff = list.Where(x => x.Id == id).FirstOrDefault(); // ID eşleşmesi
            return selected_Staff;
        }


        
        [HttpPost]
        public ActionResult Post([FromBody]Staff staff)
        {
            // ↓ Regex kontrolleri ↓

            #region Ad Soyad Kontrolü
            if (!staff.Name.All(char.IsLetter) || !staff.LastName.All(char.IsLetter)) // İsim soyisim kısımlarının harflerden oluşması için yapılan kontrol.
            {
                return BadRequest("Ad ve Soyad harflerden oluşmalı.");
            }

            #endregion

            #region Doğum Tarihi Kontrolü
            if (staff.DateOfBirth < minDate) // Girilen tarihin 1945 ten daha eski olmaması için yapılan kontrol.
            {
                return BadRequest("Girilen tarih çok eski.");
            }
            if (staff.DateOfBirth > maxDate) // Girilen tarihin 2002 den daha yeni olmaması için yapılan kontrol.
            {
                return BadRequest("Girilen tarih çok yeni.");
            }
            #endregion

            #region Telefon kontrolü
            string phoneNumber = "";
            if (staff.PhoneNumber[0] != '+' || staff.PhoneNumber[1] != '9' || staff.PhoneNumber[2] != '0')
            {
                return BadRequest("Lütfen cep telefonu numarasının başına +90 yazarak girin"); // Telefon numarasının başına +90 yazılması gerektiği için yapılan kontrol.
            }
            else
            {

                for (int i = 0; i < staff.PhoneNumber.Length; i++) // Telefon numarasının çıktı oalrak daha güzel görünmesi için
                {
                    if (i >= 2)
                    {
                        phoneNumber += staff.PhoneNumber[i];
                    }
                }
            }

            staff.PhoneNumber = phoneNumber;
            if (!staff.PhoneNumber.All(char.IsDigit))
            {
                return BadRequest("Telefon numarası, başındaki + haricinde sayılardan oluşmalı");
            }
            #endregion

            #region Email kontrolü

            if (staff.Email.Any(char.IsDigit) || staff.Email.Any(char.IsSymbol)) // Email adresinin sayı veya sembol almaması için yapılan kontrol.
            {
                return BadRequest("Email adresi sayı veya sembol alamaz.");
            }

            string mail = "";
            
            foreach(var ch in staff.Email) // Punctuation kontrolü, e mail içerisinde sadece @ ve . karakterlerine izin veriyorum.
            {
                if(ch.Equals('.') || ch.Equals('@'))
                {
                    
                } 
                else
                {
                    mail += ch;
                }
            }
            if (mail.Any(char.IsPunctuation))
            {
                return BadRequest("Email adresi sayı veya sembol alamaz.");
            }

            #endregion
            
            list.Add(staff); // Girilen veri hem regex hem de fluent validasyonlarını geçerse
            return Ok(list); // Postman üzerinden kontrol edilirse yapılan işlem sonrasında yeni verinin eklendiğinin kontrol edilmesi ve 200 döndürülmesi.
        }
        

        [HttpPut]
        public ActionResult Put([FromBody] Staff staff)
        {
            
            var editStaff = list.FirstOrDefault(x => x.Id == staff.Id); // ID eşleşmesi

            // ↓ Regex kontrolleri ↓

            #region Ad Soyad Kontrolü
            if (!staff.Name.All(char.IsLetter) || !staff.LastName.All(char.IsLetter)) // İsim soyisim kısımlarının harflerden oluşması için yapılan kontrol.
            {
                return BadRequest("Ad ve Soyad harflerden oluşmalı.");
            }

            #endregion

            #region Doğum Tarihi Kontrolü
            if (staff.DateOfBirth < minDate) // Girilen tarihin 1945 ten daha eski olmaması için yapılan kontrol.
            {
                return BadRequest("Girilen tarih çok eski.");
            }
            if (staff.DateOfBirth > maxDate) // Girilen tarihin 2002 den daha yeni olmaması için yapılan kontrol.
            {
                return BadRequest("Girilen tarih çok yeni.");
            }
            #endregion

            #region Telefon kontrolü
            
            string phoneNumber = "";
            if (staff.PhoneNumber[0] != '+' || staff.PhoneNumber[1] != '9' || staff.PhoneNumber[2] != '0')
            {
                return BadRequest("Lütfen cep telefonu numarasının başına +90 yazarak girin"); // Telefon numarasının başına +90 yazılması gerektiği için yapılan kontrol.
            }
            else
            {

                for (int i = 0; i < staff.PhoneNumber.Length; i++) // Telefon numarasının çıktı oalrak daha güzel görünmesi için
                {
                    if (i >= 2)
                    {
                        phoneNumber += staff.PhoneNumber[i];
                    }
                }
            }

            staff.PhoneNumber = phoneNumber;
            if (!staff.PhoneNumber.All(char.IsDigit))
            {
                return BadRequest("Telefon numarası, başındaki + haricinde sayılardan oluşmalı");
            }

            #endregion

            #region Email kontrolü

            if (staff.Email.Any(char.IsDigit) || staff.Email.Any(char.IsSymbol)) // Email adresinin sayı veya sembol almaması için yapılan kontrol.
            {
                return BadRequest("Email adresi sayı veya sembol alamaz.");
            }
            string mail = "";

            foreach (var ch in staff.Email) // Punctuation kontrolü, e mail içerisinde sadece @ ve . karakterlerine izin veriyorum.
            {
                if (ch.Equals('.') || ch.Equals('@'))
                {

                }
                else
                {
                    mail += ch;
                }
            }
            if (mail.Any(char.IsPunctuation))
            {
                return BadRequest("Email adresi sayı veya sembol alamaz.");
            }

            #endregion

            // ↓ ID nin eşleşmesi durumunda tüm değerler yeni girilen değerler ile değiştiriliyor.

            editStaff.Name = staff.Name;
            editStaff.LastName = staff.LastName;
            editStaff.DateOfBirth = staff.DateOfBirth;
            editStaff.Email = staff.Email;
            editStaff.PhoneNumber = staff.PhoneNumber;
            editStaff.Salary = staff.Salary;

            list.Add(staff); // Değiştirilen verilerin aktarılması.
            return Ok(list); // İşlem sonunda listeyi kontrol amaçlı döndürüyorum. 
        }

        
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var deleteStaff = list.FirstOrDefault(x => x.Id == id);
            list.Remove(deleteStaff); // Eşleşen eleman listeden çıkartılıyor.

            return Ok(list); // kontrol amaçlı liste geri döndürülüyor.
        }
    }
}
