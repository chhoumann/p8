using BlazorBLE.Data;

namespace BlazorBLE.Tests
{
    public class KBeaconDataTest
    {
        private readonly byte[] validData = new byte[] { 0x06, 0x17, 0x9c, 0xaa, 0xb5, 0xf3, 0x7e, 0x78, 0x26, 0xd3, 0xcd, 0x82, 0xe3, 0x03, 0xde, 0xcd, 0x5d, 0x3a, 0xde, 0xf4, 0xa9, 0x96, 0xd9, 0x23, 0x16 };
        private readonly Guid expectedUuid = new Guid("787ef3b5-d326-82cd-e303-decd5d3adef4");
        private readonly byte[] expectedCompanyId = new byte[] { 0x06, 0x17 };
        private readonly ushort expectedMajor = 0x96a9;
        private readonly ushort expectedMinor = 0x23d9;
        private readonly sbyte expectedTxPower = 0x16;

        [Fact]
        public void TestConstructor_ValidData_LittleEndian()
        {
            // Act
            var kBeaconData = new KBeaconData(validData);

            // Assert
            Assert.Equal(expectedUuid, kBeaconData.Uuid);
            Assert.Equal(expectedCompanyId, kBeaconData.CompanyId);
            Assert.Equal(expectedMajor, kBeaconData.Major);
            Assert.Equal(expectedMinor, kBeaconData.Minor);
            Assert.Equal(expectedTxPower, kBeaconData.TxPower);
        }

        [Theory]
        //[InlineData(null)] Should check for not null in KBeaconData
        [InlineData(new byte[] { })]
        [InlineData(new byte[] { 0x01, 0x02, 0x03 })]
        public void KBeaconData_InvalidData_ThrowsArgumentException(byte[] generatedData)
        {
            Assert.Throws<ArgumentException>(() => new KBeaconData(generatedData));
        }

        [Fact]
        public void TestConstructor_ValidData_BigEndian()
        {
            // Act
            var kBeaconData = new KBeaconData(validData, false);

            // Assert
            Assert.Equal(expectedUuid, kBeaconData.Uuid);
            Assert.Equal(expectedCompanyId, kBeaconData.CompanyId);
            Assert.Equal(expectedMajor, kBeaconData.Major);
            Assert.Equal(expectedMinor, kBeaconData.Minor);
            Assert.Equal(expectedTxPower, kBeaconData.TxPower);
        }

        [Fact]
        public void KBeaconData_ValidData_CompanyId_LittleEndian_Equals_BigEndian()
        {
            var littleEndianKBeaconData = new KBeaconData(validData, true);
            var bigEndianKBeaconData = new KBeaconData(validData, false);

            Assert.Equal(littleEndianKBeaconData.CompanyId, bigEndianKBeaconData.CompanyId);
        }
    }
}