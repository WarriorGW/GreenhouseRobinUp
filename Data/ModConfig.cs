namespace TheWarriorGW.GreenhouseRobinUp.Data
{
    public enum IDMaterials
    {
        Hardwood = 709,
        Clay = 330,
        IronBar = 335,
        GoldBar = 336,
        IridiumBar = 337
    }
    class ModConfig
    {
        public Dictionary<int, SingleUpgrade> UpgradeMaterials = new()
        {

            [1] = new SingleUpgrade(200000,
                (new MaterialSlot(IDMaterials.Clay, 200)),
                (new MaterialSlot(IDMaterials.IronBar, 50)),
                (new MaterialSlot(IDMaterials.Hardwood, 20)),
                3),
            [2] = new SingleUpgrade(500000,
                (new MaterialSlot(IDMaterials.Clay, 500)),
                (new MaterialSlot(IDMaterials.GoldBar, 100)),
                (new MaterialSlot(IDMaterials.IridiumBar, 50)),
                3)

        };
    }

    class SingleUpgrade
    {
        public int Gold { get; set; }
        public MaterialSlot Slot1 { get; set; }
        public MaterialSlot Slot2 { get; set; }
        public MaterialSlot Slot3 { get; set; }
        public int BuildDays { get; set; }

        public SingleUpgrade(int goldAmount, MaterialSlot slot1, MaterialSlot slot2, MaterialSlot slot3, int buildDays)
        {
            Gold = goldAmount;
            Slot1 = slot1;
            Slot2 = slot2;
            Slot3 = slot3;
            BuildDays = buildDays;
        }
    }

    class MaterialSlot
    {
        public IDMaterials MaterialID { get; set; }
        public int Quantity { get; set; }
        public MaterialSlot(IDMaterials materialID, int quantity)
        {
            MaterialID = materialID;
            Quantity = quantity;
        }
    }
}