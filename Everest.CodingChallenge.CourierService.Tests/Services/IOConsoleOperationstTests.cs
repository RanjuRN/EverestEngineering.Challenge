using Everest.CodingChallenge.CourierService.Interfaces;
using Moq;

namespace Everest.CodingChallenge.CourierService.Tests;

public class IOConsoleOperationstTests
{
    IIOServiceOperations ioServiceOperations;
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void CheckReadOperations_Returns_Correct()
    {
        //Arrange
        var ioServiceOperationsMock = new Mock<IIOServiceOperations>();
        ioServiceOperations = ioServiceOperationsMock.Object;
        ioServiceOperationsMock.Setup(io => io.ReadLine()).Returns("Test Input");
        

        //Act
        var result = ioServiceOperations.ReadLine();

        //Assert
        Assert.That(result, Is.EqualTo("Test Input"));

    }
    [Test]
    public void CheckWriteOperations_Called_Once()
    {
        //Arrange
        var ioServiceOperationsMock = new Mock<IIOServiceOperations>();
        ioServiceOperations = ioServiceOperationsMock.Object;
        ioServiceOperationsMock.Setup(io => io.WriteLine(It.IsAny<string>()));
        //Act
        ioServiceOperations.WriteLine("Test Output");
        //Assert
        ioServiceOperationsMock.Verify(io => io.WriteLine(It.IsAny<string>()), Times.Once);
    }
}
