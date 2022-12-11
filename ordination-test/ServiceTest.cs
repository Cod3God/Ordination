namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;
using static shared.Util;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            -2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
        //denne tester på om counten af listen dagligfast stiger med én efter der er givet en ny dosis.
    }
    [TestMethod]
    public void OpretDagligSkæv()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligSkæve().Count());

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            new Dosis[] {
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3)
            }, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligSkæve().Count());
        //denne tester på om counten af listen dagligskæv stiger med én efter der er givet en ny dosis.
    }
   
    [TestMethod]
    public void OpretPN_Gyldig()
    {
        //gyldig test
        PN test1 = new PN(new DateTime(2022, 10, 12), new DateTime(2023, 04, 09), 73, new Laegemiddel("Methotrexat", 0.01, 0.015, 0.02, "Styk"));

        bool givDosis_test1 = test1.givDosis(new Dato { dato = new DateTime(2023, 01, 05).Date });

        Assert.AreEqual(true, givDosis_test1);
        //Starter med at indsætte en gyldighedsperiode for et givent lægemiddel samt antal enheder. Dernæst testes om værdien som er datoen for givDosis ligger indenfor gyldighedsperioden, da denne metode givDosis skal returnere true hvis den ligger indenfor denne. Til sidst testes om outputtet fra metoden givDosis som blev true er equal med true.
    }

    [TestMethod]
    public void OpretPN_Ugyldig()
    {
        //ugyldig test
        PN test2 = new PN(new DateTime(2022, 10, 12), new DateTime(2023, 04, 09), 73, new Laegemiddel("Methotrexat", 0.01, 0.015, 0.02, "Styk"));

        bool givDosis_test2 = test2.givDosis(new Dato { dato = new DateTime(2025, 01, 05).Date });

        Assert.AreEqual(true, givDosis_test2);
        //Starter med at indsætte en gyldighedsperiode for et givent lægemiddel samt antal enheder. Dernæst testes om værdien som er datoen for givDosis ligger indenfor gyldighedsperioden, da denne metode givDosis skal returnere true hvis den ligger indenfor denne. Til sidst testes om outputtet fra metoden givDosis som blev true er equal med true.
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtKodenSmiderEnException()
    {
        // Herunder skal man så kalde noget kode,
        // der smider en exception.

        // Hvis koden _ikke_ smider en exception,
        // så fejler testen.

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
}