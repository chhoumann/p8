using BlazorBLE.Data;

namespace BlazorBLE.Tests
{
    public class KBeaconDataTest
    {

        [Fact]
        public void TestConstructor_ValidData_LittleEndian()
        {
            // Arrange
            byte[] data = new byte[] { 0x06, 0x17, 0x9c, 0xaa, 0xb5, 0xf3, 0x7e, 0x78, 0x26, 0xd3, 0xcd, 0x82, 0xe3, 0x03, 0xde, 0xcd, 0x5d, 0x3a, 0xde, 0xf4, 0xa9, 0x96, 0xd9, 0x23, 0x16 };

            Guid expectedUuid = new Guid("787ef3b5-d326-82cd-e303-decd5d3adef4");
            byte[] expectedCompanyId = new byte[] { 0x06, 0x17 };
            ushort expectedMajor = 0x96a9;
            ushort expectedMinor = 0x23d9;
            sbyte expectedTxPower = 0x16;

            // Act
            var KBeaconData = new KBeaconData(data);


            // Assert
            Assert.Equal(expectedUuid, KBeaconData.Uuid);
            Assert.Equal(expectedCompanyId, KBeaconData.CompanyId);
            Assert.Equal(expectedMajor, KBeaconData.Major);
            Assert.Equal(expectedMinor, KBeaconData.Minor);
            Assert.Equal(expectedTxPower, KBeaconData.TxPower);
        }

        [Fact]
        public void TestConstructor_ValidData_BigEndian()
        {

        }
    }
}