namespace ordination_test;

using shared.Model;

[TestClass]
public class PatientTest
{

    [TestMethod]
    public void PatientHasName()
    {
        string cpr = "160563-1234";
        string navn = "John";
        double vægt = 83;
        
        Patient patient = new Patient(cpr, navn, vægt);
        Assert.AreEqual(navn, patient.navn);
    }


    [TestMethod]
    public void TestDerAltidFejler()
    {
        string cpr = "160563-1234";
        string navn = "John";
        double vægt = 83;

        Patient patient = new Patient(cpr, navn, vægt);
        Assert.AreEqual("Egon", patient.navn);
    }




    //Mathias' test
    [TestMethod]
    public void MathiasVægt()
    {
        string cpr = "192939-3837";
        string navn = "Mathias";
        double vægt = 4000;

        Patient patient = new Patient(cpr, navn, vægt);
            Assert.AreEqual(4000, patient.vaegt);
    }
    

}


