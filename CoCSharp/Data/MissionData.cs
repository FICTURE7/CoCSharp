namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from missions.csv.
    /// </summary>
    public class MissionData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="MissionData"/> class.
        /// </summary>
        public MissionData()
        {
            // Space
        }

        public string Name { get; set; }
        public string Dependencies { get; set; }
        public bool FirstStep { get; set; }
        public bool WarStates { get; set; }
        public bool WarTutorial { get; set; }
        public bool Deprecated { get; set; }
        public bool OpenInfo { get; set; }
        public bool ShowWarBase { get; set; }
        public bool ShowDonate { get; set; }
        public bool SwitchSides { get; set; }
        public bool OpenAchievements { get; set; }
        public string BuildBuilding { get; set; }
        public int BuildBuildingLevel { get; set; }
        public int BuildBuildingCount { get; set; }
        public string DefendNPC { get; set; }
        public string AttackNPC { get; set; }
        public bool AttackPlayer { get; set; }
        public bool ChangeName { get; set; }
        public int Delay { get; set; }
        public int TrainTroops { get; set; }
        public bool ShowMap { get; set; }
        public string TutorialText { get; set; }
        public int TutorialStep { get; set; }
        public bool Darken { get; set; }
        public string TutorialTextBox { get; set; }
        public string TutorialCharacter { get; set; }
        public string CharacterSWF { get; set; }
        public string SpeechBubble { get; set; }
        public bool RightAlignTextBox { get; set; }
        public string ButtonText { get; set; }
        public string TutorialMusic { get; set; }
        public string TutorialMusicAlt { get; set; }
        public string TutorialSound { get; set; }
        public string RewardResource { get; set; }
        public int RewardResourceCount { get; set; }
        public int RewardXP { get; set; }
        public string RewardTroop { get; set; }
        public int RewardTroopCount { get; set; }
        public int CustomData { get; set; }
        public bool ShowGooglePlusSignin { get; set; }
        public bool HideGooglePlusSignin { get; set; }
        public bool ShowInstructor { get; set; }
    }
}
