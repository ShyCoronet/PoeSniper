using PoeTrade;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace PoeSniperTests {
    public class PoeTradeParserTest {
        [Fact]
        public void Parse_with_valid_data() {
            string html = "<div id='search - results - 0'>" +
                            "<table class='search - results' id='search - results - first'>" +
                                "<tbody id='item - container - 0' " +
                                    "class='item item-live - 52cb0d446878d0b48c1fe16c35f58f49' " +
                                    "data-seller='wafssg' data-buyout='500 alteration' data-ign='TestCharacter' " +
                                    "data-league='Ultimatum' data-name='Exalted Orb' data-tab='TradeTab' " +
                                    "data-x='11' data-y='0'>" +
                                "</tbody>" +
                            "</table>" +
                          "</div>";
            var sut = new PoeTradeParser();

            var result = sut.Parse(html).Result.ToArray();

            Assert.True(result.Length == 1);
            Assert.Equal("TestCharacter", result[0].CharacterName);
            Assert.Equal("500 alteration", result[0].Buyout);
            Assert.Equal("Ultimatum", result[0].League);
            Assert.Equal("Exalted Orb", result[0].ItemName);
            Assert.Equal("TradeTab", result[0].StashTabName);
            Assert.Equal("11", result[0].ItemPositionX);
            Assert.Equal("0", result[0].ItemPositionY);
        }

        [Fact]
        public void Parse_with_invalid_data() {
            string html = "<div id='search - results - 0'>" +
                            "<table class='search - results' id='search - results - first'>" +
                                "<tbody";

            var sut = new PoeTradeParser();

            Assert.ThrowsAsync<InvalidDataException>(() => sut.Parse(html));
        }
    }
}
