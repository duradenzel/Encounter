namespace EncounterModels
{
    public class Monster
    {

        public string Name { get; set; }
        public string Desc { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Group { get; set; }
        public string Alignment { get; set; }
        public int ArmorClass { get; set; }
        public string ArmorDesc { get; set; }
        public int HitPoints { get; set; }
        public string HitDice { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int? StrengthSave { get; set; }
        public int? DexteritySave { get; set; }
        public int? ConstitutionSave { get; set; }
        public int? IntelligenceSave { get; set; }
        public int? WisdomSave { get; set; }
        public int? CharismaSave { get; set; }
        public int? Perception { get; set; }
        public Dictionary<string, int> Skills { get; set; }
        public string DamageVulnerabilities { get; set; }
        public string DamageResistances { get; set; }
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string ChallengeRating { get; set; }
        public double CR { get; set; }
        public int ExperiencePoints { get; set; }

        public string ImgMain { get;set; }
        

    }
}