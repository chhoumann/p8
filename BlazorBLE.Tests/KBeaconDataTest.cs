using BlazorBLE.Data;

namespace BlazorBLE.Tests;

public class KBeaconDataTest
{
    private readonly byte[] testData = new byte[] { 0x06, 0x17, 0x9C, 0xAA, 0xB5, 0xF3, 0x7E, 0x78, 0x26, 0xD3, 0xCD, 0x82, 0xE3, 0x03, 0xDE, 0xCD, 0x5D, 0x3A, 0xDE, 0xF4, 0xA9, 0x96, 0xD9, 0x23, 0x16 };
    private readonly Guid expectedUuid = new Guid("787ef3b5-d326-82cd-e303-decd5d3adef4");
    private readonly byte[] expectedCompanyId = new byte[] { 0x06, 0x17 };
    private readonly ushort expectedMajor = 0x96A9;
    private readonly ushort expectedMinor = 0x23D9;
    private readonly sbyte expectedTxPower = 0x16;

    [Fact]
    public void TestConstructor_testData_LittleEndian()
    {
        KBeaconData kBeaconData = new KBeaconData(testData);
            
        Assert.Equal(expectedUuid, kBeaconData.Uuid);
        Assert.Equal(expectedCompanyId, kBeaconData.CompanyId);
        Assert.Equal(expectedMajor, kBeaconData.Major);
        Assert.Equal(expectedMinor, kBeaconData.Minor);
        Assert.Equal(expectedTxPower, kBeaconData.TxPower);
    }

    [Fact]
    public void TestConstructor_testData_BigEndian()
    {
        KBeaconData kBeaconData = new KBeaconData(testData, false);
            
        Assert.Equal(expectedUuid, kBeaconData.Uuid);
        Assert.Equal(expectedCompanyId, kBeaconData.CompanyId);
        Assert.Equal(expectedMajor, kBeaconData.Major);
        Assert.Equal(expectedMinor, kBeaconData.Minor);
        Assert.Equal(expectedTxPower, kBeaconData.TxPower);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[] { })]
    [InlineData(new byte[] { 0x01, 0x02, 0x03 })]
    public void KBeaconData_InvalidTestData_ThrowsArgumentException(byte[] generatedData)
    {
        Assert.Throws<ArgumentException>(() => new KBeaconData(generatedData));
    }


    [Fact]
    public void KBeaconData_testData_CompanyId_LittleEndian_Equals_BigEndian()
    {
        KBeaconData littleEndianKBeaconData = new KBeaconData(testData, true);
        KBeaconData bigEndianKBeaconData = new KBeaconData(testData, false);

        Assert.Equal(littleEndianKBeaconData.CompanyId, bigEndianKBeaconData.CompanyId);
    }
}
