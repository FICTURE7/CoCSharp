using CoCSharp.Networking.Packets.Commands;
using System;
using System.Collections.Generic;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Provides methods to create <see cref="ICommand"/> instances.
    /// </summary>
    public static class CommandFactory
    {
        static CommandFactory()
        {
            // Populates m_CommandDictionary with packet ids and types          
            // Generated from gen_commanddict.py

            m_CommandDictionary.Add(new BoostBuildingCommand().ID, typeof(BoostBuildingCommand));
            m_CommandDictionary.Add(new BuyBuildingCommand().ID, typeof(BuyBuildingCommand));
            m_CommandDictionary.Add(new BuyDecorationCommand().ID, typeof(BuyDecorationCommand));
            m_CommandDictionary.Add(new BuyResourcesCommand().ID, typeof(BuyResourcesCommand));
            m_CommandDictionary.Add(new BuyShieldCommand().ID, typeof(BuyShieldCommand));
            m_CommandDictionary.Add(new BuyTrapCommand().ID, typeof(BuyTrapCommand));
            m_CommandDictionary.Add(new CancelConstructionCommand().ID, typeof(CancelConstructionCommand));
            m_CommandDictionary.Add(new CancelHeroUpgradeCommand().ID, typeof(CancelHeroUpgradeCommand));
            m_CommandDictionary.Add(new CancelUnitProductionCommand().ID, typeof(CancelUnitProductionCommand));
            m_CommandDictionary.Add(new CancelUpgradeUnitCommand().ID, typeof(CancelUpgradeUnitCommand));
            m_CommandDictionary.Add(new CastSpellCommand().ID, typeof(CastSpellCommand));
            m_CommandDictionary.Add(new ClaimAchievementRewardCommand().ID, typeof(ClaimAchievementRewardCommand));
            m_CommandDictionary.Add(new ClearObstacleCommand().ID, typeof(ClearObstacleCommand));
            m_CommandDictionary.Add(new CollectResourcesCommand().ID, typeof(CollectResourcesCommand));
            m_CommandDictionary.Add(new FreeWorkerCommand().ID, typeof(FreeWorkerCommand));
            m_CommandDictionary.Add(new LoadTurrentCommand().ID, typeof(LoadTurrentCommand));
            m_CommandDictionary.Add(new MissionProgressCommand().ID, typeof(MissionProgressCommand));
            m_CommandDictionary.Add(new MoveBuildingCommand().ID, typeof(MoveBuildingCommand));
            m_CommandDictionary.Add(new PlaceAttackerCommand().ID, typeof(PlaceAttackerCommand));
            m_CommandDictionary.Add(new RequestAllianceUnitsCommand().ID, typeof(RequestAllianceUnitsCommand));
            m_CommandDictionary.Add(new SellBuildingCommand().ID, typeof(SellBuildingCommand));
            m_CommandDictionary.Add(new SpeedUpClearingCommand().ID, typeof(SpeedUpClearingCommand));
            m_CommandDictionary.Add(new SpeedUpConstructionCommand().ID, typeof(SpeedUpConstructionCommand));
            m_CommandDictionary.Add(new SpeedUpHeroHealthCommand().ID, typeof(SpeedUpHeroHealthCommand));
            m_CommandDictionary.Add(new SpeedUpTrainingCommand().ID, typeof(SpeedUpTrainingCommand));
            m_CommandDictionary.Add(new SpeedUpUpgradeHeroCommand().ID, typeof(SpeedUpUpgradeHeroCommand));
            m_CommandDictionary.Add(new SpeedUpUpgradeUnitCommand().ID, typeof(SpeedUpUpgradeUnitCommand));
            m_CommandDictionary.Add(new ToggleAttackModeCommand().ID, typeof(ToggleAttackModeCommand));
            m_CommandDictionary.Add(new ToggleHeroSleepCommand().ID, typeof(ToggleHeroSleepCommand));
            m_CommandDictionary.Add(new TrainUnitCommand().ID, typeof(TrainUnitCommand));
            m_CommandDictionary.Add(new UnlockBuildingCommand().ID, typeof(UnlockBuildingCommand));
            m_CommandDictionary.Add(new UpgradeBuildingCommand().ID, typeof(UpgradeBuildingCommand));
            m_CommandDictionary.Add(new UpgradeHeroCommand().ID, typeof(UpgradeHeroCommand));
            m_CommandDictionary.Add(new UpgradeUnitCommand().ID, typeof(UpgradeUnitCommand));
        }

        private static Dictionary<int, Type> m_CommandDictionary = new Dictionary<int, Type>();

        /// <summary>
        /// Creates a new <see cref="ICommand"/> instance with the specified command ID.
        /// </summary>
        /// <param name="id">ID of command.</param>
        /// <returns>Instance of <see cref="ICommand"/> created.</returns>
        public static ICommand Create(int id)
        {
            var commandType = (Type)null;
            if (!m_CommandDictionary.TryGetValue(id, out commandType))
                throw new Exception("Unknown command type!"); // can't make UnknownCommand cause we dont know the length
            return (ICommand)Activator.CreateInstance(commandType);
        }

        /// <summary>
        /// Tries to creates a new <see cref="ICommand"/> instance with the specified command ID.
        /// </summary>
        /// <param name="id">The ID of the command to create the instance.</param>
        /// <param name="command">The instance <see cref="ICommand"/> created, returns null if failed to create the instance.</param>
        /// <returns><see cref="true"/> if the instance was created successfully.</returns>
        public static bool TryCreate(int id, out ICommand command)
        {
            var commandType = (Type)null;
            if (!m_CommandDictionary.TryGetValue(id, out commandType))
            {
                command = null;
                return false;
            }
            command = (ICommand)Activator.CreateInstance(commandType);
            return true;
        }
    }
}
