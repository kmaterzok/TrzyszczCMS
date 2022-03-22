using NUnit.Framework;
using TrzyszczCMS.Core.Server.Models.Settings;
using Microsoft.Extensions.Options;

namespace TrzyszczCMS.Core.Server.Services.Implementation.Tests
{
    public class CryptoServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_verifying_password_for_argon2()
        {
            string password = "Testing123$";
            IOptions<CryptoSettings> options = Options.Create(new CryptoSettings()
            {
                Argon2Password = new Argon2Settings()
                {
                    Iterations = 16,
                    MemoryCost = 64,
                    Parallelism = 2
                },
                TokenHashIterations = 5000
            });
            var service = new CryptoService(options);

            var passData = service.GenerateHashWithSalt(password);
            
            Assert.True(service.PasswordValid(passData.HashedPassword,
                                              passData.PasswordDependentSalt,
                                              password,
                                              options.Value.Argon2Password.Parallelism,
                                              options.Value.Argon2Password.Iterations,
                                              options.Value.Argon2Password.MemoryCost),
                        "The password IS valid");

            Assert.False(service.PasswordValid(passData.HashedPassword,
                                               passData.PasswordDependentSalt,
                                               "testing123$",
                                               options.Value.Argon2Password.Parallelism,
                                               options.Value.Argon2Password.Iterations,
                                               options.Value.Argon2Password.MemoryCost),
                        "The password is NOT valid");
        }

    }
}