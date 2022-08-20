using FluentValidation;
namespace paycoreodev2
{
    public class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            // Diğer kontroller Regex ile StaffController sayfası içerisinde yapılmakta.

            RuleFor(s => s.Id) // ID için yapılan kontrol. Boş olamaz.
                .NotEmpty()
                .NotNull();
            RuleFor(s => s.Name) // İsim için yapılan kontrol. Boş olamaz, 2 ile 120 arasında karakter alabilir.
                .NotEmpty()
                .NotNull()                
                .Length(2, 120);
            RuleFor(s => s.LastName) // Soyisim için yapılan kontrol. Boş olamaz, 2 ile 120 arasında karakter alabilir.
                .NotEmpty()
                .NotNull()
                .Length(2, 120);
            RuleFor(s => s.Email) // Email için yapılan kontrol. Boş olamaz, Email tipinde olmalı.
                .NotEmpty()
                .NotNull()
                .EmailAddress();
            RuleFor(s => s.Salary) // Maaş için yapılan kontrol. Boş olamaz, 2000 ile 9000 arasında olmalı.
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(2000)
                .LessThanOrEqualTo(9000);

            RuleFor(s => s.PhoneNumber) // Telefon numarası için yapılan kontrol. Boş olamaz, 5 ile 16 arasında karakter içermeli.
                .NotEmpty()
                .NotNull()
                .Length(5, 16);
            RuleFor(s => s.DateOfBirth) // Doğum tarihi için yapılan kontrol. Boş olamaz.
                .NotEmpty()
                .NotNull();

        }
    }
}
