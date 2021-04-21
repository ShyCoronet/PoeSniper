namespace Models {
    public class TradeOffer {
        public string CharacterName { get; }
        public string ItemName { get; }
        public string Buyout { get; }
        public string League { get; }
        public string StashTabName { get; }
        public string ItemPositionX { get; }
        public string ItemPositionY { get; }

        public TradeOffer(string characterName, string itemName,
            string buyout, string league, string tabname, string x, string y) {
            CharacterName = characterName;
            ItemName = itemName;
            Buyout = buyout;
            League = league;
            StashTabName = tabname;
            ItemPositionX = x;
            ItemPositionY = y;
        }

        public override string ToString() {
            return $"@{CharacterName} Hi, I would like to buy your {ItemName} " +
                $"listed for {Buyout} in {League} (stash tab \"{StashTabName}\"; " +
                $"position: left {ItemPositionX}, top {ItemPositionY})";
        }
    }
}
